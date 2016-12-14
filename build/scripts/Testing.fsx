#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"
#load @"Tooling.fsx"

open System.IO
open Fake 
open Paths
open Projects
open Tooling

[<AutoOpen>]
module Tests = 
    let private testProjectJson parallelization =
        Projects.DotNetProject.All
        |> Seq.iter(fun p -> 
            let path = Paths.ProjectJson p.Name
            Tooling.DotNet.Exec ["restore"; path]
        )

        let testPath = Paths.Source "AsciiDocNet.Tests/project.json"
        Tooling.DotNet.Exec ["restore"; testPath]
        Tooling.DotNet.Exec ["build"; testPath; "--configuration Release"; "-f"; "netcoreapp1.0"]
        Tooling.DotNet.Exec ["test"; testPath; "-parallel"; parallelization; "-xml"; Paths.Output("TestResults-Core-Clr.xml")]

    let private testDesktopClr parallelization = 
        let folder = Paths.ProjectOutputFolder (PrivateProject PrivateProject.AsciiDocNetTests) Projects.DotNetFramework.Net45
        let testDll = Path.Combine(folder, "AsciiDocNet.Tests.dll")
        Tooling.XUnit.Exec [testDll; "-parallel"; parallelization; "-xml"; Paths.Output("TestResults-Desktop-Clr.xml")] 
        |> ignore
        
    let RunUnitTests() = 
        testDesktopClr "all"
        testProjectJson "all"