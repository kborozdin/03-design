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
			/*
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
			var aiPath = args[0];
			*/

			var kernel = new StandardKernel();
			//kernel.Bind<ISettings>().To<Settings1>();
			var x = kernel.Get<A>();
			//kernel.Bind<ISettings>().To<Settings>();
			//TODO
			//ISettings settings = new Settings("settings.txt"); //TODO
			//Settings settings = kernel.Get<Settings>();//(new ConstructorArgument("settingsFilename", "settings.txt"));
			/*
			//TODO what to do here?
			var random = new Random(settings.RandomSeed);

			//kernel.Bind<Random>().ToConstant<Random>(random);
			//kernel.Bind<ISettings>().ToConstant<ISettings>(settings);
			//kernel.Bind<IMapGenerator>().To<MapGenerator>();

			IMapGenerator gen = new MapGenerator(settings, random); //TODO
			//var gen = kernel.Get<IMapGenerator>();
			IGameVisualizer vis = new GameVisualizer(); //TODO
			IProcessMonitor monitor = new ProcessMonitor(settings); //TODO
			IAi ai = new Ai(aiPath, monitor); //TODO
			IAiTester tester = new AiTester(settings, gen, vis, monitor, ai); //TODO
			if (File.Exists(aiPath))
				tester.TestSingleFile(aiPath);
			else
				Console.WriteLine("No AI exe-file " + aiPath);
			*/
		}
	}
}