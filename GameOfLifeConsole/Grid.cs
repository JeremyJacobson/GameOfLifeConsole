using System;
using System.Collections.Generic;

namespace GameOfLifeConsole
{
    public class Grid
    {
        public List<Cell> CellGrid { get; }
        public int Width { get; }
        public int Height { get; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            CellGrid = new List<Cell>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    CellGrid.Add(new Cell(x, y));
                }
            }
        }

        public void AddLivingCell(Cell cell)
        {
            int cellIndex = CellGrid.FindIndex((Cell obj) => (obj.X == cell.X) && (obj.Y == cell.Y));
            cell.Alive = true;
            CellGrid[cellIndex] = cell;
        }

        public void UpdateGrid(List<Cell> newCells)
        {
            CellGrid.Clear();
            foreach (Cell cell in newCells)
            {
                CellGrid.Add(cell);
            }
        }

        public void ClearLivingCells()
        {
            foreach (Cell cell in CellGrid)
            {
                cell.Alive = false;
            }
        }

        public void PrintGrid()
        {
            Console.SetCursorPosition(0, 0);
            int gridIndex = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    CellGrid[gridIndex].PrintCell();
                    gridIndex++;
                }
                Console.WriteLine();
            }
        }
    }
}
