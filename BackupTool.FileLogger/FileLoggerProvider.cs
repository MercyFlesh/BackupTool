using Microsoft.Extensions.Logging;

namespace BackupTool.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string filePath;

        public FileLoggerProvider(string savePath)
        {
            filePath = Path.Combine(savePath, $"{DateTime.Now.ToString("dd-MM-yyyy HH-mm")}.log");
            using var fs = File.Create(filePath);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(filePath);
        }

        public void Dispose()
        { }
    }
}
