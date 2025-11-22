
using WEB.APP.ApiClients;
using WEB.APP.Extensions;

namespace WEB.APP
{
    public class SetupHttpClient
    {
        public static string authApiBase { get; set; }

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static void InitialService(WebApplicationBuilder builder)
        {
            SetupHttpClient.authApiBase = builder.Configuration["ApiClients:authApiBase"];

            builder.Services.AddTransient<MicroservicesHandler>();
            builder.Services.AddTransient<ApiClientsHandler>();


            builder.Services.AddHttpClient<IauthApiClients, authApiClients>(client =>
            {
                client.BaseAddress = new Uri(authApiBase);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<MicroservicesHandler>();


            builder.Services.AddHttpClient("WithMicroservicesHandler", client =>
            {
                client.BaseAddress = new Uri(authApiBase);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<MicroservicesHandler>();

            builder.Services.AddHttpClient<IUserManagesGroupPermissionApiClients, UserManagesGroupPermissionApiClients>(client =>
            {
                client.BaseAddress = new Uri(authApiBase);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<MicroservicesHandler>();

        }
    }
}
