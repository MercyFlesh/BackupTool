using System.Text.Json.Serialization;

namespace BackupTool
{
    public class Settings
    {
        [JsonPropertyName("Logging")]
        public LogSettings? LogSettings { get; init; }

        [JsonPropertyName("Backup")]
        public BackupSettings? BackupSettings { get; init; }
    }

    public class LogSettings
    {
        public string LogLevel { get; init; } = null!;

        public string LogFile { get; init; } = null!;
    }

    public class BackupSettings
    {
        public List<string> SourceDirs { get; init; }
        public string TargetDir { get; init; }
        public bool Overwrite { get; init; } = true;
    }
}
