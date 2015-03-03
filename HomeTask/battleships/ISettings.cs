using System;

namespace battleships
{
	public interface ISettings
	{
		int CrashLimit { get; }
		int GamesCount { get; }
		int Height { get; }
		bool Interactive { get; }
		int MemoryLimit { get; set; }
		int RandomSeed { get; set; }
		int[] Ships { get; set; }
		int TimeLimitSeconds { get; set; }
		bool Verbose { get; set; }
		int Width { get; set; }

		TimeSpan TimeLimit { get; }
	}
}

