using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace battleships
{
	public class Program
	{
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
			var aiPath = args[0];

			var settings = new Settings("settings.txt");
			var random = new Random(settings.RandomSeed);
			Action<string> consolePrinter = s => Console.WriteLine(s);

			var gen = new MapGenerator(settings, random);
			var vis = new GameVisualizer();
			var monitor = new ProcessMonitor(settings.TimeLimit, settings.MemoryLimit);

			var tester = new AiTester(
				settings,
				s => new Ai(s, monitor.Register),
				ai => new Game(gen.GenerateMap(), ai));

			tester.OnVisualization += g =>
				{
					Console.Clear();
					foreach (string line in vis.Visualize(g))
						Console.WriteLine(line);
				};
			tester.OnUserConfirmation += () =>
				{
					if (settings.Interactive)
						Console.ReadKey();
				};
			tester.OnMessage += s => consolePrinter(s);

			if (File.Exists(aiPath))
			{
				var stats = tester.TestSingleFile(aiPath);
				new StatisticsFormatter(settings, stats).WriteTotal(consolePrinter);
			}
			else
				Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}