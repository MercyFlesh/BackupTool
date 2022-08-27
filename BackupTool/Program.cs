using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using BackupTool;
using BackupTool.FileLogger;


if (!File.Exists("settings.json"))
{
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write("fail");
    Console.ResetColor();
    Console.WriteLine($": Could not find settings file settings.json");
    Environment.Exit(-1);
}

var json = File.ReadAllText("settings.json");
var settings = JsonSerializer.Deserialize<Settings>(json);

using var serviceProvider = new ServiceCollection()
    .AddLogging(loggerBuilder => 
    {
        var logSettings = settings?.LogSettings;
        var logLevel = logSettings?.LogLevel is null 
            ? LogLevel.Information 
            : (LogLevel)Enum.Parse(typeof(LogLevel), logSettings.LogLevel);
        
        loggerBuilder
            .AddProvider(new FileLoggerProvider(logSettings?.LogFileDir ?? Environment.CurrentDirectory))
            .AddConsole()
            .SetMinimumLevel(logLevel);
    })
    .AddSingleton<IBackupService, BackupService>()
    .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
try
{
    logger.LogInformation("Start BackupTool");

    if (settings?.BackupSettings is not { } backupSettings)
    {
        logger.LogInformation("No Backup section in settings");
    }
    else
    {
        if (backupSettings.SourceDirs is not { } sourceDirs)
        {
            logger.LogInformation("sourceDirs parameter settings is null, no source directory to copy");
        }
        else
        {
            var backupService = serviceProvider.GetService<IBackupService>();
            foreach (var dir in sourceDirs)
                backupService?.Backup(dir, backupSettings.TargetDir, backupSettings.Overwrite);
        }
    }
}
catch (ArgumentNullException ex)
{
    logger.LogError($"Invalid value in settings, directory {ex.ParamName} is null");
}
catch (Exception ex)
{
    logger.LogError($"Unhandled critical error received during progress: {ex.Message}");
}
finally
{
    logger.LogInformation("BackupTool stoped");
}
