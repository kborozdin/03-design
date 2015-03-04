using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Diagnostics;

namespace battleships
{
	public delegate void OnVisualizationHandler(Game game);
	public delegate void OnUserConfirmationHandler();
	public delegate void OnMessageHandler(string msg);

	public class AiTester
	{
		private static readonly Logger resultsLog = LogManager.GetLogger("results");

		private readonly Settings settings;
		private readonly Func<string, Ai> createAi;
		private readonly Func<Ai, Game> createNewGame;
		private Ai ai;

		public event OnVisualizationHandler OnVisualization = delegate { };
		public event OnUserConfirmationHandler OnUserConfirmation = delegate { };
		public event OnMessageHandler OnMessage = delegate { };

		public AiTester(Settings settings, Func<string, Ai> createAi, Func<Ai, Game> createNewGame)
		{
			this.settings = settings;
			this.createAi = createAi;
			this.createNewGame = createNewGame;
		}

		public Statistics TestSingleFile(string exe)
		{
			var stats = new Statistics();
			using (ai = createAi(exe))
			{
				stats.SetName(ai.Name);
				for (var gameIndex = 0; gameIndex < settings.GamesCount; gameIndex++)
				{
					RunOneGame(exe, gameIndex, stats);
					if (stats.Crashes > settings.CrashLimit)
						break;
				}
			}
			return stats;
		}

		private void RunOneGame(string exe, int gameIndex, Statistics stats)
		{
			var game = createNewGame(ai);
			RunGameToEnd(game);
			stats.AddStatsOfGame(game);
			if (game.AiCrashed)
				ai = createAi(exe);
			if (settings.Verbose)
			{
				OnMessage(string.Format("Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
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
						OnMessage(game.LastError.Message.ToString());
					OnUserConfirmation();
				}
			}
		}
	}
}