## 16-10-2025

Work done:

Finished implementing all classes (Vehicle, Car, Motorcycle, ParkingSpot, and Garage).

Added logic for parking, removing, moving, and displaying vehicles.

Tested with multiple scenarios (single/multiple motorcycles, cars, full/empty spots).

Simplified some methods for readability and clarity.


Reasoning / Learning:

Learned to apply object-oriented programming (inheritance, lists, and encapsulation).

Understood how to separate logic between classes (Vehicle vs. ParkingSpot vs. Garage).

Realized that clean helper methods make the program easier to maintain and debug.
------------------------------------------------------------------------------------
## 20-10-2025

Work done:

Implemented GarageStorage class for saving and loading data to/from JSON file.

Connected GarageStorage to the main program so that parked vehicles are saved when exiting and reloaded when restarting.

Added a Vehicles property to ParkingSpot to allow reading parked vehicles safely.

Simplified file handling using try/catch to avoid program crashes on missing or corrupted files.

Tested saving and loading with multiple cars and motorcycles — data persisted correctly between sessions.

Reviewed and cleaned up unnecessary complexity (removed extra exception throws, simplified constructors).

Reasoning / Learning:

Learned how to serialize and deserialize objects in C# using System.Text.Json.

Understood how to separate storage logic into a helper class (GarageStorage) instead of putting everything in Program.cs.

Gained insight into the importance of validation and error handling when working with files.
--------------------------------------------------------------------------------
## 23-10-2025

Work done:
- Integrated ConfigService and AppConfiguration (config.json now auto-creates and loads).
- Implemented auto-save feature after parking, removing, moving, and checking out vehicles.
- Added CheckoutVehicle() method with pricing logic using free minutes and hourly rates.
- Added MoveVehicle() method to move vehicles between spots.
- Verified GarageStorage saves/loads correctly and data persists after restart.

Reasoning / Learning:
- Learned how to serialize and deserialize data to JSON for persistent storage.
- Understood how to separate configuration and runtime data using different classes.
- Improved understanding of method reusability (Garage.TryParkOnSpot used by MoveVehicle).
- Gained experience applying encapsulation and clean object interactions.
------------------------------------------------------------------------------
## 24-10-2025  

Work done:
- Replaced all Console.WriteLine and Console.ReadLine interactions with **Spectre.Console** components.  
- Upgraded Program.cs to use interactive menus, prompts, tables, and panels for a more modern and readable UI.  
- Simplified user interaction by using SelectionPrompt, TextPrompt, and helper methods for input.  
- Tested all features (parking, removing, moving, checkout, and save/load) through the new Spectre.Console interface.
  
Reasoning / Learning:
- Learned how to use the Spectre.Console library to create rich and interactive console applications.  
- Understood how to separate business logic (Garage, Vehicle, etc.) from the user interface layer.  
- Realized that a clear and structured UI improves usability and user experience without changing the core logic.  
- Gained experience in integrating external libraries while keeping the program simple and maintainable.  
