namespace PragueParkingV2;

public class Car : Vehicle
{
    public Car(string registrationNumber) // Constructor for Car class
        : base(registrationNumber, 1.0)   // a car takes 1 full parking unit
    {
    }
    internal Car(string registrationNumber, DateTimeOffset checkInTime)
        : base(registrationNumber, 1.0)
    {
        CheckInTime = checkInTime; // restore saved check-in
    }
    public override string VehicleType => "Car"; // Override VehicleType property to return "Car"
}
