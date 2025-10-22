using System;

namespace PragueParkingV2
{
    public class AppConfiguration
    {
        // Garage settings
        public int SpotCount { get; set; } = 100;
        public double SpotCapacity { get; set; } = 1.0;

        // Pricing 
        public double PricePerHourCar { get; set; } = 20.0;        
        public double PricePerHourMotorcycle { get; set; } = 10.0;
        public int FreeMinutes { get; set; } = 10;              

}
}
