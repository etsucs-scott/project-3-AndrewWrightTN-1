using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{

    /// <summary>
    /// Represents a tile on the board, will store mines, reveal, flag, and adjacent spot
    /// </summary>
    public class Tile
    {

        /// <summary>
        /// gets and set a mine on a tile
        /// </summary>
        public bool IsMine { get; set; }

        /// <summary>
        /// gets and sets a wether a tile is revealed
        /// </summary>
        public bool IsRevealed { get; set; }

        /// <summary>
        /// gets and sets wether a tile is flagged
        /// </summary>
        public bool IsFlagged { get; set; }
        
        /// <summary>
        /// gets and sets adjacent mines around the tile
        /// </summary>
        public int AdjacentMines { get; set; }
    }
}
