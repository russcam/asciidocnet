namespace Scripts

open System.IO
open Fake.IO
open Projects
open Commandline

module Benchmark = 
    let private benchmarksProjectName = (PrivateProject (AsciiDocNetBenchmarks)).Name

    let run args =
        let projectPath = Paths.Source benchmarksProjectName
        
        let runInteractive = not args.NonInteractive
        let runCommandPrefix = "run -f netcoreapp2.1 -c Release"
        let runCommand =
            if runInteractive then runCommandPrefix
            else sprintf "%s -- --all" runCommandPrefix 

        Tooling.DotNet.ExecIn projectPath [runCommand] |> ignore
        
        Paths.BenchmarkFolder |> Directory.Delete 
        Paths.BenchmarkFolder |> Directory.CreateDirectory |> ignore
        Directory.EnumerateFiles(Paths.BenchmarkDotNetArtifacts, "*github.md") |> Shell.copyFiles Paths.BenchmarkFolder 
        Directory.EnumerateFiles(Paths.BenchmarkDotNetArtifacts, "*json") |> Shell.copyFiles (Paths.Benchmark "json")