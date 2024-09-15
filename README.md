# Rover Control Mission Assignment
## Overview
This project is a .NET 8 solution that simulates controlling rovers on a plateau using a mission file with instructions. The solution is composed of three main components:

- A **class library** that handles core logic, including plateau and rover management
- A **console application** that provides an ASCII-based representation of the rover mission and processes the mission file
- A **unit test project** using NUnit to verify the functionality of the core library

## Prerequisites
- **.NET 8 SDK** installed on your machine
- **Visual Studio 22** or later

## Assumptions
1. **No Collision Detection:** Multiple rovers can occupy the same coordinates at the same time. No collision detection occurs, and only one rover is visually represented at each position.
2. **Reading Instructions from a Text File:** The console application reads all mission instructions from a text file and loads them into memory using a Queue. This approach assumes that the file is relatively small. For larger files, a streaming approach would be used to avoid loading all lines into memory at once.
3. **Rover Instructions Parsing:** The application assumes that each rover always has two lines of instructions: one for its initial position and one for its movement. If a position line is invalid, the rover is skipped and the next rover is parsed, but both the position and movement lines are consumed regardless. This could lead to issues if a rover has a position line but no corresponding movement line.
4. **Invalid Movement Characters:** If a movement instruction contains invalid characters (e.g., 'X' or 'P'), the rover ignores them and processes the remaining valid instructions.
5. **Rovers Stay at Plateau Boundaries:** If a rover attempts to move off the plateau, it will remain at the boundary rather than moving or falling off.
6. **Auto-Correction for Out-of-Bounds Initial Position:** If a rover's initial position is off the plateau, it is automatically placed at coordinates (0, 0).
7. **Early Exit on Critical Errors:** The application exits early if critical errors occur that it cannot recover from, such as an invalid plateau dimension line or empty mission file.

## Solution Structure
The solution consists of three projects:
1. **RoverControl.Core** (Class Library)
   - Contains the core business logc for managing rovers and the plateau
   - Manages the state of the rovers, their positions, and the execution of mission instructions
2. **RoverControl.ConsoleApp** (ConsoleApplication)
   - Provides an ASCII art representation of the mission, showing the plateau and rover movements
   - Reads a mission file that contains the plateau's dimensions, initial rover positions, and movement commands
   - Displays the final position of each rover after executing instructions
   - Contains a folder title *Missions* that has example .txt files to insert into console application (user can drag & drop file path into console application when prompted for file path)
3. **RoverControl.Tests** (NUnit Test Project)
   - Contains unit tests for the core library (RoverControl.Core)

## How to Run
1. Clone the repository
```
git clone https://github.com/haliechm/RoverControl.git
```
2. Open the solution in Visual Studio 2022
3. To run the console application:
   - Set RoverControl.ConsoleApp as the startup project
   - Build and run the project (Ctrl+F5 in Visual Studio)
   - You will be prompted to enter the path to a mission file (look into the *Missions* folder for example .txt files that can be used)
4. To run the tests:
   - Open the Test Explorer window in Visual Studio (Test > Test Explorer)
   - Run all tests to ensure the functionality is correct
 
## Mission File Format
The mission file should have the following format:

```
5 5               # Plateau upper-right coordinates (in this case, both the width and the height of the plateau is 6, as the bottom-left coordinates are (0,0))
1 2 N             # Rover 1's initial position (x, y, orientation)
LMLMLMLMM         # Rover 1's movement instructions (L, R, M)
3 3 E             # Rover 2's initial position
MMRMMRMRRM        # Rover 2's movement instructions
... more rover instructions
```

## Example Output
When you run the console application, you will see an ASCII representation of the plateau, rovers, and their movements.

Example final output:

```
1 3 N
5 1 E
```



  
