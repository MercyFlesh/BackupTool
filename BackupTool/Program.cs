using System.IO;
using System.Text.Json.Nodes;

try
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
