using Microsoft.Extensions.Logging;

namespace BackupTool.FileLogger
{
    public class FileLogger : ILogger
    {
        private readonly string filePath;
        private static object lockObj = new object();

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
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
                lock(lockObj)
                {
                    string exc = "";
                    if (ex is not null) exc = $"\n{ex.GetType()}: {ex.Message}\n{ex.StackTrace}\n";
                        File.AppendAllText(filePath, $"{logLevel}: {DateTime.Now} {formatter(state, ex!)}\n{exc}");
                }
            }
        }
    }
}
