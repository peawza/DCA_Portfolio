using System.Threading.Channels;

namespace Gateway_ocelot_Solution.ApiLogs.MongoLog
{
    public interface IMongoApiLogsService
    {
        void Log(LogEntry entry);
    }
    public class MongoApiLogsService : IMongoApiLogsService
    {
        private readonly Channel<LogEntry> _logQueue;

        public MongoApiLogsService(Channel<LogEntry> logQueue)
        {
            _logQueue = logQueue;
        }

        public void Log(LogEntry entry)
        {
            _logQueue.Writer.TryWrite(entry);
        }
    }
}
