

using BusinessAPI.Repositories;
using BusinessAPI.Services;

namespace BusinessAPI
{
    public class StartupService
    {
        public static void InitialService(IServiceCollection services)
        {

            /* --- Services --- */

            services.AddTransient<ICommonRepository, CommonRepository>();



            services.AddTransient<ICommonService, CommonService>();
        }
    }
}
