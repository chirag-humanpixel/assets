using Business.Interface;
using Persistent.Interface;
using ResoWebApi.Common;
using System.Threading.Tasks;

namespace Business.Xero
{
    public class XeroAuthentication : IXeroAuthentication
    {
        #region Propertyies
        private readonly ILogger _logger;
        private readonly IXeroCommon _xeroCommon;
        #endregion

        #region Constructor
        public XeroAuthentication(IXeroCommon xeroCommon, ILogger logger)
        {
            _xeroCommon = xeroCommon;
            _logger = logger;
        } 
        #endregion

        #region Public method
        public bool XeroTokenExists()
        {
            var requestOption = _xeroCommon.GetRequestOption("Demo Company (AU)").Result;
            return (requestOption != null &&
                    !string.IsNullOrEmpty(requestOption.AccessToken) &&
                    !string.IsNullOrEmpty(requestOption.AccessToken));

        }
        public async Task AuthenticateXeroAccount(string code, string state)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            {
                _logger.Error("Authenticate code/state of xero account is/are not found", null);
                return;
            }
            await _xeroCommon.AuthenticateXeroAccount(code, state);
        }

        public async Task DestroyXeroToken()
        {
            await _xeroCommon.DestroyXeroToken();
        }
        #endregion
    }
}
