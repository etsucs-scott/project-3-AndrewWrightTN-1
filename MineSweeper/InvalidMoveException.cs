
namespace MineSweeper
{
    [Serializable]
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message) : base(message) 
        { 
        
        }
    }
}