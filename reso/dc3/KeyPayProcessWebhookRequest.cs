using Business.Entity;
using Business.Interface;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ResoWebApi.Common;
using ResoWebApi.Common.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Business.KeyPay
{
    public class KeyPayProcessWebhookRequest : IKeyPayProcessWebhookRequest
    {
        #region Properties
        private ILogger _logger;
        private static ConcurrentQueue<KeyPayWebhookEventBody> _payloadQueue;
        private static BackgroundWorker _processPayloadWorker;
        private Func<EventType, IKeyPayWebhookProcess> _keyPayWebhookProcess;
        private readonly IServiceProvider _services;
        private static bool IsWorkerRunning = false;
        #endregion

        #region Constructur
        public KeyPayProcessWebhookRequest(IServiceProvider services)
        {
            _services = services;
            //Confiure queue if not confiure
            CreateQueue();
            CreateServiceScoped();
        }
        #endregion

        #region Public Method
        public void ProcessRequest(string payloadString, long businessId)
        {
            try
            {
                var employees= JsonConvert.DeserializeObject<KeyPayWebhookPayload>(payloadString);
                foreach (var employee in employees.EmployeeDetails)
                {
                    _payloadQueue.Enqueue(new KeyPayWebhookEventBody()
                    {
                        EventType = employee.Action,
                        EventBody = JsonConvert.SerializeObject(employee),
                        EventDateTime = DateTime.UtcNow,
                        BusinessId = businessId
                    });
                }
                if (!IsWorkerRunning)
                {
                    IsWorkerRunning = true;
                    _processPayloadWorker.RunWorkerAsync();
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"ProcessRequest: Error while publish keypay event for event : {payloadString}", exception);
            }
        }
        #endregion

        #region Private Method
        private void CreateQueue()
        {
            if (_payloadQueue == null)
            {
                _payloadQueue = new ConcurrentQueue<KeyPayWebhookEventBody>();
            }

            if (_processPayloadWorker == null)
            {
                _processPayloadWorker = new BackgroundWorker()
                {
                    // Configure background worker 
                    WorkerSupportsCancellation = true
                };
            }

            _processPayloadWorker.DoWork += ProcessPayload;
        }

        private void ProcessPayload(object sender, DoWorkEventArgs e)
        {
            ProcessPayloadQueue();
        }

        // Use method to process payloads
        private async void ProcessPayloadQueue()
        {
            while (_payloadQueue.Count > 0)
            {
                try
                {
                    _payloadQueue.TryDequeue(out KeyPayWebhookEventBody keyPayWebhookEventBody);
                    await _keyPayWebhookProcess(keyPayWebhookEventBody.EventType).Execute(keyPayWebhookEventBody);
                }
                catch (Exception exception)
                {
                    _logger.Error("ProcessPayloadQueue: Error while processing KeyPay webhook event", exception);
                }
            }
            IsWorkerRunning = false;
        }

        private void CreateServiceScoped()
        {
            var scope = _services.CreateScope();
            _keyPayWebhookProcess = scope.ServiceProvider.GetRequiredService<Func<EventType, IKeyPayWebhookProcess>>();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        }
        #endregion
    }
}
