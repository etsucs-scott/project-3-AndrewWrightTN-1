using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class GameBoard
    {
        public int Size { get; }
        public int MineCount { get; }
        public Tile[,] Tiles { get; }
        public bool Explosion { get; private set; }

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

        public void ToggleFlag(int row, int col)
        {
            if (!Tiles[row, col].IsRevealed)
                Tiles[row, col].IsFlagged = !Tiles[row, col].IsFlagged;
        }

        public void Reveal(int row, int col)
        {
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
