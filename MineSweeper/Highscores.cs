using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Highscores
    {        
        public DateTime Time { get; set; }
        public int Size { get; set; }
        public int Seconds { get; set; }
        public int Moves { get; set; }
        public int Seed { get; set; }
    }
}
