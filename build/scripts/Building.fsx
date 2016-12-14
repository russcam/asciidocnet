#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"
#load @"Tooling.fsx"

open Fake 

open Paths
open Projects
open Tooling

type Build() = 

    static let runningRelease = hasBuildParam "version" || getBuildParam "target" = "release"

    static let compileCore() =
        Projects.DotNetProject.AllPublishable
        |> Seq.iter(fun p -> 
            let path = Paths.ProjectJson p.Name
            let o = Paths.ProjectOutputFolder p Projects.DotNetFramework.NetStandard13
            Tooling.DotNet.Exec ["restore"; path]
            Tooling.DotNet.Exec ["build"; path; "--configuration Release"; "-o"; o; "-f"; Projects.DotNetFramework.NetStandard13.Identifier.MSBuild]
        )

    static let compileDesktop target =
        Tooling.MsBuild.Build(target, Projects.DotNetFramework.Net45.Identifier)

    static let gitLink() =
        Projects.DotNetProject.AllPublishable
        |> Seq.iter(fun p ->
            let projectName = (p.Name |> directoryInfo).Name
            let link framework = 
                Tooling.GitLink.Exec ["."; "-u"; Paths.Repository; "-d"; (Paths.ProjectOutputFolder p framework); "-include"; projectName] 
                |> ignore
            link Projects.DotNetFramework.Net45
            link Projects.DotNetFramework.NetStandard13
        )
        
    static member Compile() = 
        compileDesktop "Rebuild"
        if runningRelease then compileCore()
        if not isMono && runningRelease then gitLink()

    static member Clean() =
        CleanDir Paths.BuildOutput
        Projects.DotNetProject.All |> Seq.iter(fun p -> CleanDir(Paths.BinFolder p.Name))