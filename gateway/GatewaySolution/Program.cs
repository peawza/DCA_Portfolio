using Gateway_Yarp_Solution.Middleware;
using GatewaySolution.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthetication();

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





builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);


//builder.Services.AddOcelot(builder.Configuration).AddDelegatingHandler<JwtRedisValidationHandler>(true);

builder.Services.AddOcelot(builder.Configuration);
//usingPostgresLog.InitialPostgresLog_builder(builder);
var app = builder.Build();
//usingPostgresLog.InitialPostgresLog_app(app);
app.UseCors("AllowAll");

app.UseMiddleware<JwtRedisValidationMiddleware>();


app.UseAuthentication();
app.UseAuthorization();
//app.MapGet("/", () => "Hello World!");
app.UseOcelot().GetAwaiter().GetResult();
await app.RunAsync();
