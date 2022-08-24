using Microsoft.Extensions.Logging;

namespace BackupTool
{
    public class Backupper
    {
        private readonly ILogger _logger;

        public Backupper(ILogger<Backupper> logger)
        {
            _logger = logger;
        }

        public static void Backup(string baseDir, string targetDir)
        {
            ArgumentNullException.ThrowIfNull(baseDir, nameof(baseDir));
            ArgumentNullException.ThrowIfNull(targetDir, nameof(targetDir));

            if (baseDir == targetDir || !Directory.Exists(baseDir))
                return;

            var backupDirPath = Path.Combine(targetDir, Path.GetFileName(baseDir));
            Directory.CreateDirectory(backupDirPath);

            foreach (var dirPath in Directory.GetDirectories(baseDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(baseDir, backupDirPath));
            }

            foreach (var filePath in Directory.GetFiles(baseDir, "*", SearchOption.AllDirectories))
            {
                File.Copy(filePath, filePath.Replace(baseDir, backupDirPath), true);
            }
        }
    }
}
