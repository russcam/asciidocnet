namespace Scripts

open Fake.IO
open Projects

module Paths =
    
    let Repository = "https://github.com/russcam/asciidocnet"

    let SourceFolder = "src"  
    let BuildFolder = "build"
    let BuildOutput = Path.combine BuildFolder "output"
    let NugetOutput = Path.combine BuildOutput "_packages"  
    let BenchmarkFolder = "benchmarks"
    
    let Source folder = Path.combine SourceFolder folder
    let ProjectFolder (project:DotNetProject) = project.Name |> Source
    
    let ProjectOutputFolder (project:DotNetProject) (framework:DotNetFramework) =
        sprintf "%s/%s/%s" BuildOutput framework.Identifier.MSBuild project.Name
          
    let Output folder = Path.combine BuildOutput folder    
    let Build folder = Path.combine BuildFolder folder
    let Benchmark folder = Path.combine BenchmarkFolder folder
    let BenchmarkDotNetArtifacts =
        let projectFolder = ProjectFolder (PrivateProject AsciiDocNetBenchmarks)
        Path.combine projectFolder "BenchmarkDotNet.Artifacts/results"
    
    let Solution = Source "AsciiDocNet.sln"
    
    let binFolder (folder:string) = 
        let f = folder.Replace(@"\", "/")
        sprintf "%s/%s/bin/Release" SourceFolder f