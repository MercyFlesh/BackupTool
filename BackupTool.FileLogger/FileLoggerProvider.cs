using Microsoft.Extensions.Logging;

namespace BackupTool.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string path;

        public FileLoggerProvider(string filePath = "")
        {
            path = filePath;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose()
        { }
    }
}
