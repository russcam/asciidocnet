#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"
#load @"Tooling.fsx"
#load @"Versioning.fsx"

open Fake 

open Paths
open Projects
open Versioning
open Tooling

type Release() = 
    static member NugetPack() =
        let currentVersion = Versioning.CurrentVersion.ToString()
        Projects.DotNetProject.AllPublishable
        |> Seq.iter(fun p ->
            CreateDir Paths.NugetOutput

            let name = p.Name;
            let nuspec = (sprintf @"build\%s.nuspec" name)
            let nugetOutFile =  Paths.Output(sprintf "%s.%s.nupkg" name currentVersion)

            Tooling.Nuget.Exec ["pack"; nuspec; "-version"; currentVersion; "-outputdirectory"; Paths.BuildOutput; ] |> ignore
            traceFAKE "%s" Paths.BuildOutput
            MoveFile Paths.NugetOutput nugetOutFile
        )