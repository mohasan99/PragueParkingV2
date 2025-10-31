using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PragueParkingV2;

public static class GarageStorage
{
    private const string FileName = "garage.json";

    // Flat record for JSON
    private class ParkingRecord
    {
        public int Spot { get; set; }
        public string Type { get; set; } = "";
        public string Plate { get; set; } = "";
        public DateTimeOffset CheckIn { get; set; }   // saved original check-in
    }

    // Save all parked vehicles (including their original check-in)
    public static void Save(Garage garage)
    {
        var list = new List<ParkingRecord>();

        foreach (var spot in garage.Spots)
        {
            foreach (var vehicle in spot.Vehicles)
            {
                list.Add(new ParkingRecord
                {
                    Spot = spot.Number,
                    Type = vehicle.VehicleType,               // "Car" / "Motorcycle"
                    Plate = vehicle.RegistrationNumber,
                    CheckIn = vehicle.CheckInTime             // <-- keep original time
                });
            }
        }

        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
        Console.WriteLine("Saved.");
    }

    // Load vehicles and restore their check-in times
    public static Garage Load(int defaultSpots = 100, double spotCapacity = 1.0)
    {
        var garage = new Garage(defaultSpots, spotCapacity);

        if (!File.Exists(FileName))
        {
            Console.WriteLine("No save file found. Starting fresh.");
            return garage;
        }

        try
        {
            var json = File.ReadAllText(FileName);
            var list = JsonSerializer.Deserialize<List<ParkingRecord>>(json) ?? new List<ParkingRecord>();

            foreach (var record in list)
            {
                // Recreate with the saved check-in time
                Vehicle vehicle = record.Type.Equals("car", StringComparison.OrdinalIgnoreCase)
                    ? new Car(record.Plate, record.CheckIn)             // <-- pass CheckIn
                    : new Motorcycle(record.Plate, record.CheckIn);     // <-- pass CheckIn

                garage.TryParkOnSpot(vehicle, record.Spot);             // put back to original spot if it fits
            }

            Console.WriteLine("Loaded previous state.");
        }
        catch (Exception error)
        {
            Console.WriteLine($"Failed to load save file: {error.Message}");
        }

        return garage;
    }
}
