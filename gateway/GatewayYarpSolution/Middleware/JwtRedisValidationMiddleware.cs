using Gateway_Yarp_Solution.Models;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Gateway_Yarp_Solution.Middleware
{
    public class JwtRedisValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtRedisValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConnectionMultiplexer redis)
        {
            var allowAnonymousPaths = new[] { "/api/public/", "/health", "/swagger" };
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            if (allowAnonymousPaths.Any(p => path.StartsWith(p)))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var typeService = jwtToken.Claims.FirstOrDefault(c => c.Type == "typeService")?.Value;
            var device = jwtToken.Claims.FirstOrDefault(c => c.Type == "device")?.Value ?? "web";

            if (typeService == "micoService")
            {
                await _next(context);
                return;
            }

            var db = redis.GetDatabase();
            var redisValue = await db.StringGetAsync($"jwt:{userId}:{device}");
            if (redisValue.IsNullOrEmpty)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token not found in Redis");
                return;
            }

            var redisData = JsonSerializer.Deserialize<RedisJwtInfo>(redisValue);
            //string connectionString = "Host=127.0.0.1;Port=22153;Username=postgres;Password=P@ssw0rd@M3$!";
            context.Request.Headers.TryAdd("X-ConnectionStringDB", redisData.ConnectionStringDB);
            if (redisData == null || redisData.Token != token)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token mismatch or expired");
                return;
            }

            await _next(context);
        }
    }

}
