using Business.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ResoWebApi.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResoWebApi.Controllers
{
    [Route("/webhooks")]
    public class KeyPayWebhookController : Controller
    {
        #region Properties
        private readonly ILogger _logger;
        private readonly IValidateWebhookRequest _validateWebhookRequest;
        private readonly IConfiguration _configuration;
        private readonly IKeyPayProcessWebhookRequest _keyPayProcessWebhookRequest;
        #endregion

        public KeyPayWebhookController(
            ILogger logger, 
            IValidateWebhookRequest validateWebhookRequest, 
            IConfiguration configuration, 
            IKeyPayProcessWebhookRequest keyPayProcessWebhookRequest)
        {
            _logger = logger;
            _validateWebhookRequest = validateWebhookRequest;
            _configuration = configuration;
            _keyPayProcessWebhookRequest = keyPayProcessWebhookRequest;
        }

        [HttpPost]
        public IActionResult Index()
        {
            var payloadString = GetRequestBody().Result.ToString();
            var signature = Request.Headers[_configuration.GetKeyPayConfig("KeyPaySignature")].FirstOrDefault();

            // Valid signature, enqueue payload to queue and start asynchronous processing of payload
            var (isValidate, businessId) = _validateWebhookRequest.ValidateKeyPayRequest(payloadString, signature, Request.QueryString.Value);
            if (isValidate)
            {
                _logger.Info($"Validate Request: {payloadString}");
                _keyPayProcessWebhookRequest.ProcessRequest(payloadString, businessId);
            }
            return Ok();
        }

        private async Task<string> GetRequestBody()
        {
            using var reader = new StreamReader(Request.Body);
            return await reader.ReadToEndAsync();
        }
    }
}
