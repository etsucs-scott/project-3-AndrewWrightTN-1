using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Has all thegame logic and calculations
    /// </summary>
    public class GameBoard
    {
        /// <summary>
        /// Width and height of the board
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Total mines on the board
        /// </summary>
        public int MineCount { get; }

        /// <summary>
        /// 2d array for the board
        /// </summary>
        public Tile[,] Tiles { get; }

        /// <summary>
        /// Mine go boom
        /// </summary>
        public bool Explosion { get; private set; }

        /// <summary>
        /// Creates a new game with the seed 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="mineCount"></param>
        /// <param name="seed">determines the board layout</param>
        public GameBoard(int size, int mineCount, int seed)
        {
            Size = size;
            MineCount = mineCount;
            Tiles = new Tile[size, size];

            for (int i = 0; i < size; i++) 
                for (int j = 0; j < size; j++)
                    Tiles[i, j ] = new Tile();

            PlaceMines(seed);
            AdjacentOnes();
        }

        /// <summary>
        /// The mines are randomly placed 
        /// </summary>
        /// <param name="seed"></param>
        private void PlaceMines(int seed)
        {
            Random rng = new(seed);
            int placed;
            placed = 0;

            while (placed < MineCount)
            { 
                int i = rng.Next(Size);
                int j = rng.Next(Size);

                if (!Tiles[i, j].IsMine)
                {
                    Tiles[i, j].IsMine = true;
                    placed++;
                }
            }
        }

        /// <summary>
        /// This calculates the adjacent tiles
        /// </summary>
        private void AdjacentOnes()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0 ; j < Size; j++)
                {
                    if (Tiles[i, j].IsMine) continue;

                    int count; 
                    count = 0;
                    for (int k = -1; k <= 1; k++)
                    for (int l = -1; l <= 1; l++)
                        {
                            int nk = i + k;
                            int nj = j + l;
                            if (nk >= 0 && nk < Size && nj >= 0 && nj < Size && Tiles[nk, nj].IsMine)
                                count++;
                        }
                }
            }
        }

        /// <summary>
        /// Validates proper user placements
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <exception cref="InvalidMoveException">Throws exeption if the move is out of bounds</exception>
        private void Validation(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                throw new InvalidMoveException("Coordinates are out of the range.");
            }
        }

        /// <summary>
        /// Flags a spot if it is not revealed
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void ToggleFlag(int row, int col)
        {
            Validation(row, col);

            if (!Tiles[row, col].IsRevealed)
                Tiles[row, col].IsFlagged = !Tiles[row, col].IsFlagged;
        }

        /// <summary>
        /// Reveals the players move tile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void Reveal(int row, int col)
        {
            Validation(row, col);

            var tile = Tiles[row, col];
            if (tile.IsFlagged || tile.IsRevealed) 
                return;

            tile.IsRevealed = true;

            if (tile.IsMine)
            {
                Explosion = true;
                return;
            }

            if (tile.AdjacentMines == 0)
                CascadeReveal(row, col); 
        }
        
        /// <summary>
        /// This is reveal tile recursively as empty tiles are next to empty tiles
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void CascadeReveal(int row, int col)
        {
            for (int k = -1; k <= 1; k++)
            for (int l = -1; l <= 1; l++)
                {
                    int nk = row + k;
                    int nj = col + l;

                    if (nk < 0 || nk >= Size || nj < 0 || nj >= Size)
                        continue;

                    var nextTile = Tiles[nk, nj];
                    if (!nextTile.IsRevealed && !nextTile.IsMine && !nextTile.IsFlagged)
                    {
                        nextTile.IsRevealed = true;
                        if (nextTile.AdjacentMines == 0)
                            CascadeReveal(nk, nj);
                    }
                }
        }

        /// <summary>
        /// Registers when all non mine tiles are revealed
        /// </summary>
        /// <returns></returns>
        public bool HasWon()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (!Tiles[i, j].IsMine && !Tiles[i, j].IsRevealed)
                        return false;
            return true;
        }
    }
}
