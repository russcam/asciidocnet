[<AutoOpen>]
module Projects = 

    type DotNetFrameworkIdentifier = { 
        MSBuild: string; 
        Nuget: string; 
        DefineConstants: string; 
    }

    type DotNetFramework = 
        | Net45 
        | NetStandard13
        static member All = [Net45; NetStandard13] 
        member this.Identifier = 
            match this with
            | Net45 -> { MSBuild = "v4.5"; Nuget = "net45"; DefineConstants = "TRACE;NET45"; }
            | NetStandard13 -> { MSBuild = "netstandard1.3"; Nuget = "netstandard1.3"; DefineConstants = "TRACE;DOTNETCORE"; }

    type Project =
        | AsciiDocNet

    type PrivateProject =
        | AsciiDocNetTests

    type DotNetProject = 
        | Project of Project
        | PrivateProject of PrivateProject

        static member All = 
            seq [
                Project Project.AsciiDocNet; 
                PrivateProject PrivateProject.AsciiDocNetTests
            ]

        static member AllPublishable = seq [Project Project.AsciiDocNet;] 
        static member AsciiDocNetTests = seq [PrivateProject PrivateProject.AsciiDocNetTests;] 

        member this.Name =
            match this with
            | Project p ->
                match p with
                | AsciiDocNet -> "AsciiDocNet"
            | PrivateProject p ->
                match p with
                | AsciiDocNetTests -> "AsciiDocNet.Tests"
      
        static member TryFindName (name: string) =
            DotNetProject.All
            |> Seq.map(fun p -> p.Name)
            |> Seq.tryFind(fun p -> p.ToLowerInvariant() = name.ToLowerInvariant())

    type DotNetFrameworkProject = { framework: DotNetFramework; project: DotNetProject }
    
    let AllPublishableProjectsWithSupportedFrameworks = seq {
            for framework in DotNetFramework.All do
            for project in DotNetProject.AllPublishable do
                yield { framework = framework; project= project }
        }


