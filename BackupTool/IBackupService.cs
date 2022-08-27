namespace BackupTool
{
    public interface IBackupService
    {
        void Backup(string baseDir, string targetDir, bool overwrite);
    }
}
