using System;

namespace GameOfLifeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GameOfLife gameOfLife = new GameOfLife();
            gameOfLife.Run();
        }
    }
}
