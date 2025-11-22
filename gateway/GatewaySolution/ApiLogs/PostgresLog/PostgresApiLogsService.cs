using System.Threading.Channels;

namespace Gateway_ocelot_Solution.ApiLogs.PostgresLog
{
    public interface IPostgresApiLogsService
    {
        void Log(LogEntry entry);
    }
    public class PostgresApiLogsService : IPostgresApiLogsService
    {
        private readonly Channel<LogEntry> _logQueue;

        public PostgresApiLogsService(Channel<LogEntry> logQueue)
        {
            _logQueue = logQueue;
        }

        public void Log(LogEntry entry)
        {
            _logQueue.Writer.TryWrite(entry);
        }
    }
}
