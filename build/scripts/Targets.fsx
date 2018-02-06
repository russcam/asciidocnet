#r "../../packages/build/FAKE/tools/FakeLib.dll"

#load @"Projects.fsx"
#load @"Paths.fsx"
#load @"Tooling.fsx"
#load @"Versioning.fsx"
#load @"Releasing.fsx"
#load @"Building.fsx"
#load @"Testing.fsx"

open Fake

open Building
open Testing
open Versioning
open Releasing

Target "Build" <| fun _ -> traceHeader "STARTING BUILD"

Target "Clean" Build.Clean

Target "Restore" Build.Restore

Target "BuildApp" Build.Compile

Target "Test" Tests.RunUnitTests

Target "Version" <| fun _ -> tracefn "Current Version: %A" Versioning.CurrentVersion

Target "Release" Release.NugetPack

"Clean" 
  =?> ("Version", hasBuildParam "version")
  ==> "Restore"
  ==> "BuildApp"
  =?> ("Test", (not ((getBuildParam "skiptests") = "1")))
  ==> "Build"

"Version"
  ==> "Release"

"Build"
  ==> "Release"

RunTargetOrDefault "Build"

