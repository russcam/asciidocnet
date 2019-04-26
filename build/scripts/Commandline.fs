namespace Scripts

open System.Runtime.InteropServices
open Fake.Core

module Commandline =

    let private usage = """
USAGE:

build <target> [params]
"""

    type VersionArguments = { Version: string; }
    type TestArguments = { TestFilter: string option; }
    type BenchmarkArguments = { Endpoint: string; Username: string option; Password: string option; }

    type CommandArguments =
        | Unknown
        | SetVersion of VersionArguments
        | Test of TestArguments
        | Benchmark of BenchmarkArguments

    type PassedArguments = {
        NonInteractive: bool;
        SkipTests: bool;
        RemainingArguments: string list;
        Target: string;
        NeedsFullBuild: bool;
        NeedsClean: bool;
        DoSourceLink: bool;
        CommandArguments: CommandArguments;
    }

    let notWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
        
    let runningOnCi = Environment.hasEnvironVar "TF_BUILD" || Environment.hasEnvironVar "APPVEYOR_BUILD_VERSION"
    
    let parse (args: string list) =
        
        let filteredArgs = 
            args
            |> List.filter(fun x -> 
               x <> "skiptests" && 
               x <> "non-interactive")
               
        let target = 
            match (filteredArgs |> List.tryHead) with
            | Some t -> t
            | _ -> "build"
            
        let skipTests = args |> List.exists (fun x -> x = "skiptests")

        let parsed = {
            NonInteractive = args |> List.exists (fun x -> x = "non-interactive")
            SkipTests = skipTests
            RemainingArguments = filteredArgs
            Target = 
                match (filteredArgs |> List.tryHead) with
                | Some t -> t.Replace("-one", "")
                | _ -> "build"
            NeedsFullBuild = 
                match (target, skipTests) with
                | (_, true) -> true
                //dotnet-xunit needs to a build of its own anyways
                | ("test", _) -> false
                | _ -> true
            NeedsClean = 
                match (target, skipTests) with
                //dotnet-xunit needs to a build of its own anyways
                | ("test", _)
                | ("build", _) -> false
                | _ -> true;
            CommandArguments = Unknown
            DoSourceLink = false
        }
            
        let arguments =
            match filteredArgs with
            | _ :: tail -> target :: tail
            | [] -> [target]
        
        match arguments with
        | [] | ["build"] | ["test"] | ["clean"] | ["benchmark"] -> parsed
        | _ ->
            eprintf "%s" usage
            failwith "Please consult printed help text on how to call our build"
