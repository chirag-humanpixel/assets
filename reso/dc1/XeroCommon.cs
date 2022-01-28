//using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Persistent.Entity.XeroEntity;
using Persistent.Interface;
using ResoWebApi.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xero.NetStandard.OAuth2.Client;
using Xero.NetStandard.OAuth2.Config;
using Xero.NetStandard.OAuth2.Token;

namespace Persistent
{
    public class XeroCommon : IXeroCommon
    {
        #region Properties
        private ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly IOptions<XeroConfiguration> _xeroConfig;
        private static readonly string serializedXeroTokenPath = "./xerotoken.json";
        #endregion

        #region private Constructor
        public XeroCommon(IOptions<XeroConfiguration> xeroConfig, IServiceProvider services)
        {
            _services = services;
            _xeroConfig = xeroConfig;
            CreateServiceScoped();
        }
        #endregion

        #region Private Method
        private static void StoreToken(XeroOAuth2Token xeroToken)
        {
            var serializedXeroToken = JsonConvert.SerializeObject(xeroToken);
            File.WriteAllText(serializedXeroTokenPath, serializedXeroToken);
        }

        private static void DeleteTokenFile()
        {
            File.Delete(serializedXeroTokenPath);
        }

        private static XeroOAuth2Token GetStoredToken()
        {
            return JsonConvert.DeserializeObject<XeroOAuth2Token>(File.ReadAllText(serializedXeroTokenPath));
        }

        private void GrantAccessOfAccount()
        {
            var client = new XeroClient(_xeroConfig.Value, new HttpClient());
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = client.BuildLoginUri()
            });
        }

        private void CreateServiceScoped()
        {
            var scope = _services.CreateScope();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        }
        #endregion

        #region Public Method
        public bool TokenExists()
        {
            return File.Exists(serializedXeroTokenPath);
        }

        public async Task DestroyXeroToken()
        {
            var xeroToken = GetStoredToken();
            var client = new XeroClient(_xeroConfig.Value, new HttpClient());
            await client.DeleteConnectionAsync(xeroToken, xeroToken.Tenants[0]);
            DeleteTokenFile();
            return;
        }

        public async Task AuthenticateXeroAccount(string code, string state)
        {
            var client = new XeroClient(_xeroConfig.Value, new HttpClient());
            var xeroToken = (XeroOAuth2Token)await client.RequestAccessTokenAsync(code);

            var tenants = await client.GetConnectionsAsync(xeroToken);

            StoreToken(xeroToken);
        }

        public async Task<XeroConfig> GetRequestOption(string tenantName)
        {
            try
            {
                if (TokenExists())
                {
                    var xeroToken = GetStoredToken();
                    var utcTimeNow = DateTime.UtcNow;
                    var httpClient = new HttpClient();
                    if (utcTimeNow > xeroToken.ExpiresAtUtc)
                    {
                        var client = new XeroClient(_xeroConfig.Value, httpClient);
                        xeroToken = (XeroOAuth2Token)await client.RefreshAccessTokenAsync(xeroToken);
                        StoreToken(xeroToken);
                    }

                    return new XeroConfig
                    {
                        AccessToken = xeroToken.AccessToken,
                        XeroTenantId = xeroToken.Tenants.Find(x => x.TenantName == tenantName)?.TenantId.ToString()
                    };
                }
                else
                {
                    GrantAccessOfAccount();
                }
            }
            catch (Exception exception)
            {
                GrantAccessOfAccount();
            }

            return null;
        }
        #endregion
    }
}
