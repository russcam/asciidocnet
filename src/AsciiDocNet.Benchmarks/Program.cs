using System;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace AsciiDocNet.Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			var switcher = BenchmarkSwitcher.FromAssembly(Assembly.GetCallingAssembly()); 
			switcher.Run(args);
		}
	}
}