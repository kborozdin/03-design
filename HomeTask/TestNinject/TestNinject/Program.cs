using System;
using Ninject;
using Ninject.Parameters;

namespace TestNinject
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var kernel = new StandardKernel();
			kernel.Bind<IA>().To<A>();
			var x = kernel.Get<IA>(new ConstructorArgument("x", 1));
		}
	}
}
