using System;

namespace battleships
{
	public interface ISettings
	{
		int CrashLimit { get; set; }
		int GamesCount { get; set; }
		int Height { get; set; }
		bool Interactive { get; set; }
		int MemoryLimit { get; set; }
		int RandomSeed { get; set; }
		int[] Ships { get; set; }
		int TimeLimitSeconds { get; set; }
		bool Verbose { get; set; }
		int Width { get; set; }

		TimeSpan TimeLimit { get; }
	}
}

