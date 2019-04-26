namespace Scripts

open System
open Fake.Core
open System.IO
open Commandline
open Versioning

module Tests =
    
    let private buildingOnAzurePipeline = Environment.environVarAsBool "TF_BUILD"

    let run args =
        Directory.CreateDirectory Paths.BuildOutput |> ignore
        let command =
            let command = ["test"; "."; "-c"; "RELEASE"]
            // TODO /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
            // Using coverlet.msbuild package
            // https://github.com/tonerdo/coverlet/issues/110
            // Bites us here as well a PR is up already but not merged will try again afterwards
            // https://github.com/tonerdo/coverlet/pull/329
            if buildingOnAzurePipeline then [ "--logger"; "trx"; "--collect"; "\"Code Coverage\""; "-v"; "m"] |> List.append command
            else command
            
        Tooling.DotNet.ExecInWithTimeout "src/AsciiDocNet.Tests" command (TimeSpan.FromMinutes 30.) 