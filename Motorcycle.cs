namespace PragueParkingV2;

public class Motorcycle : Vehicle
{
    public Motorcycle(string registrationNumber) // Constructor for Motorcycle class
        : base(registrationNumber, 0.5)   // a motorcycle takes half a parking unit
    {
    }
    internal Motorcycle(string registrationNumber, DateTimeOffset checkInTime)
        : base(registrationNumber, 0.5)
    {
        CheckInTime = checkInTime; // restore saved check-in
    }
    public override string VehicleType => "Motorcycle"; // Override VehicleType property to return "Motorcycle"
}
