using System.Linq.Expressions;

namespace MineSweeper
{
    internal class Program
    {
        /// <summary>
        /// Runs the game
        /// </summary>
        static void Main(string[] args)
        {
            HighscoreManager scoreManager = new HighscoreManager();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menu:\n1) 8x8\n2) 12x12\n3) 16x16");

                string choice = Console.ReadLine()!;

                int size = choice switch
                {
                    "1" => 8,
                    "2" => 12,
                    "3" => 16,
                    _ => 8
                };

                int mines = size switch
                {
                    8 => 10,
                    12 => 25,
                    _ => 40
                };

                Console.Write("Seed (blank = time): ");
                string seedInput = Console.ReadLine()!;
                int seed = string.IsNullOrWhiteSpace(seedInput)
                    ? DateTime.Now.Millisecond
                    : int.Parse(seedInput);

                Game game = new(size, mines, seed);

                while (!game.Board.Explosion && !game.Board.HasWon())
                {
                    RenderBoard(game.Board, false);

                    Console.WriteLine($"Seed: {seed}");
                    Console.Write($"Mines: {mines} ");
                    Console.WriteLine($"Commands: r row col | f row col | q");
                    Console.WriteLine("> ");

                    string input = Console.ReadLine()!;
                    try
                    {
                        string[] parts = input.Split(' ');

                        if (parts.Length == 1 && parts[0].ToLower() == "q")
                        {
                            break;
                        }
                        if (parts.Length != 3)
                        {
                            throw new InvalidMoveException("Format must be r row col or f row col");
                        }
                        string command = parts[0].ToLower();

                        if (!int.TryParse(parts[1], out int row) || !int.TryParse(parts[2], out int col))
                        {
                            throw new InvalidMoveException("Row and column gotta me numbers > 0");

                        }

                        switch (command)
                        {
                            case "r":
                                game.Reveal(row, col);
                                break;
                            case "f":
                                game.Flag(row, col);
                                break;
                            default:
                                throw new InvalidMoveException("Use r, f, or q");
                        }
                    }
                    catch (InvalidMoveException ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine($"Press enter");
                        Console.WriteLine();
                    }
                }

                RenderBoard(game.Board, true);

                if (game.Board.HasWon())
                {
                    Console.WriteLine($"You win");
                    Console.ReadLine();
                    scoreManager.ScoreRecord(new Highscores
                    {
                        Size = size,
                        Seconds = game.GetElapseSeconds(),
                        Moves = game.Moves,
                        Seed = seed,
                        Time = DateTime.Now
                    });

                    Console.WriteLine($"High Score Recoredd");
                    Console.ReadLine();
                }
                else if (game.Board.Explosion)
                {
                    Console.WriteLine($"Boom");
                    Console.ReadLine();
                }

                Console.WriteLine($"Press enter to return to menu");
                Console.WriteLine();

            }

            ///<summary>
            /// Loads the board 
            /// </summary>
            static void RenderBoard(GameBoard board, bool revealAll)
            {
                Console.Clear();
                Console.Write("  ");
                for (int c = 0; c < board.Size; c++)
                    Console.Write(c + " ");
                Console.WriteLine();
                for (int r = 0; r < board.Size; r++)
                {
                    Console.Write(r + " ");
                    for (int c = 0; c < board.Size; c++)
                    {
                        var tile = board.Tiles[r, c];
                        char symbol = '#';

                        if ((tile.IsRevealed || revealAll) && tile.IsMine) symbol = 'b';
                        else if (tile.IsFlagged) symbol = 'f';
                        else if (tile.IsRevealed)
                            symbol = tile.AdjacentMines == 0 ? '.' : tile.AdjacentMines.ToString()[0];

                        Console.Write(symbol + " ");

                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
