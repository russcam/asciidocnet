﻿#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"
#r @"System.IO.Compression.FileSystem.dll"

#load "Projects.fsx"

open System
open System.IO
open System.Diagnostics
open System.Net

open Fake

open Projects

[<AutoOpen>]
module Paths =
    open Projects

    let Repository = "https://github.com/russcam/asciidocnet"

    let BuildFolder = "build"
    let BuildOutput = sprintf "%s/output" BuildFolder

    let ProjectOutputFolder (project:DotNetProject) (framework:DotNetFramework) = 
        sprintf "%s/%s/%s" BuildOutput framework.Identifier.MSBuild project.Name

    let Tool tool = sprintf "packages/build/%s" tool
    let CheckedInToolsFolder = "build/Tools"
    let KeysFolder = sprintf "%s/keys" BuildFolder
    let NugetOutput = sprintf "%s/_packages" BuildOutput
    let SourceFolder = "src"
    
    let CheckedInTool(tool) = sprintf "%s/%s" CheckedInToolsFolder tool
    let Keys(keyFile) = sprintf "%s/%s" KeysFolder keyFile
    let Output(folder) = sprintf "%s/%s" BuildOutput folder
    let Source(folder) = sprintf "%s/%s" SourceFolder folder
    let Build(folder) = sprintf "%s/%s" BuildFolder folder

    let BinFolder(folder) = 
        let f = replace @"\" "/" folder
        sprintf "%s/%s/bin/Release" SourceFolder f

    let ProjectFile(projectName) =
        Source(sprintf "%s/%s.csproj" projectName projectName)