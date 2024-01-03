
# Game of Life in C#

## Overview
This C# application simulates Conway's Game of Life, a cellular automaton devised by mathematician John Conway. In this game, cells evolve based on a set of rules, creating fascinating patterns and behaviors.

## Features
- Click to activate/deactivate cells
- Run the simulation from a random seed or a specific time
- Customize cell and grid colors
- Choose between finite or toroidal (wrap-around) grid

## Getting Started
1. **Clone the Repository:**
   ```bash
   git clone https://github.com/KayleBrown/game-of-life-csharp.git
   ```

2. **Open the Solution:**
   Open the solution file (`GameOfLife.sln`) in your preferred C# IDE (e.g., Visual Studio).

3. **Build and Run:**
   Build and run the application.

## Usage
- **Activate/Deactivate Cells:**
  Click on individual cells to toggle their state (alive or dead).

- **Start Simulation:**
  Click the "Run" or "Start" button to begin the simulation.

- **Random Seed:**
  Click the "Random Seed" button to initialize the grid with a random configuration.

- **Customization:**
  - Change cell color: Use the color picker to choose a different color for live cells.
  - Change grid color: Similarly, use the color picker to select a color for the grid.

- **Grid Type:**
  - Finite: Cells at the grid edges have fewer neighbors.
  - Toroidal: Grid wraps around, making cells at opposite edges neighbors.

## Configuration
- **Modify Colors:**
  Open the settings or configuration file to change the default colors for cells and the grid.

- **Adjust Grid Size:**
  If needed, find and modify variables in the code related to the grid size.

## Examples
![Game of Life Gif](https://s13.gifyu.com/images/SjaaH.gif)

## Author
[Kayle Brown]
[kaylebrown3825@gmail.com]

Feel free to contribute and open issues if you encounter any problems or have suggestions for improvements!
