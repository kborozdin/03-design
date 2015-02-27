using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships
{
	class Statistics
	{
		public int BadShots { get; private set; }
		public int Crashes { get; private set; }
		public List<int> Shots { get; private set; }

		public Statistics()
		{
			BadShots = Crashes = 0;
			Shots = new List<int>();
		}

		public void AddToBadShots(int delta)
		{
			BadShots += delta;
		}

		public void HaveCrushed()
		{
			Crashes++;
		}

		public void AddShot(int shot)
		{
			Shots.Add(shot);
		}
	}
}
