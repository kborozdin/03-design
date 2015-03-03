using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Ninject;
using Ninject.Parameters;
using Ninject.Extensions.Conventions;

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
			kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
			kernel.Rebind<ISettings>().ToConstant<Settings>(settings);
			kernel.Rebind<Random>().ToConstant<Random>(random);
			kernel.Rebind<IProcessMonitor>().To<ProcessMonitor>().InSingletonScope();
			kernel.Rebind<IUserInterface>().To<UserInterface>().WithConstructorArgument<bool>(settings.Interactive);
			kernel.Rebind<IAiTester>().To<AiTester>()
				.WithConstructorArgument<Func<string, IAi>>(s => kernel.Get<IAi>(new ConstructorArgument("exePath", s)))
				.WithConstructorArgument<Func<IAi, Game>>(ai => new Game(kernel.Get<IMapGenerator>().GenerateMap(), ai));

			var tester = kernel.Get<IAiTester>();

			if (File.Exists(aiPath))
				tester.TestSingleFile(aiPath);
			else
				Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}