using Gateway_ocelot_Solution.ApiLogs.MongoLog;
using System.Threading.Channels;

namespace Gateway_ocelot_Solution.ApiLogs
{
    public class usingMongoLog
    {

        public static void InitialMongoLog(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(Channel.CreateUnbounded<LogEntry>());
            builder.Services.AddSingleton<IMongoApiLogsService, MongoApiLogsService>();
            builder.Services.AddHostedService<MongoLogWriter>();

        }
    }
}
