using System.Threading.Channels;

namespace Gateway_ocelot_Solution.ApiLogs.PostgresLog
{
    public class usingPostgresLog
    {
        public static void InitialPostgresLog_builder(WebApplicationBuilder builder)
        {

            // สร้างตาราง ApiLogs ถ้ายังไม่มี
            //using var scope = builder.Services.BuildServiceProvider().CreateScope();
            //var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            //var connectionString = config.GetConnectionString("PostgresLogDb");

            //using var conn = new NpgsqlConnection(connectionString);
            //conn.Open();
            //using var cmd = new NpgsqlCommand(@"
            //                                    CREATE TABLE IF NOT EXISTS ""ApiLogs"" (
            //                                        ""Id"" SERIAL PRIMARY KEY,
            //                                        ""Timestamp"" TIMESTAMP NOT NULL DEFAULT NOW(),
            //                                        ""Level"" TEXT,
            //                                        ""Message"" TEXT,
            //                                        ""Request"" JSONB,
            //                                        ""Response"" JSONB,
            //                                        ""Path"" TEXT,
            //                                        ""Method"" TEXT,
            //                                        ""StatusCode"" INTEGER,
            //                                        ""Exception"" TEXT
            //                                    )", conn);

            //cmd.ExecuteNonQuery();
            builder.Services.AddSingleton(Channel.CreateUnbounded<LogEntry>());
            builder.Services.AddSingleton<IPostgresApiLogsService, PostgresApiLogsService>();
            builder.Services.AddHostedService<PostgresLogWriter>();
            builder.Services.AddHostedService<PostgresLogCleanupService>();



        }
        public static void InitialPostgresLog_app(WebApplication app)
        {


            app.UseMiddleware<PostgresApiLogsHandlerMiddleware>();
        }
    }
}
