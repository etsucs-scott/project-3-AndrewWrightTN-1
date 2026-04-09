using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Records the board, movecount, seed, and time
    /// </summary>
    internal class Game
    {
        /// <summary>
        /// Current game board 
        /// </summary>
        public GameBoard Board { get; }

        /// <summary>
        /// Gets the total moves
        /// </summary>
        public int Moves { get; private set; }

        /// <summary>
        /// Seed determines the board layout
        /// </summary>
        public int Seed { get; }

        /// <summary>
        /// Gets the start time
        /// </summary>
        public DateTime Start { get; }

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <param name="size">size of the board</param>
        /// <param name="mineCount">amount of mines</param>
        /// <param name="seed">board layout</param>
        public Game(int size, int mineCount, int seed)
        {
            Seed = seed;
            Board = new GameBoard(size, mineCount, seed);
            Start = DateTime.Now;
        }

        /// <summary>
        /// Reveals the tiles next to each other when they are bombed
        /// </summary>
        /// <param name="row">the row</param>
        /// <param name="col">the column</param>
        public void Reveal(int row, int col) 
        { 
        Board.Reveal(row, col);
            Moves++;
        }

        /// <summary>
        /// Creates a toggle tile. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void Flag (int row, int col)
        {
            Board.ToggleFlag(row, col);
            Moves++;
        }

        /// <summary>
        /// Gets the amount of time in the game.
        /// </summary>
        /// <returns></returns>
        public int GetElapseSeconds() => (int)(DateTime.Now - Start).TotalSeconds;

    }
}
