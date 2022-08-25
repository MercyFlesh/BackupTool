using System.Text.Json.Nodes;
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
    var json = File.ReadAllText("settings.json");
    var settingsObj = JsonNode.Parse(json);
    
    var backupService = serviceProvider.GetService<IBackupService>();
    foreach (var dir in settingsObj["base_dirs"].AsArray())
        backupService.Backup((string)dir, (string)settingsObj["target_dir"]);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}