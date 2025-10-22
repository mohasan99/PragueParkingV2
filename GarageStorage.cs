using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PragueParkingV2;

public static class GarageStorage
{
    private const string FileName = "garage.json";

    // Simple flat record for JSON
    private class ParkingRecord
    {
        public int Spot { get; set; }
        public string Type { get; set; } = "";
        public string Plate { get; set; } = "";
    }

    public static void Save(Garage garage) // saves all parked vehicles with their spot numbers
    {
        var list = new List<ParkingRecord>(); // flat list for serialization

        foreach (var spot in garage.Spots) 
        {
            foreach (var vehicle in spot.Vehicles)
            {
                list.Add(new ParkingRecord // create record
                {
                    Spot = spot.Number, 
                    Type = vehicle.VehicleType,      
                    Plate = vehicle.RegistrationNumber
                });
            }
        }
        // Serialize to JSON and write to file
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
        Console.WriteLine("Saved.");
    }

    public static Garage Load(int defaultSpots = 100, double spotCapacity = 1.0) // loads from file or creates new garage
    {
        var garage = new Garage(defaultSpots, spotCapacity);// create new garage

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
                Vehicle vehicle = record.Type.ToLower() == "car" ? new Car(record.Plate) : new Motorcycle(record.Plate); // recreate vehicle
                garage.TryParkOnSpot(vehicle, record.Spot); // place back on its original spot if it fits
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
