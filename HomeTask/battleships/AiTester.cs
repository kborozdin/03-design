using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace battleships
{
    public class AiTester : IAiTester
    {
        private static readonly Logger resultsLog = LogManager.GetLogger("results");
        private readonly ISettings settings;
        private readonly Func<string, IAi> createAi;
        private readonly Func<IAi, Game> createNewGame;
        private readonly IUserInterface userInterface;

        public AiTester(ISettings settings, Func<string, IAi> createAi, Func<IAi, Game> createNewGame, IUserInterface userInterface)
        {
            this.settings = settings;
            this.createAi = createAi;
            this.createNewGame = createNewGame;
            this.userInterface = userInterface;
        }

        public void TestSingleFile(string exe)
        {
            var badShots = 0;
            var crashes = 0;
            var gamesPlayed = 0;
            var shots = new List<int>();

			IAi ai = null;
            using (ai = createAi(exe))
            {
                for (var gameIndex = 0; gameIndex < settings.GamesCount; gameIndex++)
                {
                    var game = createNewGame(ai);
                    RunGameToEnd(game);
                    gamesPlayed++;
                    badShots += game.BadShots;
                    if (game.AiCrashed)
                    {
                        crashes++;
                        if (crashes > settings.CrashLimit)
							break;
						ai = createAi(exe);
                    }
                    else
                        shots.Add(game.TurnsCount);
					if (settings.Verbose)
					{
						userInterface.WriteLine(string.Format("Game #{3,4}: Turns {0,4}, BadShots {1}{2}", game.TurnsCount,
							game.BadShots, game.AiCrashed ? ", Crashed" : "", gameIndex));
					}
                }
                WriteTotal(shots, crashes, badShots, gamesPlayed, ai.Name);
            }
        }

        private void RunGameToEnd(Game game)
        {
            while (!game.IsOver())
            {
                game.MakeStep();
                if (settings.Interactive)
                {
                    userInterface.Visualize(game);
                    if (game.AiCrashed)
                        userInterface.WriteLine(game.LastError.Message);
                    userInterface.ReadKey();
                }
            }
        }

        private void WriteTotal(List<int> shots, int crashes, int badShots, int gamesPlayed, string aiName)
        {
            if (shots.Count == 0) shots.Add(1000*1000);
            shots.Sort();
			var median = shots.Count % 2 == 1
				? shots[shots.Count / 2]
				: (shots[shots.Count / 2] + shots[(shots.Count + 1) / 2]) / 2;
            var mean = shots.Average();
			var sigma = Math.Sqrt(shots.Average(s => (s - mean) * (s - mean)));
            var badFraction = (100.0*badShots)/shots.Sum();
            var crashPenalty = 100.0*crashes/settings.CrashLimit;
			var efficiencyScore = 100.0 * (settings.Width * settings.Height - mean) / (settings.Width * settings.Height);
            var score = efficiencyScore - crashPenalty - badFraction;
			var headers =
				FormatTableRow(new object[] { "AiName", "Mean", "Sigma", "Median", "Crashes", "Bad%", "Games", "Score" });
			var message =
				FormatTableRow(new object[] { aiName, mean, sigma, median, crashes, badFraction, gamesPlayed, score });
            resultsLog.Info(message);
            userInterface.WriteLine("");
			userInterface.WriteLine("Score statistics");
            userInterface.WriteLine("================");
            userInterface.WriteLine(headers);
            userInterface.WriteLine(message);
        }

        private string FormatTableRow(object[] values)
        {
            return FormatValue(values[0], 15)
                   + string.Join(" ", values.Skip(1).Select(v => FormatValue(v, 7)));
        }

        private static string FormatValue(object v, int width)
        {
            return v.ToString().Replace("\t", " ").PadRight(width).Substring(0, width);
        }
    }
}