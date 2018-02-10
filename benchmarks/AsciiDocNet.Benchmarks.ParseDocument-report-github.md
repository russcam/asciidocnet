``` ini

BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063.786)
Intel Core i7-2920XM CPU 2.50GHz (Sandy Bridge), 1 CPU, 8 logical cores and 4 physical cores
Frequency=2433514 Hz, Resolution=410.9284 ns, Timer=TSC
.NET Core SDK=2.1.4
  [Host]    : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT
  RyuJitX64 : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
| Method |     Mean |     Error |    StdDev |   Gen 0 |   Gen 1 | Allocated |
|------- |---------:|----------:|----------:|--------:|--------:|----------:|
|  Parse | 11.82 ms | 0.2347 ms | 0.4110 ms | 62.5000 | 15.6250 | 300.02 KB |
