using System;
using System.Collections.Generic;

namespace GameOfLifeConsole
{
    public class Neighbors
    {
        /// <summary>
        /// A list of living cells used in the Cell class to keep track of the living cells surrounding it.
        /// </summary>
        public List<Cell> NeighborCells = new List<Cell>();
    }
}
