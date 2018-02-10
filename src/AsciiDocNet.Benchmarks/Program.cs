using System;
using BenchmarkDotNet.Running;

namespace AsciiDocNet.Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<ParseDocument>();
		}
	}
}