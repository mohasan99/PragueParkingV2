using System;
using System.Collections.Generic;

namespace PragueParkingV2;

public class ParkingSpot 
{
    public int Number { get; } 
    public double Capacity { get; } 

    private readonly List<Vehicle> vehicles = new();

    public System.Collections.Generic.IEnumerable<Vehicle> Vehicles => vehicles;

    public ParkingSpot(int number, double capacity = 1.0) // Constructor with default capacity of 1.0
    {
        Number = number;
        Capacity = capacity;
    }

    public bool TryPark(Vehicle vehicle) // Try to park a vehicle in this spot
    {
        double usedSpace = 0;
        foreach (var v in vehicles)
            usedSpace += v.SizeUnits;

        if (usedSpace + vehicle.SizeUnits > Capacity)
            return false; // Not enough space

        vehicles.Add(vehicle);
        return true;
    }

    public bool Remove(string registrationNumber) // Remove a vehicle by its registration number
    {
        string plate = registrationNumber.Trim().ToUpperInvariant();

        foreach (var v in vehicles)
        {
            if (v.RegistrationNumber == plate)
            {
                vehicles.Remove(v);
                return true;
            }
        }

        return false; // Not found
    }

    public override string ToString() // String representation of the parking spot
    {
        if (vehicles.Count == 0)
            return $"Spot {Number}: (empty)"; // If no vehicles, indicate empty spot

        string info = "";
        foreach (var v in vehicles) // List all vehicles in the spot
            info += $"{v.VehicleType} {v.RegistrationNumber} "; // Append vehicle info

        return $"Spot {Number}: {info}"; // Return spot number and vehicle info
    }
}
