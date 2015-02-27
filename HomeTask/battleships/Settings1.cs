using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninject;

namespace battleships
{
	public class Settings1 : ISettings
	{
		public int CrashLimit { get; set; }
		public int GamesCount { get; set; }
		public int Height { get; set; }
		public bool Interactive { get; set; }
		public int MemoryLimit { get; set; }
		public int RandomSeed { get; set; }
		public int[] Ships { get; set; }
		public int TimeLimitSeconds { get; set; }
		public bool Verbose { get; set; }
		public int Width { get; set; }

		public TimeSpan TimeLimit
		{
			get
			{
				return TimeSpan.FromSeconds(TimeLimitSeconds * GamesCount);
			}
		}

		public Settings1()
		{
			//Console.WriteLine("1");
		}
		/*
		public Settings(string settingsFilename)
		{
			Console.WriteLine("2");
			var lines = File.ReadAllLines(settingsFilename)
				.Where(line => !string.IsNullOrWhiteSpace(line))
				.Select(line => line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries))
				.ToList();
			Interactive = GetBool(lines, "interactive");
			Verbose = GetBool(lines, "verbose");
			GamesCount = GetInt(lines, "gamesCount", 1);
			Width = GetInt(lines, "width", 20);
			Height = GetInt(lines, "height", 20);
			RandomSeed = GetInt(lines, "randomSeed", Environment.TickCount);
			Ships = Get(lines, "ships").Select(int.Parse).ToArray();
			TimeLimitSeconds = GetInt(lines, "timelimitSeconds", 60);
			MemoryLimit = GetInt(lines, "memoryLimit", 500*1024*1024);
			CrashLimit = GetInt(lines, "crashlimit", 1);
		}
*/

		private IEnumerable<string> Get1(IEnumerable<string[]> lines, string key)
		{
			return
				lines.Where(line => line[0].Equals(key, StringComparison.InvariantCultureIgnoreCase))
					.SelectMany(line => line.Skip(1));
		}

		private bool GetBool(List<string[]> lines, string key)
		{
			return Get1(lines, key).FirstOrDefault() == "true";
		}

		private int GetInt(IEnumerable<string[]> lines, string key, int defaultValue)
		{
			int v;
			return int.TryParse(Get1(lines, key).SingleOrDefault(), out v) ? v : defaultValue;
		}
	}
}