namespace Scripts

open System.IO
open Fake.IO
open Projects
open Commandline

module Benchmark = 
    let private benchmarksProjectName = (PrivateProject (AsciiDocNetBenchmarks)).Name

    let run args =
        let projectPath = Paths.Source benchmarksProjectName
        let runCommand =
            let runCommandPrefix = "run -c Release"
            match args.RemainingArguments with
            | [] -> runCommandPrefix
            | _ ->
                args.RemainingArguments
                |> String.concat " "
                |> sprintf "%s -- %s" runCommandPrefix

        Tooling.DotNet.ExecIn projectPath [runCommand] |> ignore
        
        if Directory.Exists Paths.BenchmarkFolder then Paths.BenchmarkFolder |> Directory.Delete 
        Paths.BenchmarkFolder |> Directory.CreateDirectory |> ignore
        Directory.EnumerateFiles(Paths.BenchmarkDotNetArtifacts, "*github.md") |> Shell.copyFiles Paths.BenchmarkFolder 
        Directory.EnumerateFiles(Paths.BenchmarkDotNetArtifacts, "*json") |> Shell.copyFiles (Paths.Benchmark "json")