using MongoDB.Driver;
using Newtonsoft.Json;
using System.Threading.Channels;
namespace Gateway_ocelot_Solution.ApiLogs.MongoLog
{
    public class MongoLogWriter : BackgroundService
    {
        private readonly Channel<LogEntry> _logQueue;
        private readonly IMongoCollection<LogEntry> _logCollection;

        public MongoLogWriter(Channel<LogEntry> logQueue, IConfiguration config)
        {
            _logQueue = logQueue;

            var client = new MongoClient(config.GetConnectionString("MongoLogDb"));
            var db = client.GetDatabase("mes-logs");
            _logCollection = db.GetCollection<LogEntry>("Logs");

            // ✅ Create TTL index to auto-expire logs after 30 days
            _logCollection.Indexes.CreateOne(new CreateIndexModel<LogEntry>(
                Builders<LogEntry>.IndexKeys.Ascending(e => e.Timestamp),
                new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(30) }
            ));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var buffer = new List<LogEntry>();
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

            while (!stoppingToken.IsCancellationRequested)
            {
                while (_logQueue.Reader.TryRead(out var log))
                {
                    buffer.Add(log);
                    if (buffer.Count >= 50)
                        break;
                }

                if (buffer.Count > 0)
                {
                    await WriteLogsAsync(buffer, stoppingToken);
                    buffer.Clear();
                }

                await timer.WaitForNextTickAsync(stoppingToken);
            }
        }

        private async Task WriteLogsAsync(List<LogEntry> logs, CancellationToken token)
        {
            int retryCount = 0;
            while (retryCount < 5 && !token.IsCancellationRequested)
            {
                try
                {
                    await _logCollection.InsertManyAsync(logs, cancellationToken: token);
                    return;
                }
                catch (MongoException)
                {
                    retryCount++;
                    await Task.Delay(TimeSpan.FromSeconds(2 * retryCount), token);
                }
            }

            // ❗ Fallback: Write to file if all retries fail
            try
            {
                File.AppendAllText("failed-logs.txt", JsonConvert.SerializeObject(logs) + System.Environment.NewLine);
            }
            catch
            {
                // Ignore
            }
        }
    }
}