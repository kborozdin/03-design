using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Diagnostics;

namespace battleships
{
	public delegate void OnVisualizationHandler(Game game);

	public class AiTester
	{
		private static readonly Logger resultsLog = LogManager.GetLogger("results");
		private readonly Settings settings;
		Action<string> messageLogger;
		Func<Map> generateMap;

		public event OnProcessRegisteredHandler OnProcessRegistered;
		public event OnVisualizationHandler OnVisualization;

		public AiTester(Settings settings, Func<Map> generateMap, Action<string> messageLogger)
		{
			this.settings = settings;
			this.generateMap = generateMap;
			this.messageLogger = messageLogger;
		}

		public void TestSingleFile(string exe)
		{
			var stats = new Statistics();
			var ai = new Ai(exe);
			ai.OnProcessRegistered += p => OnProcessRegistered(p);
			for (var gameIndex = 0; gameIndex < settings.GamesCount; gameIndex++)
			{
				RunOneGame(gameIndex, ai, stats);
				if (stats.Crashes > settings.CrashLimit)
					break;
			}
			ai.Dispose();
			WriteTotal(ai, stats, settings.GamesCount);
		}

		private void RunOneGame(int gameIndex, Ai ai, Statistics stats)
		{
			var map = generateMap();
			var game = new Game(map, ai);
			RunGameToEnd(game);
			stats.AddToBadShots(game.BadShots);
			if (game.AiCrashed)
			{
				stats.HaveCrushed();
				ai.Reset();
			}
			else
				stats.AddShot(game.TurnsCount);
			if (settings.Verbose)
			{
				messageLogger(string.Format("Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
					game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gameIndex));
			}
		}

		private void RunGameToEnd(Game game)
		{
			while (!game.IsOver())
			{
				game.MakeStep();
				if (settings.Interactive)
				{
					OnVisualization(game);
					if (game.AiCrashed)
						messageLogger(game.LastError.Message.ToString());
					Console.ReadKey();
				}
			}
		}

		private void WriteTotal(Ai ai, Statistics stats, int gamesPlayed)
		{
			var shots = stats.Shots;
			var badShots = stats.BadShots;
			var crashes = stats.Crashes;
			if (shots.Count == 0)
				shots.Add(1000 * 1000);
			shots.Sort();
			var median = shots.Count % 2 == 1 ? shots[shots.Count / 2] : (shots[shots.Count / 2] + shots[(shots.Count + 1) / 2]) / 2;
			var mean = shots.Average();
			var sigma = Math.Sqrt(shots.Average(s => (s - mean) * (s - mean)));
			var badFraction = (100.0 * badShots) / shots.Sum();
			var crashPenalty = 100.0 * crashes / settings.CrashLimit;
			var efficiencyScore = 100.0 * (settings.Width * settings.Height - mean) / (settings.Width * settings.Height);
			var score = efficiencyScore - crashPenalty - badFraction;
			var headers = FormatTableRow(new object[] { "AiName", "Mean", "Sigma", "Median", "Crashes", "Bad%", "Games", "Score" });
			var message = FormatTableRow(new object[] { ai.Name, mean, sigma, median, crashes, badFraction, gamesPlayed, score });
			resultsLog.Info(message);
			messageLogger("");
			messageLogger("Score statistics");
			messageLogger("================");
			messageLogger(headers);
			messageLogger(message);
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