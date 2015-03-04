using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships
{
	public class Statistics
	{
		public int BadShots { get; private set; }
		public int Crashes { get; private set; }
		public List<int> Shots { get; private set; }
		public int GamesPlayed { get; private set; }
		public string AiName { get; private set; }

		public Statistics()
		{
			Shots = new List<int>();
		}

		public void AddStatsOfGame(Game game)
		{
			GamesPlayed++;
			BadShots += game.BadShots;
			if (game.AiCrashed)
				Crashes++;
			else
				Shots.Add(game.TurnsCount);
		}

		public void SetName(string aiName)
		{
			AiName = aiName;
		}
	}
}
