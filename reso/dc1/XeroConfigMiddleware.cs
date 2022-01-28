using Business.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ResoWebApi
{
    public class XeroConfigMiddleware
    {
        #region Property
        private IXeroAuthentication _xeroAuthentication;
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        #endregion

        public XeroConfigMiddleware(RequestDelegate next, ILoggerFactory logFactory, IServiceProvider services)
        {
            _next = next;
            _logger = logFactory.CreateLogger("XeroConfigMiddleware");
            _services = services;
            CreateServiceScoped();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _xeroAuthentication.XeroTokenExists();
            await _next(httpContext);
        }

        private void CreateServiceScoped()
        {
            var scope = _services.CreateScope();
            _xeroAuthentication = scope.ServiceProvider.GetRequiredService<IXeroAuthentication>();
        }
    }

    public static class XeroConfigMiddlewareExtensions
    {
        public static IApplicationBuilder UseXeroConfigMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<XeroConfigMiddleware>();
        }
    }
}
