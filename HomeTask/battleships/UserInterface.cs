using System;
using System.IO;

namespace battleships
{
    class UserInterface : IUserInterface
    {
		private readonly bool isInteractive;
		private readonly IGameVisualizer vis;

        public UserInterface(bool isInteractive, IGameVisualizer vis)
        {
			this.isInteractive = isInteractive;
			this.vis = vis;
        }

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public char ReadKey()
        {
			return isInteractive ? Console.ReadKey().KeyChar : '\0';
        }

		public void Visualize(Game game)
		{
			Console.Clear();
			foreach (string s in vis.Visualize(game))
				WriteLine(s);
		}
    }
}