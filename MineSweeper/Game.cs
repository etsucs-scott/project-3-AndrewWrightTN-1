using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal class Game
    {
        public GameBoard Board { get; }
        public int Moves { get; private set; }
        public int Seed { get; }
        public DateTime Start { get; }

        public Game(int size, int mineCount, int seed)
        {
            Seed = seed;
            Board = new GameBoard(size, mineCount, seed);
            Start = DateTime.Now;
        }

        public void Reveal(int row, int col) 
        { 
        Board.Reveal(row, col);
            Moves++;
        }
        public void Flag (int row, int col)
        {
            Board.ToggleFlag(row, col);
            Moves++;
        }

        public int GetElapseSeconds() => (int)(DateTime.Now - Start).TotalSeconds;

    }
}
