#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"
#r @"System.IO.Compression.FileSystem.dll"

#load "Projects.fsx"

open Fake
open Projects

[<AutoOpen>]
module Paths =
    let Repository = "https://github.com/russcam/asciidocnet"

    let BuildFolder = "build"
    let BuildOutput = sprintf "%s/output" BuildFolder

    let ProjectOutputFolder (project:DotNetProject) (framework:DotNetFramework) = 
        sprintf "%s/%s/%s" BuildOutput framework.Identifier.MSBuild project.Name

    let Tool tool = sprintf "packages/build/%s" tool
    let CheckedInToolsFolder = BuildFolder @@ "Tools"
    let KeysFolder = BuildFolder @@ "keys"
    let NugetOutput = BuildOutput @@ "_packages"
    let SourceFolder = "src"
    let BenchmarkFolder = "benchmarks"
    let BenchmarkDotNetArtifacts = "BenchmarkDotNet.Artifacts/results"
    
    let CheckedInTool tool = CheckedInToolsFolder @@ tool
    let Keys keyFile = KeysFolder @@ keyFile
    let Output folder = BuildOutput @@ folder
    let Source folder = SourceFolder @@ folder
    let Build folder = BuildFolder @@ folder
    let Benchmark folder = BenchmarkFolder @@ folder
    
    let BinFolder(folder) = 
        let f = replace @"\" "/" folder
        sprintf "%s/%s/bin/Release" SourceFolder f

    let ProjectFile(projectName) =
        Source(sprintf "%s/%s.csproj" projectName projectName)