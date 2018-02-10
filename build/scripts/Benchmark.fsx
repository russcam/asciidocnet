#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"
#load @"Tooling.fsx"
#load @"Versioning.fsx"

open System
open System.IO
open Fake
open Paths
open Projects
open Versioning
open Tooling

type Benchmark() = 
    static let benchmarksProjectName = (PrivateProject (AsciiDocNetBenchmarks)).Name

    static member Run() =
        let projectPath = Paths.Source benchmarksProjectName
        
        projectPath
        |> sprintf "run --project %s -c Release"
        |> DotNetCli.RunCommand (fun p -> { p with TimeOut = TimeSpan.FromMinutes(3.) })  
        |> ignore
        
        Paths.BenchmarkFolder |> DeleteDir 
        Paths.BenchmarkFolder |> CreateDir 
        Directory.EnumerateFiles(Paths.BenchmarkDotNetArtifacts, "*github.md") |> CopyFiles Paths.BenchmarkFolder 
        Directory.EnumerateFiles(Paths.BenchmarkDotNetArtifacts, "*json") |> CopyFiles (Paths.Benchmark "json")