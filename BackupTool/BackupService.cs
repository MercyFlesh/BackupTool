using Microsoft.Extensions.Logging;

namespace BackupTool
{
    public class BackupService : IBackupService
    {
        private readonly ILogger _logger;

        public BackupService(ILogger<BackupService> logger)
        {
            _logger = logger;
            
        }

        public void Backup(string baseDir, string targetDir)
        {
            ArgumentNullException.ThrowIfNull(baseDir, nameof(baseDir));
            ArgumentNullException.ThrowIfNull(targetDir, nameof(targetDir));

            if (!Directory.Exists(baseDir))
            {
                _logger.LogError($"Directory {baseDir} does not exist");
                return;
            }

            if (baseDir == targetDir)
            {
                _logger.LogError($"Source directory {baseDir} is the same as target");
                return;
            }
                
            if (targetDir == Path.GetDirectoryName(baseDir) + @"\")
            {
                _logger.LogError($"Source directory {baseDir} is the subdirectory target {targetDir}");
                return;
            }

            if (targetDir.StartsWith($"{baseDir}"))
            {
                _logger.LogError($"Target directory {targetDir} is the subdirectory source {baseDir}");
                return;
            }

            var backupDirPath = Path.Combine(targetDir, Path.GetFileName(baseDir));
            if (!CreateDir(backupDirPath))
                return;

            foreach (var dirPath in Directory.GetDirectories(baseDir, "*", SearchOption.AllDirectories))
            {
                CreateDir(dirPath.Replace(baseDir, backupDirPath));
            }

            foreach (var filePath in Directory.GetFiles(baseDir, "*", SearchOption.AllDirectories))
            {
                File.Copy(filePath, filePath.Replace(baseDir, backupDirPath), true);
            }
        }

        public bool CreateDir(string dir)
        {
            try
            {
                _logger.LogDebug($"Directory {dir} creation");
                Directory.CreateDirectory(dir);
                return true;
            }
            catch(UnauthorizedAccessException)
            {
                _logger.LogError($"Not permission to create directory {dir}");
            }
            catch(PathTooLongException)
            {
                _logger.LogError($"Directory path {dir} exceeds maximum length");
            }
            catch(DirectoryNotFoundException)
            {
                _logger.LogError($"Specified directory path {dir} is invalid");
            }
            catch(Exception)
            {
                _logger.LogError($"Unsupported directory path format {dir}");
            }

            return false;
        }

        public bool CopyFile()
        {
            return true;
        }
    }
}
