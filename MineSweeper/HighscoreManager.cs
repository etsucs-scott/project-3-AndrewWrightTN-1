namespace MineSweeper
{

    /// <summary>
    /// Saves the score in a csv and is limited to 5 to every board size
    /// </summary>
    public class HighscoreManager
    {

        /// <summary>
        /// This is the path to the cvs 
        /// </summary>
        private readonly string _filepath;

        public HighscoreManager(string filepath = "data/highscores.csv")
        {
            _filepath = filepath;
            EnsureFile();
        }

        public void EnsureFile()
        {
            string? dir = Path.GetDirectoryName(_filepath);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(_filepath))
                File.WriteAllText(_filepath, "size,seconds,moves,seed,timestamp");
        }

        public List<Highscores> LoadScores()
        {
            var scores = new List<Highscores>();
            var lines = File.ReadAllLines(_filepath);

            foreach (var line in lines)
            {
                try
                {
                    var parts = line.Split(',');
                    scores.Add(new Highscores
                    {
                        Size = int.Parse(parts[0]),
                        Seconds = int.Parse(parts[1]),
                        Moves = int.Parse(parts[2]),
                        Seed = int.Parse(parts[3]),
                        Time = DateTime.Parse(parts[4])
                    });
                }
                catch { continue; }
            }
            return scores;

        }

        public void ScoreRecord(Highscores score)
        {
            var scores = LoadScores();
            scores.Add(score);

            var trimmed = scores
            .GroupBy(s => s.Size)
                .SelectMany(group => group
                .OrderBy(s => s.Seconds)
                .ThenBy(s => s.Moves)
                .Take(5))
                .ToList();

            SaveScores(trimmed);
        }

        private void SaveScores(List<Highscores> scores)
        {
            var lines = new List<string> { "size,seconds,moves,seed,timestamp" };

            lines.AddRange(scores.Select(score => $"{score.Size},{score.Seconds},{score.Moves},{score.Seed},{score.Time}"));

            File.WriteAllLines(_filepath, lines);
        }
    }
}
