using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace WEB.APP.Extensions
{
    public class MicroservicesHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        public MicroservicesHandler(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public string GenerateToken()
        {
            if (_cache.TryGetValue("MicroserviceToken_FrontEnd", out string? cachedToken) && !string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }

            var claims = new List<Claim>
            {
                new Claim("typeService", "micoService"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtExpireMinutes"]));

            var token = new JwtSecurityToken(
               issuer: _configuration["JwtIssuer"],
               audience: _configuration["JwtIssuer"],
               claims,
               expires: expires,
               signingCredentials: creds
           );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            // Cache Token จนกว่าจะหมดอายุ
            _cache.Set("MicroserviceToken_FrontEnd", tokenString, TimeSpan.FromMinutes(Convert.ToDouble(_configuration["JwtExpireMinutes"]) - 5));
            return tokenString;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = GenerateToken();
            if (request.Headers.Authorization == null && !string.IsNullOrEmpty(token))
            {

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}

