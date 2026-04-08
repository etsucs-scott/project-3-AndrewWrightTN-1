namespace MineSweeper
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                    Console.Write("> ");
                    string input = Console.ReadLine()!;

                    if (input == "q") break;

                    string[] parts = input.Split(' ');
                    if (parts.Length != 3)
                        continue;


                    string cmd = parts[0];
                    int row = int.Parse(parts[1]);
                    int col = int.Parse(parts[2]);

                    if (cmd == "r") game.Reveal(row, col);
                    if (cmd == "f") game.Flag(row, col);
                }

                RenderBoard(game.Board, true);
                Console.WriteLine(game.Board.HasWon() ? "You Win!" : "Game Over!");
                Console.WriteLine("Press Enter to return to menu...");
                Console.ReadLine();
            }

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
