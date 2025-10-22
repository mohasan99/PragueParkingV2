using System;

namespace PragueParkingV2;

class Program
{
    static void Main()
    {

        var cfg = ConfigService.Load();                     // NEW: read config
        var garage = GarageStorage.Load(cfg.SpotCount, cfg.SpotCapacity); // use it

        while (true)
        {
            Console.Clear();
            Console.WriteLine("PRAGUE PARKING 2.0");
            Console.WriteLine("1. Park vehicle");
            Console.WriteLine("2. Remove vehicle");
            Console.WriteLine("3. Show all spots");
            Console.WriteLine("4. Save & Exit");   
            Console.Write("Choose option: ");
            var choice = Console.ReadLine() ?? "";

            if (choice == "1") ParkVehicle(garage);
            else if (choice == "2") RemoveVehicle(garage);
            else if (choice == "3") { garage.ShowAllSpots(); Pause(); }
            else if (choice == "4") { GarageStorage.Save(garage); return; }
            else { Console.WriteLine("Invalid choice."); Pause(); }
        }
    }

    static void ParkVehicle(Garage garage)
    {
        Console.Write("Type (car/mc): ");
        var type = (Console.ReadLine() ?? "").Trim().ToLower();

        if (type != "car" && type != "mc")
        {
            Console.WriteLine("Please type 'car' or 'mc'.");
            Pause();
            return;
        }

        Console.Write("Registration number: ");
        var registrationNumber = Console.ReadLine() ?? "";

        Vehicle vehicle = (type == "car") ? new Car(registrationNumber) : new Motorcycle(registrationNumber);

        Console.WriteLine(garage.TryPark(vehicle)
            ? $"{vehicle.VehicleType} {vehicle.RegistrationNumber} parked!"
            : "No space available.");

        Pause();
    }

    static void RemoveVehicle(Garage garage)
    {
        Console.Write("Registration number to remove: ");
        var registrationNumber = Console.ReadLine() ?? "";

        Console.WriteLine(garage.Remove(registrationNumber)
            ? $"{registrationNumber} removed."
            : "Vehicle not found.");

        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
}
