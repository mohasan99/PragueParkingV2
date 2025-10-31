using System;
using Spectre.Console;

namespace PragueParkingV2;

class Program
{
    static void Main()
    {
        var configuration = ConfigService.LoadAll();
        var garage = GarageStorage.Load(configuration.SpotCount, configuration.SpotCapacity);

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]PRAGUE PARKING 2.0[/]").Centered().RuleStyle("grey"));

            // Show the price list panel
            ShowPriceList(configuration);

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Välj ett alternativ:[/]")
                    .PageSize(10)
                    .AddChoices(
                        "1) Park vehicle",
                        "2) Remove vehicle",
                        "3) Show all spots",
                        "4) Checkout (with price)",
                        "5) Move vehicle",
                        "6) Save & Exit"
                    ));

            // Route by choice
            if (choice.StartsWith("1")) ParkVehicle(garage);
            else if (choice.StartsWith("2")) RemoveVehicle(garage);
            else if (choice.StartsWith("3")) { garage.ShowGarageMap(10); Pause(); }
            else if (choice.StartsWith("4")) { CheckoutVehicle(garage, configuration); }
            else if (choice.StartsWith("5")) { MoveVehicle(garage); }
            else if (choice.StartsWith("6"))
            {
                GarageStorage.Save(garage);
                return;
            }
        }
    }

    static string AskForPlate(string question) =>
        AnsiConsole.Prompt(new TextPrompt<string>(question)
            .PromptStyle("cyan")
            .Validate(input =>
                string.IsNullOrWhiteSpace(input)
                    ? ValidationResult.Error("[red]Enter a valid registration number[/]")
                    : ValidationResult.Success()));

    static string AskForType() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose vehicle type:")
                .AddChoices("car", "mc"));

    static int AskForSpotNumber() =>
        AnsiConsole.Prompt(
            new TextPrompt<int>("Enter target spot number:")
                .PromptStyle("cyan")
                .Validate(number =>
                    number <= 0
                        ? ValidationResult.Error("[red]Spot number must be greater than 0[/]")
                        : ValidationResult.Success()));

    static void Pause()
    {
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Press ENTER to continue[/]").AllowEmpty());
    }

    static void ParkVehicle(Garage garage)
    {
        string vehicleType = AskForType();
        string registrationNumber = AskForPlate("Enter registration number:");

        Vehicle vehicle = (vehicleType == "car")
            ? new Car(registrationNumber)
            : new Motorcycle(registrationNumber);

        if (garage.TryPark(vehicle))
        {
            AnsiConsole.MarkupLine($"[green]{vehicle.VehicleType} {vehicle.RegistrationNumber} parked![/]");
            GarageStorage.Save(garage); // autosave after change
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No space available.[/]");
        }
        Pause();
    }

    static void RemoveVehicle(Garage garage)
    {
        string registrationNumber = AskForPlate("Enter registration number to remove:");

        if (garage.Remove(registrationNumber))
        {
            AnsiConsole.MarkupLine($"[yellow]{registrationNumber} removed.[/]");
            GarageStorage.Save(garage); // autosave after change
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Vehicle not found.[/]");
        }
        Pause();
    }

    static void CheckoutVehicle(Garage garage, AppConfiguration configuration)
    {
        string registrationNumber = AskForPlate("Enter registration number to checkout:").Trim().ToUpperInvariant();

        Vehicle? foundVehicle = null;
        int spotIndex = -1;

        for (int i = 0; i < garage.Spots.Length; i++)
        {
            foreach (var vehicle in garage.Spots[i].Vehicles)
            {
                if (vehicle.RegistrationNumber == registrationNumber)
                {
                    foundVehicle = vehicle;
                    spotIndex = i;
                    break;
                }
            }
            if (foundVehicle != null) break;
        }

        if (foundVehicle == null)
        {
            AnsiConsole.MarkupLine("[red]Vehicle not found.[/]");
            Pause();
            return;
        }

        var now = DateTimeOffset.Now;
        var parkedDuration = now - foundVehicle.CheckInTime;

        double fee = 0;
        if (parkedDuration.TotalMinutes > configuration.FreeMinutes)
        {
            var hours = Math.Ceiling(parkedDuration.TotalHours);
            var rate = foundVehicle.VehicleType.Equals("car", StringComparison.OrdinalIgnoreCase)
                ? configuration.PricePerHourCar
                : configuration.PricePerHourMotorcycle;

            fee = hours * rate;
        }

        var panel = new Panel(
            $"[white]Parked since:[/] {foundVehicle.CheckInTime:yyyy-MM-dd HH:mm}\n" +
            $"[white]Now:[/] {now:yyyy-MM-dd HH:mm}\n" +
            $"[white]Duration:[/] {parkedDuration.TotalMinutes:F0} minutes\n" +
            $"[white]Fee:[/] [bold]{fee:0.##} CZK[/]")
        {
            Header = new PanelHeader($"Checkout — {foundVehicle.VehicleType} {foundVehicle.RegistrationNumber}", Justify.Center)
        };

        AnsiConsole.Write(panel);

        if (garage.Remove(registrationNumber))
        {
            AnsiConsole.MarkupLine("[green]Checked out and removed successfully.[/]");
            GarageStorage.Save(garage);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Unexpected error: could not remove vehicle after checkout.[/]");
        }

        Pause();
    }

    static void MoveVehicle(Garage garage)
    {
        string registrationNumber = AskForPlate("Enter registration number to move:").Trim().ToUpperInvariant();

        Vehicle? foundVehicle = null;
        int originalSpotIndex = -1;

        for (int i = 0; i < garage.Spots.Length; i++)
        {
            foreach (var vehicle in garage.Spots[i].Vehicles)
            {
                if (vehicle.RegistrationNumber == registrationNumber)
                {
                    foundVehicle = vehicle;
                    originalSpotIndex = i;
                    break;
                }
            }
            if (foundVehicle != null) break;
        }

        if (foundVehicle == null)
        {
            AnsiConsole.MarkupLine("[red]Vehicle not found.[/]");
            Pause();
            return;
        }

        int targetSpot = AskForSpotNumber();

        if (!garage.TryParkOnSpot(foundVehicle, targetSpot))
        {
            AnsiConsole.MarkupLine("[red]Cannot move vehicle — target spot is invalid or full.[/]");
            Pause();
            return;
        }

        garage.Spots[originalSpotIndex].Remove(registrationNumber);
        AnsiConsole.MarkupLine($"[green]Moved {registrationNumber} to spot {targetSpot}.[/]");
        GarageStorage.Save(garage);
        Pause();
    }

 
    static void ShowAllSpots(Garage garage)
    {
        var table = new Table().Centered();
        table.AddColumn("[bold]Spot[/]");
        table.AddColumn("[bold]Vehicles[/]");

        foreach (var spot in garage.Spots)
        {
            string info = "";
            foreach (var vehicle in spot.Vehicles)
                info += $"{vehicle.VehicleType} {vehicle.RegistrationNumber}  ";

            table.AddRow($"[yellow]{spot.Number}[/]", string.IsNullOrWhiteSpace(info) ? "[grey](empty)[/]" : info.Trim());
        }

        AnsiConsole.Write(table);
    }

    static void ShowPriceList(AppConfiguration configuration)
    {
        var priceText =
            $"[white]Bil:[/] {configuration.PricePerHourCar:0} CZK/timme\n" +
            $"[white]MC:[/]  {configuration.PricePerHourMotorcycle:0} CZK/timme\n\n" +
            $"[green]Fri parkering: {configuration.FreeMinutes} minuter[/]";

        var panel = new Panel(priceText)
            .Header("[orchid1]Parkeringsavgifter[/]")
            .Border(BoxBorder.Rounded)
            .Expand();

        AnsiConsole.Write(panel);
    }
}
