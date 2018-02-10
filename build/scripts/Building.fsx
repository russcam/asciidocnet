#I @"../../packages/build/FAKE/tools"
#I @"../../packages/build/FSharp.Data/lib/net45"
#r @"FakeLib.dll"
#r @"FSharp.Data.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"
#load @"Tooling.fsx"
#load @"Versioning.fsx"

open Fake 
open System 
open System.IO
open System.Reflection
open Fake 
open FSharp.Data 
open Paths
open Projects
open Tooling
open Versioning


type Build() = 

    static let runningRelease = hasBuildParam "version" || getBuildParam "target" = "release"
    static let pinnedSdkVersion = GlobalJson.Sdk.Version
    static let sln = Source("AsciiDocNet.sln")
    static let timeout = TimeSpan.FromMinutes 3.

    static let compileCore() =
      if not (DotNetCli.isInstalled()) then failwith  "You need to install the dotnet command line SDK to build for .NET Core"
      let runningSdkVersion = DotNetCli.getVersion()
      if (runningSdkVersion <> pinnedSdkVersion) then failwithf "Attempting to run with dotnet.exe with %s but global.json mandates %s" runningSdkVersion pinnedSdkVersion
      let sourceLink = if runningRelease then "true" else "false"
      let props = 
          [ 
              "CurrentVersion", (Versioning.CurrentVersion.ToString());
              "CurrentAssemblyVersion", (Versioning.CurrentAssemblyVersion.ToString());
              "CurrentAssemblyFileVersion", (Versioning.CurrentAssemblyFileVersion.ToString());
              "DoSourceLink", sourceLink;
              "OutputPathBaseDir", Path.GetFullPath Paths.BuildOutput;            
          ] 
          |> List.map (fun (p,v) -> sprintf "%s=%s" p v)
          |> String.concat ";"
          |> sprintf "/property:%s"
      
      DotNetCli.Build
          (fun p -> 
              { p with 
                  Configuration = "Release" 
                  Project = sln
                  TimeOut = timeout
                  AdditionalArgs = [props]
              }
          ) |> ignore
          
    static let compileDesktop target =
        Tooling.MsBuild.Build(target, Projects.DotNetFramework.Net45.Identifier)

    static member Restore() =
          DotNetCli.Restore
              (fun p ->
                  { p with 
                      Project = sln
                      TimeOut = timeout
                  }
              ) |> ignore 

    static member Compile() = 
        //compileDesktop "Rebuild"
        compileCore()

    static member Clean() =
        CleanDir Paths.BuildOutput
        DotNetCli.RunCommand 
            (fun p -> 
                { p with 
                    TimeOut = TimeSpan.FromMinutes(3.) 
                }
            ) "clean src/AsciiDocNet.sln -c Release" |> ignore
        Projects.DotNetProject.All |> Seq.iter(fun p -> CleanDir(Paths.BinFolder p.Name))