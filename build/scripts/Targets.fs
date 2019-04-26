namespace Scripts

open System
open Bullseye
open ProcNet
open Build
open Commandline

module Main =

    let private target name action = Targets.Target(name, new Action(action)) 
    let private skip name = printfn "SKIPPED target '%s' evaluated not to run" name |> ignore
    let private conditional condition name action = target name (if condition then action else (fun _ -> skip name)) 
    let private command name dependencies action = Targets.Target(name, dependencies, new Action(action))
    
    let [<EntryPoint>] main args = 
        
        let parsed = Commandline.parse (args |> Array.toList)
        
        let buildVersions = Versioning.buildVersioning parsed
        let artifactsVersion = Versioning.artifactsVersion buildVersions
        Versioning.validate parsed.Target buildVersions
        
        target "touch" <| fun _ -> printfn "Touching build %O" artifactsVersion

        target "start" <| fun _ -> printfn "STARTING BUILD"

        conditional parsed.NeedsClean "clean" Build.clean 

        conditional parsed.NeedsFullBuild "full-build" <| fun _ -> Build.compile parsed artifactsVersion

        conditional (not parsed.SkipTests) "test" <| fun _ -> Tests.run parsed |> ignore
        
        target "version" <| fun _ -> printfn "Artifacts Version: %O" artifactsVersion

        target "restore" restore

        target "test-nuget-package" <| fun _ -> 
            if not Commandline.runningOnCi then ignore ()
            else Tests.run artifactsVersion |> ignore
            
        target "nuget-pack" <| fun _ -> Release.nugetPack artifactsVersion

        target "validate-artifacts" <| fun _ -> Versioning.validateArtifacts artifactsVersion
        
        // the following are expected to be called as targets directly        
        let buildChain = [
            "clean"; "version"; "restore"; "full-build"; "test"; 
        ]
        
        command "build" buildChain <| fun _ -> printfn "STARTING BUILD"

        command "benchmark" [ "clean"; "full-build"; ] <| fun _ -> Benchmark.run parsed

        command "canary" [ "version"; "release"; "test-nuget-package";] <| fun _ -> printfn "Finished Release Build %O" artifactsVersion

        command "release" [ 
           "build"; "nuget-pack"; "validate-artifacts";
        ] (fun _ -> printfn "Finished Release Build %O" artifactsVersion)

        Targets.RunTargetsAndExit([parsed.Target], fun e -> e.GetType() = typeof<ProcExecException>)

        0

