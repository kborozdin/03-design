using System;

namespace battleships
{
	public interface IUserInterface
	{
		void WriteLine(string line);
		char ReadKey();
		void Visualize(Game game);
	}
}
