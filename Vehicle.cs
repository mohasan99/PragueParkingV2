using System;

namespace PragueParkingV2;

public abstract class Vehicle
{
    protected Vehicle(string registrationNumber, double sizeUnits) 
    {
        RegistrationNumber = registrationNumber.Trim().ToUpperInvariant(); // Normalize registration number

        if (RegistrationNumber.Length < 3 || RegistrationNumber.Length > 10) // Validation of registration number length
            throw new ArgumentException("Registration number must be 3-10 signs.");

        SizeUnits = sizeUnits; // Assign size units
        CheckInTime = DateTimeOffset.Now; // Set check-in time to current time
    }

    public string RegistrationNumber { get; } // Normalized registration number
    public double SizeUnits { get; } // Size of the vehicle in size units
    public DateTimeOffset CheckInTime { get; protected set; } // Time of vehicle check-in
    public abstract string VehicleType { get; } // Abstract property for vehicle type

    public override string ToString() => $"{VehicleType} {RegistrationNumber}"; // String representation of the vehicle
}
