using System;
namespace GameOfLifeConsole
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public Neighbors CellNeighbors = new Neighbors(); //This was our way of adding a third class for the assignment.
        public bool Alive = false;

        /// <summary>
        /// Creates a Cell with a given X and a Y value for placement on a grid.
        /// The cell has Neighbors which is a list of any alive cells immediately surrounding the cell.
        /// The cell is default dead.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Adds a Cell to the Neighbors list.
        /// </summary>
        /// <param name="cell"></param>
        public void AddNeighbor(Cell cell)
        {
            CellNeighbors.NeighborCells.Add(cell);
        }

        /// <summary>
        /// Clears the Neighbors list.
        /// </summary>
        public void ClearNeighbors()
        {
            CellNeighbors.NeighborCells.Clear();
        }

        public void PrintCell()
        {
            Console.BackgroundColor = Alive ? ConsoleColor.White : ConsoleColor.Black;

            Console.Write(" ");
        }
    }
}
