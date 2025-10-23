Prague Parking 2.0
------------------
Prague Parking 2.0 is a C# console application that simulates a parking garage system.
The program lets users park, remove, move, and check out vehicles (cars and motorcycles) while saving data automatically between sessions.

Features:

# Object-Oriented Design
Built using OOP principles such as inheritance, encapsulation, and composition.

# Persistent Storage (JSON)
All parked vehicles are saved automatically to a file (garage.json) and reloaded on startup.

# Configurable Settings
Garage size, spot capacity, prices, and free minutes are read from config.json.
If the file doesn’t exist, it is created automatically with default settings.

# Multiple Vehicle Types
Supports:
Car (size 1.0)
Motorcycle (size 0.5)

#Pricing System
Includes:
Hourly pricing for each vehicle type
Configurable free minutes before billing starts

# User Actions
Park new vehicle
Remove vehicle
Checkout (with price calculation)
Move vehicle between spots
View all parking spots
Auto-save after every change

# How It Works
When you start the program:
It loads the configuration from config.json.
If it doesn’t exist, one is automatically created with default values.
It loads the garage state (parked vehicles) from garage.json.
If that file doesn’t exist, a new empty garage is created.
The menu is shown in the console:

PRAGUE PARKING 2.0 
1. Park vehicle
2. Remove vehicle
3. Show all spots
4. Checkout (with price)
5. Move vehicle
6. Save & Exit

The user can interact with the system — every change is auto-saved.

# Project Structure
File	Description
Program.cs	Main console UI and logic
Vehicle.cs	Abstract base class for all vehicles
Car.cs	Car class (inherits from Vehicle)
Motorcycle.cs	Motorcycle class (inherits from Vehicle)
ParkingSpot.cs	Holds vehicles and capacity logic
Garage.cs	Collection of parking spots and operations
GarageStorage.cs	Saves and loads garage data to/from JSON
AppConfiguration.cs	Holds configurable settings
ConfigService.cs	Reads or creates config.json
PriceService.cs	Calculates parking cost (optional helper)
🧮 Configuration Example

The file config.json looks like this:

{
  "SpotCount": 100,
  "SpotCapacity": 1.0,
  "PricePerHourCar": 20.0,
  "PricePerHourMotorcycle": 10.0,
  "FreeMinutes": 10
}
You can edit these values to change the garage size or pricing without touching the code.

# Save File Example
garage.json stores all currently parked vehicles:

[
  { "Spot": 1, "Type": "Car", "Plate": "ABC123" },
  { "Spot": 2, "Type": "Motorcycle", "Plate": "MC88" }
]

# How to Run

Open the project in Visual Studio 2022.
Press F5 or select Run.
Use the menu to park and manage vehicles.
Exit using option 6 to save and close.
Restart the app — your vehicles and settings are restored automatically.

# Learning & Reflection

Through this project I learned:
How to use classes, inheritance, and composition in C#.
How to serialize and deserialize objects using System.Text.Json.
How to build a menu-driven console program with persistent data.
How to separate logic into clear, maintainable files.
How to apply OOP design patterns to solve real-world problems.

Author

Student: Mohammed Hasan
Course: C# programming — Prague Parking 2.0
Date: 23-10-2025
