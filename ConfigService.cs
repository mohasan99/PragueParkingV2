using System;
using System.IO;
using System.Text.Json;

namespace PragueParkingV2;

public static class ConfigService
{
    private const string FileName = "config.json";

    public static AppConfiguration Load()
    {
        // If config file doesn't exist, create a default one
        if (!File.Exists(FileName))
        {
            var defaultConfig = new AppConfiguration();
            var json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
            Console.WriteLine("Created default config.json");
            return defaultConfig;
        }

        try
        {
            // Read existing config
            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<AppConfiguration>(json) ?? new AppConfiguration();
        }
        catch
        {
            Console.WriteLine("Error reading config.json, using default settings.");
            return new AppConfiguration();
        }
    }
}
