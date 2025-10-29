using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace PragueParkingV2;

public static class ConfigService
{
    private const string ConfigFile = "config.json";
    private const string PricesFile = "prices.txt";

    public static AppConfiguration LoadAll()
    {
        // 1) Load or create config.json
        AppConfiguration configuration;
        if (!File.Exists(ConfigFile))
        {
            configuration = new AppConfiguration();
            File.WriteAllText(ConfigFile,
                JsonSerializer.Serialize(configuration, new JsonSerializerOptions { WriteIndented = true }));
        }
        else
        {
            var json = File.ReadAllText(ConfigFile);
            configuration = JsonSerializer.Deserialize<AppConfiguration>(json) ?? new AppConfiguration();
        }

        // 2) Load or create prices.txt (overrides config values)
        LoadPricesInto(configuration);

        return configuration;
    }

    private static void LoadPricesInto(AppConfiguration configuration)
    {
        if (!File.Exists(PricesFile))
        {
            File.WriteAllText(PricesFile,
                "# Prague Parking price list (CZK/hour)\n" +
                "Car=20\n" +
                "Motorcycle=10\n" +
                "FreeMinutes=10\n");
            return;
        }

        foreach (var line in File.ReadAllLines(PricesFile))
        {
            var text = line.Split('#')[0].Trim();
            if (string.IsNullOrEmpty(text)) continue;

            var parts = text.Split('=', 2);
            if (parts.Length != 2) continue;

            var key = parts[0].Trim().ToLowerInvariant();
            var valueText = parts[1].Trim().Replace(",", ".");
            if (!double.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                continue;

            if (key == "car") configuration.PricePerHourCar = value;
            else if (key == "motorcycle") configuration.PricePerHourMotorcycle = value;
            else if (key == "freeminutes") configuration.FreeMinutes = (int)value;
        }
    }
}
