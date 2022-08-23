using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupTool
{
    public class Backupper
    {
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
