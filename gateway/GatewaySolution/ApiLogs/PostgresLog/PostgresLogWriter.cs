using Newtonsoft.Json;
using Npgsql;
using System.Threading.Channels;

namespace Gateway_ocelot_Solution.ApiLogs.PostgresLog
{
    public class PostgresLogWriter : BackgroundService
    {
        private readonly Channel<LogEntry> _logQueue;
        private readonly string _connectionString;

        public PostgresLogWriter(Channel<LogEntry> logQueue, IConfiguration config)
        {
            _logQueue = logQueue;
            _connectionString = config.GetConnectionString("PostgresLogDb");
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
                    using var conn = new NpgsqlConnection(_connectionString);
                    await conn.OpenAsync(token);

                    using var writer = conn.BeginTextImport(@"
                                                            COPY ""ApiLogs"" (
                                                                ""Timestamp"",
                                                                ""Level"",
                                                                ""Message"",
                                                                ""Request"",
                                                                ""Response"",
                                                                ""Path"",
                                                                ""Method"",
                                                                ""StatusCode"",
                                                                ""RequestSizeKb"",
                                                                ""ResponseSizeKb"",
                                                                ""Exception""
                                                            ) FROM STDIN (FORMAT csv, DELIMITER '|')");

                    foreach (var log in logs)
                    {
                        var csv = string.Join("|", new[]
                        {
                        EscapeCsv(log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                        EscapeCsv(log.Level),
                        EscapeCsv(log.Message),
                        EscapeCsv(JsonConvert.SerializeObject(log.Request)),
                        EscapeCsv(JsonConvert.SerializeObject(log.Response)),
                        EscapeCsv(log.Path),
                        EscapeCsv(log.Method),
                        EscapeCsv(log.StatusCode?.ToString()),
                        EscapeCsv(log.RequestSizeKb.ToString("F2")),
                        EscapeCsv(log.ResponseSizeKb.ToString("F2")),
                        EscapeCsv(log.Exception)
                    });

                        await writer.WriteLineAsync(csv);
                    }

                    await writer.DisposeAsync();
                    return;
                }
                catch
                {
                    retryCount++;
                    await Task.Delay(TimeSpan.FromSeconds(2 * retryCount), token);
                }
            }

            // Fallback to file if DB fails
            try
            {
                File.AppendAllText("failed-logs.txt", JsonConvert.SerializeObject(logs) + Environment.NewLine);
            }
            catch { }
        }


        private static string EscapeCsv(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            input = input.Replace("|", "/"); // Replace delimiter
            input = input.Replace("\"", "\"\""); // Escape double quotes

            // Wrap in quotes if needed
            if (input.Contains(',') || input.Contains('\n') || input.Contains('"'))
                input = $"\"{input}\"";

            return input;
        }
    }
}
