using Microsoft.Extensions.Logging;

namespace BackupTool.FileLogger
{
    public class FileLogger : ILogger
    {
        private readonly string savePath;
        private static object _lock = new object();

        public FileLogger(string savePath)
        {
            this.savePath = savePath;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception ex, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    string fullFilePath = Path.Combine(savePath, DateTime.Now.ToString("yyyy-MM-dd") + "_log.log");
                    string exc = "";
                    if (ex is not null) exc = $"\r\n{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}\r\n";
                    File.AppendAllText(fullFilePath, $"{logLevel}: {DateTime.Now} {formatter(state, ex!)}\r\n{exc}");
                }
            }
        }
    }
}
