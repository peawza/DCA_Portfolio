using Gateway_ocelot_Solution.Models;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace Gateway_ocelot_Solution.Handler
{
    public class JwtRedisValidationHandler : DelegatingHandler
    {
        private readonly IConnectionMultiplexer _redis;

        public JwtRedisValidationHandler(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath.ToLower() ?? "";
            var allowAnonymousPaths = new[] { "/api/public/", "/health", "/swagger" };

            if (allowAnonymousPaths.Any(p => path.StartsWith(p)))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var token = request.Headers.Authorization?.Parameter;
            if (string.IsNullOrEmpty(token))
                return await base.SendAsync(request, cancellationToken);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var typeService = jwtToken.Claims.FirstOrDefault(c => c.Type == "typeService")?.Value;
            var device = jwtToken.Claims.FirstOrDefault(c => c.Type == "device")?.Value ?? "web";

            if (typeService == "micoService")
                return await base.SendAsync(request, cancellationToken);

            var db = _redis.GetDatabase();
            var redisValue = await db.StringGetAsync($"jwt:{userId}:{device}");

            if (redisValue.IsNullOrEmpty)
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var redisData = JsonSerializer.Deserialize<RedisJwtInfo>(redisValue);

            if (redisData == null || redisData.Token != token)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("Token mismatch or expired")
                };
            }

            // ✅ Append connection string as header for downstream services
            //string connectionString = "Host=127.0.0.1;Port=22153;Username=postgres;Password=P@ssw0rd@M3$!";
            request.Headers.TryAddWithoutValidation("X-ConnectionStringDB", redisData.ConnectionStringDB);
            request.Headers.TryAddWithoutValidation("X-CustomerID", redisData.CustomerID.ToString());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
