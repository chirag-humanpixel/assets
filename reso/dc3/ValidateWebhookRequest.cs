using Business.Entity;
using Business.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ResoWebApi.Common;
using ResoWebApi.Common.Enums;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Business.KeyPay
{
    public class ValidateWebhookRequest : IValidateWebhookRequest
    {
        #region Properties
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IKeyPayDatabaseService _keyPayDatabaseService;
        #endregion

        public ValidateWebhookRequest(IConfiguration configuration, ILogger logger, IKeyPayDatabaseService keyPayDatabaseService)
        {
            _logger = logger;
            _configuration = configuration;
            _keyPayDatabaseService = keyPayDatabaseService;
        }

        public (bool, long) ValidateKeyPayRequest(string payload, string receivedSignature, string queryString)
        {
            var result = false;
            long businessId = 0;
            try
            {
                businessId = GetBusinessIdFromQueryString(queryString);
                var eventType = GetEventType(payload);
                if (businessId > 0 && eventType.HasValue)
                {
                    var encoder = new UTF8Encoding();
                    var webhook = _keyPayDatabaseService.GetWebhookDetail(businessId, eventType.Value);
                    webhook.Wait();
                    var webhookDetail = webhook.Result;
                    if (webhookDetail != null)
                    {
                        using var hashValue = new HMACSHA256(encoder.GetBytes(webhookDetail.SecretKey));
                        var signature = hashValue.ComputeHash(encoder.GetBytes(payload));
                        var expectedSignature = BitConverter.ToString(signature).Replace("-", "");
                        result = expectedSignature == receivedSignature.Replace("sha256=", "");
                    }
                }
             }
            catch (Exception exception)
            {
                _logger.Error("ValidateKeyPayRequest: Error while validating request.", exception);
            }

            return (result, businessId);
        }

        private long GetBusinessIdFromQueryString(string queryString)
        {
            var stringBusinessId = HttpUtility.ParseQueryString(queryString).Get("businessId");
            long.TryParse(stringBusinessId, out long businessId);
            return businessId;
        }

        private EventType? GetEventType(string payload)
        {
            var employeeData = JsonConvert.DeserializeObject<KeyPayWebhookPayload>(payload);
            return employeeData?.EmployeeDetails?.FirstOrDefault()?.Action;
        }
    }
}
