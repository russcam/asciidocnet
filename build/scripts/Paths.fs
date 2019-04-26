namespace Scripts

open Fake.IO
open Projects

module Paths =
    
    let Repository = "https://github.com/russcam/asciidocnet"

    let BuildFolder = "build"
    let BuildOutput = sprintf "%s/output" BuildFolder

    let ProjectOutputFolder (project:DotNetProject) (framework:DotNetFramework) = 
        sprintf "%s/%s/%s" BuildOutput framework.Identifier.MSBuild project.Name

    let NugetOutput = Path.combine BuildOutput "_packages"
    let SourceFolder = "src"
    let BenchmarkFolder = "benchmarks"
    let BenchmarkDotNetArtifacts = "BenchmarkDotNet.Artifacts/results"
     
    let Output folder = Path.combine BuildOutput folder
    let Source folder = Path.combine SourceFolder folder
    let Build folder = Path.combine BuildFolder folder
    let Benchmark folder = Path.combine BenchmarkFolder folder
    
    let Solution = Source "AsciiDocNet.sln"
    
    let binFolder (folder:string) = 
        let f = folder.Replace(@"\", "/")
        sprintf "%s/%s/bin/Release" SourceFolder f