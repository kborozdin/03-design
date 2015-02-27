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

			var gen = new MapGenerator(settings, random);
			var vis = new GameVisualizer();
			var monitor = new ProcessMonitor(settings.TimeLimit, settings.MemoryLimit);

			var tester = new AiTester(settings, gen.GenerateMap, msg => Console.WriteLine(msg));
			tester.OnProcessRegistered += monitor.Register;
			tester.OnVisualization += vis.Visualize;

			if (File.Exists(aiPath))
				tester.TestSingleFile(aiPath);
			else
				Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}