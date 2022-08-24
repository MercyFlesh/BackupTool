using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BackupTool.FileLogger;


using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddProvider(new FileLoggerProvider(""))
        .AddConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("hello");

/*try
{
    var json = File.ReadAllText("settings.json");
    var settingsObj = JsonNode.Parse(json);

    foreach (var dir in settingsObj["base_dirs"].AsArray())
        BackupTool.Backupper.Backup((string)dir, (string)settingsObj["target_dir"]);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}


static IConfiguration BuildConfig(IConfigurationBuilder builder)
{
   return builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("settings.json")
        .Build();
}

static void ConfigureServices(ServiceCollection services)
{
    services.AddLogging(loggerBuilder =>
    {
        loggerBuilder
            .AddProvider(new FileLoggerProvider("testlog.txt"))
            .AddConsole();
    }).AddTransient<BackupTool.Backupper>();
}*/