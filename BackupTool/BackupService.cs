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

        public void Backup(string sourceDir, string targetDir, bool overwrite)
        {
            ArgumentNullException.ThrowIfNull(sourceDir, nameof(sourceDir));
            ArgumentNullException.ThrowIfNull(targetDir, nameof(targetDir));

            _logger.LogInformation($"Copying source folder {sourceDir} to {targetDir}");

            if (!CheckedValidDirs(sourceDir, targetDir))
            {
                _logger.LogInformation($"Copying source folder {sourceDir} is faile");
                return;
            }

            var backupDirPath = Path.Combine(targetDir, Path.GetFileName(sourceDir));
            if (!CreateDir(backupDirPath))
            {
                _logger.LogInformation($"Failed to create backup path folder {backupDirPath}");
                return;
            }

            foreach (var dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                CreateDir(dirPath.Replace(sourceDir, backupDirPath));
            }

            foreach (var filePath in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                CopyFile(filePath, filePath.Replace(sourceDir, backupDirPath), overwrite);
            }

            _logger.LogInformation($"Copying source folder {sourceDir} is done");
        }

        public bool CheckedValidDirs(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                _logger.LogError($"Directory {sourceDir} does not exist");
                return false;
            }

            if (sourceDir == targetDir)
            {
                _logger.LogError($"Source directory {sourceDir} is the same as target");
                return false;
            }

            if (targetDir == Path.GetDirectoryName(sourceDir) + @"\")
            {
                _logger.LogError($"Source directory {sourceDir} is the subdirectory target {targetDir}");
                return false;
            }

            if (targetDir.StartsWith($"{sourceDir}"))
            {
                _logger.LogError($"Target directory {targetDir} is the subdirectory source {sourceDir}");
                return false;
            }

            return true;
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

        public bool CopyFile(string sourceFileName, string targetPath, bool overwrite)
        {
            try
            {
                _logger.LogDebug($"File {sourceFileName} is copying to {targetPath}");
                File.Copy(sourceFileName, targetPath, overwrite);
                _logger.LogDebug($"File {sourceFileName} copied success");
                return true;
            }
            catch(UnauthorizedAccessException)
            {
                _logger.LogError($"Not permissions to access the file {sourceFileName}");
            }
            catch(FileNotFoundException)
            {
                _logger.LogError($"Source file {sourceFileName} was not found");
            }
            catch(IOException)
            {
                _logger.LogError($"File with name {targetPath} already exists and overwriting is disabled");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to copy file: {ex.Message}");
            }

            return false;
        }
    }
}
