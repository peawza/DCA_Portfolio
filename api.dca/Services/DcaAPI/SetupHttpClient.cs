using Utils.Extensions;

namespace BusinessAPI
{
    public class SetupHttpClient
    {
        public static string InsertLogsApiBase { get; set; }

        public static string ProductionMasterAPIBase { get; set; }

        public static string ProductionOperationAPIBase { get; set; }
        public static string QualityOperationAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static void InitialService(WebApplicationBuilder builder)
        {



            builder.Services.AddTransient<MicroservicesHandler>();



        }
    }
}
