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
    let testProjectName = (PrivateProject (AsciiDocNetTests)).Name

    let private testNetCore parallelization =
        Projects.DotNetProject.All
        |> Seq.iter(fun p -> 
            let path = Paths.ProjectFile p.Name
            Tooling.DotNet.Exec ["restore"; path]
        )

        let testPath = Paths.ProjectFile testProjectName
        let testDir  = testProjectName |> Path.GetDirectoryName
        
        Tooling.DotNet.Exec ["restore"; testPath]
        Tooling.DotNet.Exec ["build"; testPath; "--configuration Release"; "-f"; "netcoreapp1.0"]
        
        tracefn "TEST DIR: %s" testDir     
        Tooling.DotNet.ExecIn "src/AsciiDocNet.Tests" ["xunit"; "-parallel"; parallelization; "-xml"; "../.." @@ Paths.Output("TestResults-Core-Clr.xml")]

    let private testDesktopClr parallelization = 
        let folder = Paths.ProjectOutputFolder (PrivateProject PrivateProject.AsciiDocNetTests) Projects.DotNetFramework.Net45
        let testDll = Path.Combine(folder, sprintf "%s.dll" testProjectName)
        Tooling.XUnit.Exec [testDll; "-parallel"; parallelization; "-xml"; Paths.Output("TestResults-Desktop-Clr.xml")] 
        |> ignore
        
    let RunUnitTests() = 
        testNetCore "all"