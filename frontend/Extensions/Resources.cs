using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WEB.APP.ApiClients;
using WEB.APP.Localization;

namespace WEB.APP.Extensions
{

    public class Resources
    {
        private IMemoryCache _cache;
        private IWebHostEnvironment _hostingEnvironment;
        private IConfiguration _config;
        private IMessageResources _messageResources;
        private IMessageLocalizer _messageLocalizer;
        private readonly IauthApiClients _api;



        public Resources(IWebHostEnvironment hostingEnvironment, IConfiguration config, IServiceProvider _serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;

            //this._serviceProvider = _serviceCollection.GetRequiredService<IServiceProvider>();
            // this._dashboad = _serviceScope.ServiceProvider.GetRequiredService<IDashboadService>();
            //this._hub = _serviceScope.ServiceProvider.GetRequiredService<IHubContext<DashboadHub>>();
            using (var scope = _serviceProvider.CreateScope())
            {
                _messageResources = scope.ServiceProvider.GetRequiredService<IMessageResources>();
                _messageLocalizer = scope.ServiceProvider.GetRequiredService<IMessageLocalizer>();
                _api = scope.ServiceProvider.GetRequiredService<IauthApiClients>();
            }

        }

        //public void set_culture(string Set_culture)
        //{
        //    _messageLocalizer.set_culture(Set_culture);
        //    _messageResources.set_culture(Set_culture);
        //}

        public async void update_Resources_JS()
        {

            List<LocalizedResources> modelResources = _messageResources.GetAllStrings_All_Languages(true).ToList();

            var groupedByScreenCode = modelResources
            .GroupBy(resource => resource.ScreenCode)
            .ToDictionary(
                group => group.Key,
                group => group.ToList()
            );



            var file_Resources = _hostingEnvironment.GetContentPath($"~/wwwroot/assets/resources/resources.js");

            File.Delete(file_Resources);
            using (FileStream fs = new FileStream(file_Resources, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))

                {
                    await sw.WriteAsync("");
                    string FileJs = @"let _data_LocalizedResources =" + JsonConvert.SerializeObject(groupedByScreenCode, Formatting.Indented);

                    await sw.WriteAsync(FileJs);
                }
            }


            List<ResourceMessage> modelMessages = _messageLocalizer.GetAllStrings_All_Languages(true).ToList();

            var groupedByMessageType = modelMessages
            .GroupBy(resource => resource.MessageType)
            .ToDictionary(
                group => group.Key,
                group => group.ToList()
            );

            var file_Messages = _hostingEnvironment.GetContentPath($"~/wwwroot/assets/resources/messages.js");

            File.Delete(file_Messages);
            using (FileStream fs = new FileStream(file_Messages, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))

                {
                    await sw.WriteAsync("");
                    string FileJs = @"let _data_LocalizedMessages =" + JsonConvert.SerializeObject(groupedByMessageType, Formatting.Indented);

                    await sw.WriteAsync(FileJs);

                }


            }

            var file_Config = _hostingEnvironment.GetContentPath($"~/wwwroot/assets/resources/config.js");
            File.Delete(file_Config);
            using (FileStream fs = new FileStream(file_Config, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))

                {
                    await sw.WriteAsync("");
                    string _url_ = _config["ApiBase:url"];
                    string FileJs = $@"const _url_callapi = ""{_url_}"";";
                    await sw.WriteAsync(FileJs);

                }


            }

        }
    }
}
