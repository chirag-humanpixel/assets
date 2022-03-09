using Business.Entity;
using Business.Interface;
using KeyPayV2.Au.Models.Webhook;
using Persistent.Interface.DBContext;
using Persistent.Interface.ExternalServices;
using ResoWebApi.Common;
using ResoWebApi.Common.Enums;
using ResoWebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.KeyPay
{
    public class KeyPayUpdateWebhookDetail : IKeyPayUpdateWebhookDetail
    {
        #region MyRegion
        private readonly ILogger _logger;
        private readonly IKeyPayWebhookService _keyPayWebhookService;
        private readonly IKeyPayStore _keyPayStore;
        private readonly IKeyPayService _keyPayService;
        #endregion

        #region Constructor
        public KeyPayUpdateWebhookDetail(ILogger logger, IKeyPayStore keyPayStore, IKeyPayWebhookService keyPayWebhookService, IKeyPayService keyPayService)
        {
            _logger = logger;
            _keyPayStore = keyPayStore;
            _keyPayWebhookService = keyPayWebhookService;
            _keyPayService = keyPayService;
        }
        #endregion

        public async Task Execute()
        {
            await Task.Run(async () =>
            {
                var businessDetail = await _keyPayService.GetBusinessList();
                var businessDetailModel = MapBusinessDTO(businessDetail);
                businessDetailModel = await _keyPayStore.UpdateBusinessDetail(businessDetailModel);
                await UpdateWebhookDetail(businessDetailModel);
            });
        }

        private async Task UpdateWebhookDetail(List<KeyPayBusiness> businessDetail)
        {
            if (businessDetail?.Count > 0)
            {
                foreach (var business in businessDetail)
                {
                    try
                    {
                        var webhookDetail = await _keyPayWebhookService.GetWebhookList(Convert.ToInt32(business.BusinessId));
                        if (webhookDetail?.Count > 0)
                        {
                            var webhookDTO = MapWebhookDTO(webhookDetail, business.Id);
                            await _keyPayStore.UpdateWebhookDetail(webhookDTO);
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.Error("Got an error while updating webhook details", exception);
                    }
                }
            }
        }

        private List<KeyPayBusiness> MapBusinessDTO(List<KeyPayBusinessDetail> keyPayBusinessDetails)
        {
            var keyPayBusinessDTO = new List<KeyPayBusiness>();
            if (keyPayBusinessDetails?.Count > 0)
            {
                foreach (var business in keyPayBusinessDetails)
                {
                    keyPayBusinessDTO.Add(new KeyPayBusiness
                    {
                        BusinessId = business.BusinessId,
                        BusinessName = business.BusinessName,
                        ABNNumber = business.ABNNumber,
                        ContactEmailAddress = business.ContactEmailAddress,
                        ContactName = business.ContactName,
                        ContactPhoneNumber = business.ContactPhoneNumber
                    });
                }
            }

            return keyPayBusinessDTO;
        }

        private List<KeyPayWebhookDetail> MapWebhookDTO(List<WebHook> webhooksDetail, long businessId)
        {
            var webhookDetail = new List<KeyPayWebhookDetail>();
            foreach (var webhook in webhooksDetail)
            {
                foreach (var eventType in webhook.Filters)
                {
                    webhookDetail.Add(new KeyPayWebhookDetail
                    {
                        WebhookId = webhook.Id,
                        BusinessId = businessId,
                        EventType = eventType.GetEnumFromValue<EventType>(),
                        Description = webhook.Description,
                        SecretKey = webhook.Secret,
                        WebhookUrl = webhook.WebHookUri.AbsoluteUri.ToString(),
                        IsPaused = webhook.IsPaused,
                    });
                }
            }

            return webhookDetail;
        }
    }
}
