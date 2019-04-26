namespace Scripts

open System
open System.Diagnostics
open System.IO
open System.Reflection

open Commandline
open Fake.Core
open Fake.IO
open Fake.IO.Globbing.Operators
open Newtonsoft.Json

module Versioning =
    
    let parse (v:string) = SemVer.parse(v)
    
    type SdkVersion = { version:string;  }
    type GlobalJson = { sdk: SdkVersion; version:string; }
     
    let private globalJson () =
        let jsonString = File.ReadAllText "global.json"
        JsonConvert.DeserializeObject<GlobalJson>(jsonString)

    let writeVersionIntoGlobalJson version =
        let globalJson = globalJson ()
        let newGlobalJson = { globalJson with version = version.ToString(); }
        File.WriteAllText("global.json", JsonConvert.SerializeObject(newGlobalJson))
        printfn "Written (%s) to global.json as the current version will use this version from now on as current in the build" (version.ToString()) 
    
    let GlobalJsonVersion = parse <| globalJson().version
    
    let private getVersion (args:Commandline.PassedArguments) =
        match (args.Target, args.CommandArguments) with
        | (_, SetVersion v) ->
            match v.Version with
            | v when String.IsNullOrEmpty v -> None
            | v -> Some <| parse v
        | ("canary", _) ->
            let v = GlobalJsonVersion
            let timestampedVersion = (sprintf "ci%s" (DateTime.UtcNow.ToString("yyyyMMddTHHmmss")))
            let canaryVersion = parse ((sprintf "%d.%d.0-%s" v.Major (v.Minor + 1u) timestampedVersion).Trim())
            Some canaryVersion
        | _ -> None
    
    
    type AnchoredVersion = { Full: SemVerInfo; Assembly:SemVerInfo; AssemblyFile:SemVerInfo }
    type BuildVersions =
        | Update of New: AnchoredVersion * Old: AnchoredVersion 
        | NoChange of Current: AnchoredVersion
        
    type ArtifactsVersion = ArtifactsVersion of AnchoredVersion

    let anchoredVersion version = 
        let av v = parse (sprintf "%s.0.0" (v.Major.ToString()))
        let fv v = parse (sprintf "%s.%s.%s.0" (v.Major.ToString()) (v.Minor.ToString()) (v.Patch.ToString()))
        { Full = version; Assembly = av version; AssemblyFile = fv version }
    
    let buildVersioning args =
        let currentVersion = GlobalJsonVersion
        let buildVersion = getVersion args
        match buildVersion with
        | None -> NoChange(Current = anchoredVersion currentVersion)
        | Some v -> Update(New = anchoredVersion v, Old = anchoredVersion currentVersion)
        
    let validate target version = 
        match (target, version) with
        | ("release", version) ->
            match version with
            | NoChange _ -> failwithf "cannot run release because no explicit version number was passed on the command line"
            | Update (newVersion, currentVersion) -> 
                // fail if current is greater or equal to the new version
                if (currentVersion >= newVersion) then
                    failwithf "Can not release %O its lower then current %O" newVersion.Full currentVersion.Full
                writeVersionIntoGlobalJson newVersion.Full
        | _ -> ignore()
    
    let artifactsVersion buildVersions =
        match buildVersions with
        | NoChange n -> ArtifactsVersion n
        | Update (newVersion, _) -> ArtifactsVersion newVersion
    
    let validateArtifacts (ArtifactsVersion(version)) =
        let fileVersion = version.AssemblyFile
        let tmp = "build/output/_packages/tmp"
        !! "build/output/_packages/*.nupkg"
        |> Seq.iter(fun f -> 
           Zip.unzip tmp f
           !! (sprintf "%s/**/*.dll" tmp)
           |> Seq.iter(fun f -> 
                let fv = FileVersionInfo.GetVersionInfo(f)
                let a = AssemblyName.GetAssemblyName(f).Version
                printfn "Assembly: %A File: %s Product: %s => %s" a fv.FileVersion fv.ProductVersion f
                if (a.Minor > 0 || a.Revision > 0 || a.Build > 0) then failwith (sprintf "%s assembly version is not sticky to its major component" f)
                if (parse (fv.ProductVersion) <> version.Full) then
                    failwith (sprintf "Expected product info %s to match new version %O " fv.ProductVersion fileVersion)
           )
           Directory.delete tmp
        )