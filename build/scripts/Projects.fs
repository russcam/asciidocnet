namespace Scripts

module Projects = 

    type DotNetFrameworkIdentifier = { 
        MSBuild: string; 
        Nuget: string; 
        DefineConstants: string; 
    }

    type DotNetFramework = 
        | NetStandard20
        | NetCoreApp22
        static member All = [NetStandard20] 
        member this.Identifier = 
            match this with
            | NetStandard20 -> { MSBuild = "netstandard2.0"; Nuget = "netstandard2.0"; DefineConstants = ""; }
            | NetCoreApp22 -> { MSBuild = "netcoreapp2.2"; Nuget = "netcoreapp2.2"; DefineConstants = ""; }

    type Project =
        | AsciiDocNet

    type PrivateProject =
        | AsciiDocNetTests
        | AsciiDocNetBenchmarks

    type DotNetProject = 
        | Project of Project
        | PrivateProject of PrivateProject

        static member All = 
            seq [
                Project Project.AsciiDocNet; 
                PrivateProject PrivateProject.AsciiDocNetTests
                PrivateProject PrivateProject.AsciiDocNetBenchmarks
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
                | AsciiDocNetBenchmarks -> "AsciiDocNet.Benchmarks"
                
        static member TryFindName (name: string) =
            DotNetProject.All
            |> Seq.map(fun p -> p.Name)
            |> Seq.tryFind(fun p -> p.ToLowerInvariant() = name.ToLowerInvariant())

    type DotNetFrameworkProject = { framework: DotNetFramework; project: DotNetProject }
    
    let allPublishableProjectsWithSupportedFrameworks = seq {
        for framework in DotNetFramework.All do
        for project in DotNetProject.AllPublishable do
            yield { framework = framework; project= project }
    }


