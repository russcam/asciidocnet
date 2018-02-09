#I @"../../packages/build/FAKE/tools"
#r @"FakeLib.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"
#load @"Tooling.fsx"
#load @"Versioning.fsx"

open System
open System.Text
open Fake 
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open Paths
open Projects
open Versioning
open Tooling

type Release() = 
    static member NugetPack() =
        let currentVersion = Versioning.CurrentVersion.ToString()
        Projects.DotNetProject.AllPublishable
        |> Seq.iter(fun p ->
            CreateDir Paths.NugetOutput

            let name = p.Name;
            let nuspec = (sprintf @"build\%s.nuspec" name)
            let nugetOutFile =  Paths.Output(sprintf "%s.%s.nupkg" name currentVersion)          
            let year = sprintf "%i" DateTime.UtcNow.Year
            
            let properties =
                let addKeyValue (e:Expr<string>) (builder:StringBuilder) =
                    let (value,key) = 
                        match e with
                        | PropertyGet (eo, pi, li) -> (pi.Name, (pi.GetValue(e) |> string))
                        | ValueWithName (obj,ty,nm) -> ((obj |> string), nm)
                        | _ -> failwith (sprintf "%A is not a let-bound value. %A" e (e.GetType()))                     
                    if (isNotNullOrEmpty value) then builder.AppendFormat("{0}=\"{1}\";", key, value)
                    else builder
                new StringBuilder()
                |> addKeyValue <@year@>
                |> toText

            Tooling.Nuget.Exec [ "pack"; nuspec; 
                                 "-version"; currentVersion; 
                                 "-outputdirectory"; Paths.BuildOutput; 
                                 "-properties"; properties;
                               ] |> ignore
            traceFAKE "%s" Paths.BuildOutput
            MoveFile Paths.NugetOutput nugetOutFile
        )