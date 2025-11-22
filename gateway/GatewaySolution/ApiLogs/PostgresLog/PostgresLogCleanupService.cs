using Dapper;
using Npgsql;

namespace Gateway_ocelot_Solution.ApiLogs.PostgresLog
{
    public class PostgresLogCleanupService : BackgroundService
    {
        private readonly string _connectionString;

        public PostgresLogCleanupService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("PostgresLogDb");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromHours(24)); // cleanup ทุกวัน

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using var conn = new NpgsqlConnection(_connectionString);
                    await conn.ExecuteAsync("DELETE FROM \"ApiLogs\" WHERE \"Timestamp\" < NOW() - INTERVAL '30 days'");
                }
                catch
                {

                }
            }
        }
    }

}
