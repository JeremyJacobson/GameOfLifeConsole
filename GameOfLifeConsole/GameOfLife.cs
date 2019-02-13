using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GameOfLifeConsole
{
    public class GameOfLife
    {
        private static Thread gridLoop = new Thread(new ThreadStart(Tick));
        private static Thread menu = new Thread(new ThreadStart(MenuControl));
        private static int tickTime = 500;//time between loops in miliseconds
        private static List<Cell> allCells = new List<Cell>();
        private static int gridWidth = 50; //x
        private static int gridHeight = 40; //y
        private static Grid gameGrid;
        private static bool menuMoving;
        private static bool gridPrinting;
        private static bool stopped = true;

        public void Run()
        {
            Setup();

            gridLoop.Start();
            menu.Start();
        }

        private void Setup()
        {
            //Console.SetWindowSize(50, 50);
            Console.CursorVisible = false;
            Console.Title = "Conway's Game Of Life";
            gameGrid = new Grid(gridWidth, gridHeight);

            allCells = gameGrid.CellGrid;

            gameGrid.PrintGrid();
        }

        private static void Tick()
        {
            while (stopped)
            {
                Thread.Sleep(500);
            }

            Thread.Sleep(tickTime);

            if (!menuMoving)
            {
                UpdateNeighbors();
                allCells = NewGeneration();

                gameGrid.UpdateGrid(allCells);

                gridPrinting = true;
                gameGrid.PrintGrid();
                gridPrinting = false;
            }

            Tick();
        }

        private static void PrintMenu()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            //Print Menu Area
            Console.SetCursorPosition(0, gridHeight + 1);
            for (int i = 0; i < 8; i++)
            {
                Console.CursorLeft = 0;
                for (int j = 0; j < gridWidth; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            //Print Menu Items
            PrintMenuItems(1);
        }

        private static void PrintMenuItems(int highlightedItem)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(4, gridHeight + 2);

            if (highlightedItem == 0)
                Console.ForegroundColor = ConsoleColor.Black;

            if (highlightedItem == 1)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Start");
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 2)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.CursorLeft = 13;
            Console.Write("Stop");
            Console.BackgroundColor = ConsoleColor.Black;

            Console.CursorLeft = 21;
            Console.Write("Speed");

            if (highlightedItem == 3)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("{0:X}", (char)8595);
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 4)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("{0:X}", (char)8593);
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 5)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.CursorLeft = 32;
            Console.Write("Load");

            ResetCursor();
        }

        private static void MenuControl()
        {
            PrintMenu();
            int highlightedItem = 1;
            menuMoving = false;
            ReadMenuInput(highlightedItem);
        }

        private static void ReadMenuInput(int highlightedItem)
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            ConsoleKeyInfo pressedKey = Console.ReadKey(true);

            while (gridPrinting)
            {
                Thread.Sleep(100);
            }
            menuMoving = true;

            if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                if (highlightedItem > 1)
                    highlightedItem--;
                else
                    highlightedItem = 5;
                PrintMenuItems(0);
                PrintMenuItems(highlightedItem);
            }
            if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                if (highlightedItem < 5)
                    highlightedItem++;
                else
                    highlightedItem = 1;
                PrintMenuItems(0);
                PrintMenuItems(highlightedItem);
            }
            if (pressedKey.Key == ConsoleKey.Spacebar)
            {
                if (highlightedItem == 1)//Start
                {
                    stopped = false;
                }
                if (highlightedItem == 2)//Stop
                {
                    stopped = true;
                }
                if (highlightedItem == 3)//Decrease speed
                {
                    if (tickTime < 2000)
                        tickTime += 100;
                }
                if (highlightedItem == 4)//Increase speed
                {
                    if (tickTime > 100)
                        tickTime -= 100;
                }
                if (highlightedItem == 5)//Load
                {
                    PrintMenuItems(0);
                    FileSelect();
                }
            }

            menuMoving = false;
            ReadMenuInput(highlightedItem);
        }

        private static void FileSelect()
        {
            PrintFileSelect();
            int highlightedItem = 1;
            menuMoving = false;
            ReadFileSelectInput(highlightedItem);
        }

        private static void ReadFileSelectInput(int highlightedItem)
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            ConsoleKeyInfo pressedKey = Console.ReadKey(true);

            while (gridPrinting)
            {
                Thread.Sleep(100);
            }
            menuMoving = true;

            if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                if (highlightedItem > 1)
                    highlightedItem--;
                else
                    highlightedItem = 6;
                PrintFileSelectItems(0);
                PrintFileSelectItems(highlightedItem);
            }
            if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                if (highlightedItem < 6)
                    highlightedItem++;
                else
                    highlightedItem = 1;
                PrintFileSelectItems(0);
                PrintFileSelectItems(highlightedItem);
            }
            if (pressedKey.Key == ConsoleKey.Spacebar)
            {
                LoadFile(highlightedItem);
            }
            if (pressedKey.Key == ConsoleKey.Escape)
            {
                MenuControl();
            }

            menuMoving = false;
            ReadFileSelectInput(highlightedItem);
        }

        private static void LoadFile(int selectedFile)
        {
            gameGrid.ClearLivingCells();
            List<Cell> loadedCells = new List<Cell>();
            int x;
            int y;
            string[] values;
            string fileToLoad = "";
            Console.ForegroundColor = ConsoleColor.White;
            if (selectedFile == 1)
                fileToLoad = "LoadFiles/Starter.txt";
            if (selectedFile == 2)
                fileToLoad = "LoadFiles/RPentomino.txt";
            if (selectedFile == 3)
                fileToLoad = "LoadFiles/Tumbler.txt";
            if (selectedFile == 4)
                fileToLoad = "LoadFiles/GliderGun.txt";
            if (selectedFile == 5)
                fileToLoad = "LoadFiles/SparkCoil.txt";
            if (selectedFile == 6)
                fileToLoad = "LoadFiles/QueenBee.txt";

            try
            {
                using (StreamReader sr = new StreamReader(fileToLoad))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        values = line.Split(' ');
                        x = int.Parse(values[0]);
                        y = int.Parse(values[1]);
                        loadedCells.Add(new Cell(x, y));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            foreach (Cell cell in loadedCells)
            {
                gameGrid.AddLivingCell(cell);
            }
            gameGrid.PrintGrid();
            allCells = gameGrid.CellGrid;
            loadedCells.Clear();
            MenuControl();
        }

        private static void PrintFileSelect()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            //Print File Select Area
            Console.SetCursorPosition(0, gridHeight + 1);
            for (int i = 0; i < 8; i++)
            {
                Console.CursorLeft = 0;
                for (int j = 0; j < gridWidth; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            //Print File Select Items
            PrintFileSelectItems(1);
        }

        private static void PrintFileSelectItems(int highlightedItem)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(1, gridHeight + 2);

            if (highlightedItem == 0)
                Console.ForegroundColor = ConsoleColor.Black;

            Console.Write("Load:");

            if (highlightedItem == 1)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(3, gridHeight + 4);
            Console.Write("Starter");
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 2)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(15, gridHeight + 4);
            Console.Write("R-Pentomino");
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 3)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(27, gridHeight + 4);
            Console.Write("Tumbler");
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 4)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(3, gridHeight + 6);
            Console.Write("Glider Gun");
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 5)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(15, gridHeight + 6);
            Console.Write("Spark Coil");
            Console.BackgroundColor = ConsoleColor.Black;

            if (highlightedItem == 6)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(27, gridHeight + 6);
            Console.Write("Queen Bee");
            Console.BackgroundColor = ConsoleColor.Black;

            ResetCursor();
        }

        private static void ResetCursor()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, gridHeight + 1);
        }

        private static List<Cell> NewGeneration()
        {
            List<Cell> futureGen = new List<Cell>();
            foreach (Cell cell in allCells)
            {
                futureGen.Add(DeadOrAlive(cell));
            }
            return futureGen;
        }

        private static Cell DeadOrAlive(Cell cell)
        {
            Cell updatedCell = cell;
            int totalNeighbors = updatedCell.CellNeighbors.NeighborCells.Count;

            if (updatedCell.Alive)
            {
                if (totalNeighbors < 2) //if less than two neighbors the cell dies from underpopulation
                    updatedCell.Alive = false;
                else if (totalNeighbors > 3) //if more than 3 neighbors the cell dies from overpopulation
                    updatedCell.Alive = false;
            }
            else
            {
                if (totalNeighbors == 3)
                    updatedCell.Alive = true; //if exactly 3 neighbors the cell is born from reproduction
            }

            return updatedCell;
        }

        private static void UpdateNeighbors()
        {
            for (int i = 0; i < allCells.Count; i++)
            {
                allCells[i].ClearNeighbors();

                if (i >= gridWidth) //checks if cell is not at the top edge of the grid
                    if (allCells[i - gridWidth].Alive) allCells[i].AddNeighbor(allCells[i - gridWidth]); //adds cell at the top if alive

                if (i < (gridWidth * (gridHeight - 1))) //checks if cell is not at the bottom edge of the grid
                    if (allCells[i + gridWidth].Alive) allCells[i].AddNeighbor(allCells[i + gridWidth]);//adds cell at the bottom if alive

                if ((i + 1) % gridWidth != 0) //checks if cell is not at the right edge of the grid
                {
                    if (i >= gridWidth) //checks if cell is not at the top edge of the grid
                        if (allCells[i - (gridWidth - 1)].Alive) allCells[i].AddNeighbor(allCells[i - (gridWidth - 1)]);//adds cell at the top right if alive

                    if (i < (gridWidth * (gridHeight - 1))) //checks if cell is not at the bottom edge of the grid
                        if (allCells[i + (gridWidth + 1)].Alive) allCells[i].AddNeighbor(allCells[i + (gridWidth + 1)]);//adds cell at the bottom right if alive

                    if (allCells[i + 1].Alive) allCells[i].AddNeighbor(allCells[i + 1]);//adds cell at the right if alive
                }

                if (i % gridWidth != 0) //checks if cell is not at the left edge of the grid
                {
                    if (i >= gridWidth) //checks if cell is not at the top edge of the grid
                        if (allCells[i - (gridWidth + 1)].Alive) allCells[i].AddNeighbor(allCells[i - (gridWidth + 1)]);//adds cell at the top left if alive

                    if (i < (gridWidth * (gridHeight - 1))) //checks if cell is not at the bottom edge of the grid
                        if (allCells[i + (gridWidth - 1)].Alive) allCells[i].AddNeighbor(allCells[i + (gridWidth - 1)]);//adds cell at the bottom right if alive

                    if (allCells[i - 1].Alive) allCells[i].AddNeighbor(allCells[i - 1]);//adds cell at the left if alive
                }
            }
        }
    }
}
