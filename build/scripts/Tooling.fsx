#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"
#r @"System.IO.Compression.FileSystem.dll"

#load "Projects.fsx"
#load "Paths.fsx"

open System
open System.IO
open System.Diagnostics
open System.Net

open Fake

open Projects
open Paths

[<AutoOpen>]
module Tooling = 

    let private fileDoesNotExist path = path |> Path.GetFullPath |> File.Exists |> not
    let private dirDoesNotExist path = path |> Path.GetFullPath |> Directory.Exists |> not
    let private doesNotExist path = (fileDoesNotExist path) && (dirDoesNotExist path)

    (* helper functions *)
    #if mono_posix
    #r "Mono.Posix.dll"
    open Mono.Unix.Native
    let private applyExecutionPermissionUnix path =
        let _,stat = Syscall.lstat(path)
        Syscall.chmod(path, FilePermissions.S_IXUSR ||| stat.st_mode) |> ignore
    #else
    let private applyExecutionPermissionUnix path = ()
    #endif

    let private execAt (workingDir:string) (exePath:string) (args:string seq) =
        let processStart (psi:ProcessStartInfo) =
            let ps = Process.Start(psi)
            ps.WaitForExit ()
            ps.ExitCode
        let fullExePath = exePath |> Path.GetFullPath
        applyExecutionPermissionUnix fullExePath
        let exitCode = 
            ProcessStartInfo(
                        fullExePath,
                        args |> String.concat " ",
                        WorkingDirectory = (workingDir |> Path.GetFullPath),
                        UseShellExecute = false) 
                   |> processStart
        if exitCode <> 0 then
            exit exitCode
        ()


    let execProcessWithTimeout proc arguments timeout = 
        let args = arguments |> String.concat " "
        ExecProcess (fun info ->
            info.FileName <- proc
            info.WorkingDirectory <- "."
            info.Arguments <- args
        ) timeout

    let execProcessWithTimeoutAndReturnMessages proc arguments timeout = 
        let args = arguments |> String.concat " "
        let code = 
            ExecProcessAndReturnMessages (fun info ->
            info.FileName <- proc
            info.WorkingDirectory <- "."
            info.Arguments <- args
            ) timeout
        code

    let private defaultTimeout = TimeSpan.FromMinutes 15.0

    let execProcess proc arguments =
        let exitCode = execProcessWithTimeout proc arguments defaultTimeout
        match exitCode with
        | 0 -> exitCode
        | _ -> failwithf "Calling %s resulted in unexpected exitCode %i" proc exitCode 


    let execProcessAndReturnMessages proc arguments =
        execProcessWithTimeoutAndReturnMessages proc arguments defaultTimeout

    let nugetFile =
        let targetLocation = "build/tools/nuget/nuget.exe" 
        if (fileDoesNotExist targetLocation)
        then
            trace (sprintf "Nuget not found at %s. Downloading now" targetLocation)
            let url = "http://dist.nuget.org/win-x86-commandline/latest/nuget.exe" 
            Directory.CreateDirectory("build/tools/nuget") |> ignore
            use webClient = new WebClient()
            webClient.DownloadFile(url, targetLocation)
            trace "nuget downloaded"
        targetLocation 

    type BuildTooling(path) =
        member this.Path = path
        member this.Exec arguments = execProcess this.Path arguments

    let Nuget = new BuildTooling(nugetFile)
    let GitLink = new BuildTooling(Paths.Tool("gitlink/lib/net45/gitlink.exe"))
    let Node = new BuildTooling(Paths.Tool("Node.js/node.exe"))
    let Npm = new BuildTooling(Paths.Tool("Npm/node_modules/npm/cli.js"))
    let XUnit = new BuildTooling(Paths.Tool("xunit.runner.console/tools/xunit.console.exe"))
    let Fake = new BuildTooling("FAKE/tools/FAKE.exe")

    type DotNetRuntime = | Desktop | Core | Both

    type DotNetTooling(exe) =
       member this.Exec arguments =
            this.ExecWithTimeout arguments (TimeSpan.FromMinutes 30.)

        member this.ExecWithTimeout arguments timeout =
            let result = execProcessWithTimeout exe arguments timeout
            if result <> 0 then failwith (sprintf "Failed to run dotnet tooling for %s args: %A" exe arguments)

    let DotNet = new DotNetTooling("dotnet.exe")

    type MsBuildTooling() =

        member this.Build(target, framework:Projects.DotNetFrameworkIdentifier) =
            let solution = Paths.Source "AsciiDocNet.sln";
            let outputPath = Paths.BuildOutput |> Path.GetFullPath
            let setParams defaults =
                { defaults with
                    Verbosity = Some Minimal
                    Targets = [target]
                    Properties =
                        [
                            "OutputPathBaseDir", outputPath
                            "Optimize", "True"
                            "Configuration", "Release"
                            "TargetFrameworkVersion", framework.MSBuild
                            "DefineConstants", framework.DefineConstants
                        ]
                 }
        
            build setParams solution 

    let MsBuild = new MsBuildTooling()
