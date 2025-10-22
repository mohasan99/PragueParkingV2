namespace PragueParkingV2;

public class Motorcycle : Vehicle
{
    public Motorcycle(string registrationNumber) // Constructor for Motorcycle class
        : base(registrationNumber, 0.5)   // a motorcycle takes half a parking unit
    {
    }
    public override string VehicleType => "Motorcycle"; // Override VehicleType property to return "Motorcycle"
}
