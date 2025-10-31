using Spectre.Console;
using Spectre.Console.Rendering;
using System.Linq;

namespace PragueParkingV2;

public class Garage
{
    private ParkingSpot[] spots;

    public ParkingSpot[] Spots => spots;  // for saving

    public bool TryParkOnSpot(Vehicle vehicle, int spotNumber)  // for loading
    {
        if (spotNumber < 1 || spotNumber > spots.Length) return false;
        return spots[spotNumber - 1].TryPark(vehicle);
    }


    public Garage(int spotCount = 100, double spotCapacity = 1.0) // Default: 100 spots, each with capacity of 1.0
    {
        spots = new ParkingSpot[spotCount];

        for (int i = 0; i < spotCount; i++) // Initialize each parking spot
        {
            spots[i] = new ParkingSpot(i + 1, spotCapacity);
        }
    }

    public bool TryPark(Vehicle vehicle) // Try to park a vehicle in any available spot
    {
        foreach (var spot in spots)
        {
            if (spot.TryPark(vehicle))
                return true;
        }
        return false;
    }

    public bool Remove(string registrationNumber) // Remove a vehicle by its registration number
    {
        foreach (var spot in spots)
        {
            if (spot.Remove(registrationNumber))
                return true;
        }
        return false;
    }

    public void ShowGarageMap(int columns = 10) // Display the garage map in a table format
    {
        AnsiConsole.Clear();

        var table = new Table()
            .Title("[white]Karta över P-huset[/]")
            .Border(TableBorder.Rounded)
            .Expand();

        for (int c = 1; c <= columns; c++)
            table.AddColumn(new TableColumn($"[grey]{c}[/]"));

        int rows = (int)Math.Ceiling(spots.Length / (double)columns);

        for (int r = 0; r < rows; r++)
        {
            var row = new List<IRenderable>(columns);

            for (int c = 0; c < columns; c++)
            {
                int i = r * columns + c;
                if (i >= spots.Length) { row.Add(new Markup(" ")); continue; }

                var first = spots[i].Vehicles.FirstOrDefault();
                string number = $"{i + 1:000}";

                if (first is null)
                {

                    string label = Markup.Escape("[ ]");
                    row.Add(new Markup($"[green]{number}[/]\n[green]{label}[/]"));
                }
                else
                {

                    var plate = first.RegistrationNumber?.Trim() ?? "?";

                    if (plate.Length > 10) plate = plate[..10];

                    string label = $"[{plate}{(spots[i].Vehicles.Count() > 1 ? $" +{spots[i].Vehicles.Count() - 1}" : "")}]";
                    label = Markup.Escape(label);

                    row.Add(new Markup($"[red]{number}[/]\n[red]{label}[/]"));
                }
            }

            table.AddRow(row);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine(
     "\n[grey]Legend:[/] [green]NNN[/]=empty, [red]NNN[/]=occupied, second line shows [[PLATE]] (and +n if multiple).");

    }
}
