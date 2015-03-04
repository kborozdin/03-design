using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships
{
	public class StatisticsFormatter
	{
		private static readonly Logger resultsLog = LogManager.GetLogger("results");

		Settings settings;
		Statistics stats;

		public StatisticsFormatter(Settings settings, Statistics stats)
		{
			this.settings = settings;
			this.stats = stats;
		}

		public void WriteTotal(Action<string> printer)
		{
			var shots = stats.Shots;
			var badShots = stats.BadShots;
			var crashes = stats.Crashes;
			var gamesPlayed = stats.GamesPlayed;
			var aiName = stats.AiName;

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
			var message = FormatTableRow(new object[] { aiName, mean, sigma, median, crashes, badFraction, gamesPlayed, score });
			resultsLog.Info(message);
			printer("");
			printer("Score statistics");
			printer("================");
			printer(headers);
			printer(message);
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
