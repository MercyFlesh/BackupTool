using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BackupTool;
using BackupTool.FileLogger;


using var serviceProvider = new ServiceCollection()
    .AddLogging(loggerBuilder => 
    {
        loggerBuilder
            .AddProvider(new FileLoggerProvider(""))
            .AddConsole();
    })
    .AddSingleton<IBackupService, BackupService>()
    .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("Start BackupTool");

    var json = File.ReadAllText("settings.json");
    var settingsObj = JsonNode.Parse(json);

    var backupService = serviceProvider.GetService<IBackupService>();
    var target_dir = (string)settingsObj["targetDir"];

    if (settingsObj["baseDirs"]?.AsArray() is { } base_dirs)
    {
        foreach (var dir in base_dirs)
            backupService?.Backup((string)dir, target_dir);
    }
    else
    {
        logger.LogInformation("baseDirs parameter is null, no base directory to copy");
    }
}
catch (FileNotFoundException ex)
{
    logger.LogError($"Could not find settings file {ex.FileName}");
}
catch (JsonException ex)
{
    logger.LogError($"Invalid settings json file, error line number: {ex.LineNumber}");
}
catch (ArgumentNullException ex)
{
    logger.LogError($"Invalid value in settings, directory {ex.ParamName} is null");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
finally
{
    logger.LogInformation("BackupTool stoped");
}
