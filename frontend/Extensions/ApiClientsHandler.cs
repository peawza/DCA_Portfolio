using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace WEB.APP.Extensions
{
    public class ApiClientsHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiClientsHandler(
            IConfiguration configuration,
            IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["uacj-tms-access-token"];
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = GetToken();
            if (request.Headers.Authorization == null && !string.IsNullOrEmpty(token))
            {

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}

