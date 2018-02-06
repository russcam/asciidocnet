#I @"../../packages/build/FAKE/tools"
#I @"../../packages/build/FSharp.Data/lib/net45"
#r @"FakeLib.dll"
#r @"FSharp.Data.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions
open FSharp.Data
open Fake 
open AssemblyInfoFile
open SemVerHelper
open Projects
open Paths

type Versioning() = 
     
    static let writeVersionIntoGlobalJson version = 
        let versionString = version.ToString()
        let newGlobalJson = GlobalJsonProvider.Root (GlobalJsonProvider.Sdk(GlobalJson.Sdk.Version), versionString)
        use tw = new StreamWriter("global.json")
        newGlobalJson.JsonValue.WriteTo(tw, JsonSaveOptions.None)
        tracefn "Written (%s) to global.json as the current version will use this version from now on as current in the build" versionString 
        version

    static member CurrentVersion =
        let currentVersion = parse(GlobalJson.Version)
        let buildVersion = getBuildParam "version"
        let versionToUse = if (isNullOrEmpty buildVersion) then None else Some(parse(buildVersion)) 
        match (getBuildParam "target", versionToUse) with
        | ("release", None) -> failwithf "cannot run release because no explicit version number was passed on the command line"
        | ("release", Some v) -> 
            if (currentVersion >= v) then 
                traceImportant (sprintf "creating release %s when current version is already at %s" (v.ToString()) (currentVersion.ToString()))
            writeVersionIntoGlobalJson v
        | _ -> 
            tracefn "Not running 'release' target so using version in global.json (%s) as current" (currentVersion.ToString())
            currentVersion

    static member CurrentAssemblyVersion = 
        sprintf "%i.0.0" Versioning.CurrentVersion.Major |> parse
    
    static member CurrentAssemblyFileVersion = 
        sprintf "%i.%i.%i.0" Versioning.CurrentVersion.Major Versioning.CurrentVersion.Minor Versioning.CurrentVersion.Patch |> parse

    static member ValidateArtifacts() =
        let currentVersion = Versioning.CurrentVersion
        let tmp = "build/output/_packages/tmp"
        !! "build/output/_packages/*.nupkg"
        |> Seq.iter(fun f -> 
           Unzip tmp f
           !! (sprintf "%s/**/*.dll" tmp)
           |> Seq.iter(fun f -> 
                let fv = FileVersionInfo.GetVersionInfo(f)
                let a = GetAssemblyVersion f
                traceFAKE "Assembly: %A File: %s Product: %s => %s" a fv.FileVersion fv.ProductVersion f
                if (a.Minor > 0 || a.Revision > 0 || a.Build > 0) then 
                    failwith (sprintf "%s assembly version is not sticky to its major component" f)
                if (parse (fv.ProductVersion) <> currentVersion) then 
                    failwith (sprintf "Expected product info %s to match new version %s " fv.ProductVersion (currentVersion.ToString()))
           )
           DeleteDir tmp
        )