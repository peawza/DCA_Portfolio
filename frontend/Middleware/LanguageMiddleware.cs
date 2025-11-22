using System.Globalization;
using System.Security.Claims;

namespace WEB.APP.Serilog
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate next;

        public LanguageMiddleware(RequestDelegate next)
        {
            this.next = next;
        }


        public Task Invoke(HttpContext context)
        {

            ClaimsPrincipal user = context.User;
            //UserData data = GetUserData(user);
            var LanguageCode = context.User.FindFirst("LanguageCode")?.Value ?? "en";


            if (LanguageCode == "th")
            {
                //CultureInfo.CurrentCulture = new CultureInfo("th-TH", false);
                CultureInfo.CurrentCulture = new CultureInfo("en-EN", false);
                CultureInfo.CurrentUICulture = new CultureInfo("th-TH", false);
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

            }
            else
            {
                CultureInfo.CurrentCulture = new CultureInfo("en-EN", false);
                CultureInfo.CurrentUICulture = new CultureInfo("en-EN", false);
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

            }


            return next(context);
        }
    }
}
