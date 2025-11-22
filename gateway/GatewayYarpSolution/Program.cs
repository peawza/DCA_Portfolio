using Gateway_Yarp_Solution.Middleware;
using GatewaySolution.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Load reverseproxy.json
builder.Configuration.AddJsonFile("reverseproxy.json", optional: false, reloadOnChange: true);

// Authentication (แทน AddAppAuthetication)
builder.AddAppAuthetication();

// Redis setup
Utils.Startup.UseRedis(builder);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Load YARP configuration
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");

// Forward headers (optional, useful in proxy scenarios)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Logging incoming path (debugging)
app.Use(async (context, next) =>
{
    Console.WriteLine("Incoming Path: " + context.Request.Path);
    await next();
});

// JWT Redis Validation Middleware
app.UseMiddleware<JwtRedisValidationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Proxy via YARP
app.MapReverseProxy();

await app.RunAsync();