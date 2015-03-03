using System;
using System.Collections.Generic;

namespace battleships
{
	public interface IGameVisualizer
	{
		IEnumerable<string> Visualize(Game game);
	}
}

