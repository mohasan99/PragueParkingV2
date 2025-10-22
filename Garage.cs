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

    public void ShowAllSpots() // Display the status of all parking spots
    {
        foreach (var spot in spots)
        {
            Console.WriteLine(spot.ToString());
        }
    }
}
