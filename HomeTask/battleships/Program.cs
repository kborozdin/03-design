using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Ninject;
using Ninject.Parameters;

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

			var kernel = new StandardKernel();
			kernel.Bind<ISettings>().ToConstant<ISettings>(settings);
			kernel.Bind<Random>().ToConstant<Random>(random);
			kernel.Bind<TextWriter>().ToConstant<TextWriter>(Console.Out);

			kernel.Bind<IMapGenerator>().To<MapGenerator>();
			kernel.Bind<IGameVisualizer>().To<GameVisualizer>();
			kernel.Bind<IProcessMonitor>().To<ProcessMonitor>();
			kernel.Bind<IAi>().To<Ai>().WithConstructorArgument("exePath", aiPath);
			kernel.Bind<IAiTester>().To<AiTester>();

			var tester = kernel.Get<IAiTester>();

			if (File.Exists(aiPath))
				tester.TestSingleFile(aiPath);
			else
				Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}