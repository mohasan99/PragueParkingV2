using System;

namespace PragueParkingV2;

class Program
{
    static void Main()
    {
        // Load configuration and garage (spots + any previously saved vehicles)
        var configuration = ConfigService.Load();
        var garage = GarageStorage.Load(configuration.SpotCount, configuration.SpotCapacity); 

        while (true)
        {
            Console.Clear();
            Console.WriteLine(" PRAGUE PARKING 2.0 ");
            Console.WriteLine("1. Park vehicle");
            Console.WriteLine("2. Remove vehicle");
            Console.WriteLine("3. Show all spots");
            Console.WriteLine("4. Checkout (with price)");
            Console.WriteLine("5. Move vehicle");
            Console.WriteLine("6. Save & Exit");
            Console.Write("Choose option: ");
            var choice = Console.ReadLine() ?? "";

            if (choice == "1") ParkVehicle(garage);                             
            else if (choice == "2") RemoveVehicle(garage);                      
            else if (choice == "3") { garage.ShowAllSpots(); Pause(); }
            else if (choice == "4") { CheckoutVehicle(garage, configuration); }           
            else if (choice == "5") { MoveVehicle(garage); }                    
            else if (choice == "6") { GarageStorage.Save(garage); return; }
            else { Console.WriteLine("Invalid choice."); Pause(); }
        }
    }

    //  Autosave after each change is implemented inside these helpers ===

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

        if (garage.TryPark(vehicle))
        {
            Console.WriteLine($"{vehicle.VehicleType} {vehicle.RegistrationNumber} parked!");
            GarageStorage.Save(garage); // autosave on change
        }
        else
        {
            Console.WriteLine("No space available.");
        }

        Pause();
    }

    static void RemoveVehicle(Garage garage)
    {
        Console.Write("Registration number to remove: ");
        var registrationNumber = Console.ReadLine() ?? "";

        if (garage.Remove(registrationNumber))
        {
            Console.WriteLine($"{registrationNumber} removed.");
            GarageStorage.Save(garage); // autosave on change
        }
        else
        {
            Console.WriteLine("Vehicle not found.");
        }

        Pause();
    }

    // Checkout with price (uses config values) ===

    static void CheckoutVehicle(Garage garage, AppConfiguration configuration)
    {
        Console.Write("Registration to checkout: ");
        var plate = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

        // Find the vehicle and which spot it's in
        Vehicle? found = null;
        int spotIndex = -1;

        for (int i = 0; i < garage.Spots.Length; i++)
        {
            foreach (var vehicle in garage.Spots[i].Vehicles)
            {
                if (vehicle.RegistrationNumber == plate)
                {
                    found = vehicle;
                    spotIndex = i;
                    break;
                }
            }
            if (found != null) break;
        }

        if (found == null)
        {
            Console.WriteLine("Vehicle not found.");
            Pause();
            return;
        }

        var now = DateTimeOffset.Now;
        var parked = now - found.CheckInTime;

        // Free minutes
        double fee = 0;
        if (parked.TotalMinutes > configuration.FreeMinutes)
        {
            // bill by started hour
            var hours = Math.Ceiling(parked.TotalHours);
            var rate = (found.VehicleType.ToLower() == "car")
                ? configuration.PricePerHourCar
                : configuration.PricePerHourMotorcycle;
            fee = hours * rate;
        }

        Console.WriteLine($"Parked since: {found.CheckInTime:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"Now:          {now:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"Duration:     {parked.TotalMinutes:F0} minutes");
        Console.WriteLine($"Fee:          {fee:0.##} CZK");

        // Remove and autosave
        if (garage.Remove(plate))
        {
            Console.WriteLine("Checked out.");
            GarageStorage.Save(garage); // autosave on change
        }
        else
        {
            Console.WriteLine("Unexpected: could not remove after pricing.");
        }

        Pause();
    }

    //  Move vehicle to a specific spot ===
    // Uses Garage.TryParkOnSpot to place on the target.
    static void MoveVehicle(Garage garage)
    {
        Console.Write("Plate to move: ");
        var plate = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

        // Find the vehicle and its current spot
        Vehicle? found = null;
        int fromIndex = -1;

        for (int i = 0; i < garage.Spots.Length; i++)
        {
            foreach (var vehicle in garage.Spots[i].Vehicles)
            {
                if (vehicle.RegistrationNumber == plate)
                {
                    found = vehicle;
                    fromIndex = i;
                    break;
                }
            }
            if (found != null) break;
        }

        if (found == null)
        {
            Console.WriteLine("Vehicle not found.");
            Pause();
            return;
        }

        Console.Write("Target spot number: ");
        var text = Console.ReadLine() ?? "1";
        if (!int.TryParse(text, out int targetSpot)) targetSpot = 1;

        // Try to park on target spot first (so we don't lose the vehicle if it doesn't fit)
        if (!garage.TryParkOnSpot(found, targetSpot))
        {
            Console.WriteLine("Could not move: target spot has not enough space or is invalid.");
            Pause();
            return;
        }

        // Remove from old spot and autosave
        garage.Spots[fromIndex].Remove(plate);
        Console.WriteLine($"Moved {plate} to spot {targetSpot}.");
        GarageStorage.Save(garage); // autosave on change
        Pause();
    }
    static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
}
