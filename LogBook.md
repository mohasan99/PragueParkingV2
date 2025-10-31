## 16-10-2025

Work done:
- Finished implementing all classes (Vehicle, Car, Motorcycle, ParkingSpot, and Garage).
- Added logic for parking, removing, moving, and displaying vehicles.
- Tested with multiple scenarios (single/multiple motorcycles, cars, full/empty spots).
- Simplified some methods for readability and clarity.

Reasoning / Learning:
- Learned to apply object-oriented programming (inheritance, lists, and encapsulation).
- Understood how to separate logic between classes (Vehicle vs. ParkingSpot vs. Garage).
- Realized that clean helper methods make the program easier to maintain and debug.
------------------------------------------------------------------------------------
## 20-10-2025

Work done:

- Implemented GarageStorage class for saving and loading data to/from JSON file.
- Connected GarageStorage to the main program so that parked vehicles are saved when exiting and reloaded when restarting.
- Added a Vehicles property to ParkingSpot to allow reading parked vehicles safely.
- Simplified file handling using try/catch to avoid program crashes on missing or corrupted files.
- Tested saving and loading with multiple cars and motorcycles — data persisted correctly between sessions.
. Reviewed and cleaned up unnecessary complexity (removed extra exception throws, simplified constructors).

Reasoning / Learning:

- Learned how to serialize and deserialize objects in C# using System.Text.Json.
- Understood how to separate storage logic into a helper class (GarageStorage) instead of putting everything in Program.cs.
- Gained insight into the importance of validation and error handling when working with files.
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
  ---------------------------------------------------------------------------
  ## 29-10-2025
  
  Work done:

- Implemented PriceListService for handling a human-readable prices.txt file (external price list).
- Ensured the program creates a default prices.txt automatically if it doesn’t exist.
- Updated AppConfiguration to load values from both config.json and the new price list file.
- Verified that configuration and pricing load correctly at program startup.
- Began implementing unit testing (MSTest) following the assignment’s final requirement.
- Created a new PragueParkingV2.Tests project and added references to the main project.
- Added two working unit tests for parking and removing vehicles.

Reasoning / Learning:

- Learned how to read and parse plain text configuration files in C# with comments (#) and key/value pairs.
- Understood how to integrate external data (like price lists) safely into an application.
- Gained hands-on experience with MSTest and writing simple automated tests.
- Improved understanding of project structure by separating main code, configuration, and test logic into different projects.
  -----------------------------------------------------------------------
## 31-10-2025

Work done:

- Improved main menu using Spectre.Console for a modern, interactive console UI with keyboard navigation.
- Added a price list panel, showing the hourly rates for cars and motorcycles, and free parking time.
- Verified that autosave and checkout logic still work correctly after Spectre integration.
- Simplified UI flow and ensured all major functions (Park, Remove, Move, Checkout) display feedback using Spectre panels and colors.

Reasoning / Learning:

- Learned how to use Spectre.Console for interactive menus, panels, and markup to improve user experience.
- Understood how method scope affects visibility between functions in the same class.
- Practiced organizing code into helper methods for better readability and reuse.
- Gained experience integrating console UI libraries while keeping program logic separate from display logic.
- Strengthened understanding of C# class and method structure when adding new UI components.
