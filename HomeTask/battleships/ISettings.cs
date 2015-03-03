using System;

namespace battleships
{
	public interface ISettings
	{
		int CrashLimit { get; }
		int GamesCount { get; }
		int Height { get; }
		bool Interactive { get; }
		int MemoryLimit { get; }
		int RandomSeed { get; }
		int[] Ships { get; }
		int TimeLimitSeconds { get; }
		bool Verbose { get; }
		int Width { get; }

		TimeSpan TimeLimit { get; }
	}
}

