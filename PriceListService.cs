using System;
using System.Globalization;
using System.IO;

namespace PragueParkingV2;

public static class PriceListService
{
    private const string FileName = "prices.txt";

    public static void LoadInto(AppConfiguration configuration)
    {
        // If the price file doesn't exist, create one with defaults
        if (!File.Exists(FileName))
        {
            string defaultText =
                "# Prague Parking price list (CZK/hour)\n" +
                "Car=20\n" +
                "Motorcycle=10\n" +
                "FreeMinutes=10\n";
            File.WriteAllText(FileName, defaultText);
            return;
        }

        // Read each line and process valid settings
        foreach (string line in File.ReadAllLines(FileName))
        {
            // Ignore comments (#) and empty lines
            string text = line.Split('#')[0].Trim();
            if (string.IsNullOrEmpty(text)) continue;

            string[] parts = text.Split('=', 2);
            if (parts.Length != 2) continue;

            string key = parts[0].Trim().ToLower();
            string valueText = parts[1].Trim().Replace(",", ".");
            if (!double.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                continue;

            // Apply the values to configuration
            if (key == "car") configuration.PricePerHourCar = value;
            else if (key == "motorcycle") configuration.PricePerHourMotorcycle = value;
            else if (key == "freeminutes") configuration.FreeMinutes = (int)value;
        }
    }
}
