using System;
using System.Collections.Generic;
using System.Text;

namespace battleships
{
	public class GameVisualizer : IGameVisualizer
	{
		public IEnumerable<string> Visualize(Game game)
		{
			yield return MapToString(game);
			yield return string.Format("Turn: {0}", game.TurnsCount);
			yield return string.Format("Last target: {0}", game.LastTarget);
			if (game.BadShots > 0)
				yield return string.Format("Bad shots: " + game.BadShots);
			if (game.IsOver())
				yield return "Game is over";
		}

		private string MapToString(Game game)
		{
			var map = game.Map;
			var sb = new StringBuilder();
			for (var y = 0; y < map.Height; y++)
			{
				for (var x = 0; x < map.Width; x++)
					sb.Append(GetSymbol(map[new Vector(x, y)]));
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private string GetSymbol(MapCell cell)
		{
			switch (cell)
			{
				case MapCell.Empty:
					return " ";
				case MapCell.Miss:
					return "*";
				case MapCell.Ship:
					return "O";
				case MapCell.DeadOrWoundedShip:
					return "X";
				default:
					throw new Exception(cell.ToString());
			}
		}
	}
}