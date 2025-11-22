using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using WEB.APP;
using WEB.APP.Extensions;
using WEB.APP.Localization;
using WEB.APP.MvcWebApp.Navigation;
using WEB.APP.Serilog;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;
ConfigureWebHostBuilder WebHost = builder.WebHost;
builder.Configuration.AddEnvironmentVariables();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:5137") // Explicitly allow frontend origin
//                  .AllowAnyHeader()
//                  .AllowAnyMethod()
//                  .AllowCredentials(); // Use only if credentials (cookies, auth headers) are required
//        });
//});
// Add services to the container.

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
})
.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
.AddRazorRuntimeCompilation();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddMemoryCache();
builder.Services.AddKendo();



builder.Services.AddScoped<ISiteMapProvider, DbSiteMapProvider>();



builder.Services.AddHttpClient();
builder.Services.AddSingleton<DbMessageLocalizer>();
builder.Services.AddSingleton<DbMessageResources>();
builder.Services.AddSingleton<IMessageLocalizer, DbMessageLocalizer>();
builder.Services.AddSingleton<IMessageResources, DbMessageResources>();

var redisHost = builder.Configuration["REDIS:HOST"];
var redisPort = builder.Configuration["REDIS:PORT"];
var redisAbortConnect = builder.Configuration["REDIS:ABORTCONNECT"];
var IdentityNameCookie = builder.Configuration["Identity:IdentityNameCookie"];

if (!string.IsNullOrEmpty(redisHost))
{
    var redisConnectionString = $"{redisHost}:{redisPort},abortConnect={redisAbortConnect}";
    var redis = ConnectionMultiplexer.Connect(redisConnectionString);
    try
    {
        builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys")
        .SetApplicationName(IdentityNameCookie);
    }
    catch (Exception)
    {


    }

}
else
{
    var keysPath = Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys");

    // สร้างโฟลเดอร์ถ้ายังไม่มี
    Directory.CreateDirectory(keysPath);

    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
        .SetApplicationName(IdentityNameCookie);
}

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/SignIn";
        options.LogoutPath = "/Account/SignOut";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = IdentityNameCookie;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });




builder.Services.AddHttpContextAccessor();

SetupHttpClient.InitialService(builder);


var app = builder.Build();


app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}


#region Set up Time Zone
var supportedCultures = new[] { "en" };

var cultureOptions = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<ApplicationCultureOptions>>();


var localizationOptions = new RequestLocalizationOptions()
{
    SupportedUICultures = cultureOptions.Value.GetCultures(supportedCultures),
    SupportedCultures = cultureOptions.Value.GetCultures("en")
}.SetDefaultCulture("en");
app.UseRequestLocalization(localizationOptions);
#endregion


//app.UseHttpsRedirection();
app.UseStaticFiles();


//app.UseCors("AllowAll");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.UseSession();



using (var scope = app.Services.CreateScope())
{
    var serviceMessage = scope.ServiceProvider.GetRequiredService<DbMessageLocalizer>();
    await serviceMessage.LoadResourceMessagesAsync(); // Fetch data only once


    var serviceResources = scope.ServiceProvider.GetRequiredService<DbMessageResources>();

    await serviceResources.LoadResourceMessagesAsync(); // Fetch data only once

    HtmlHelperExtensions.Configure(scope.ServiceProvider.GetRequiredService<IMessageResources>());

}
IServiceProvider services = builder.Services.BuildServiceProvider();
Resources resources = new Resources(environment, configuration, services);
resources.update_Resources_JS();


app.UseMiddleware<LanguageMiddleware>();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


await app.RunAsync();
