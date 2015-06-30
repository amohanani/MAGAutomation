module Mag

open Fake
open Fake.NDepend
open Fake.DotCover
open Fake.NUnitSequential
open Fake.FileSystem
open Fake.FileSystemHelper
open Fake.NuGetHelper
open Fake.RoundhouseHelper
open Fake.Git.Information
open System.IO
open System.Text.RegularExpressions

type DbInfo = {
  name:string;
  connectionString:string;
  rhFolder:string;
}

type NuGetInfo = {
  name:string;
  version:string;
  nuspec:string;
  dependencies:NugetDependencies;
  files:string->list<string*string option*string option>;
  copyFiles:string->unit;
}

type BuildInfo = {
  product:string;
  description:string;
  mode:string;
  output:string;
  artifacts:string;
  testDlls:FileIncludes;
  integrationDlls:FileIncludes;
  solutionFiles:FileIncludes;
  ndependFiles:FileIncludes;
  ndepend:string;
  ndependProj:string;
  dotCover:string;
  roundhouse:string;
  packages:string;
  dbs:DbInfo array;
  nugets:NuGetInfo array;
  useCoverage:bool;
}


//release or debug
let buildMode = getBuildParamOrDefault "mode" "Release"

let buildDefaults = {
  product="MAG";
  description="MAG Product";
  mode=buildMode;
  output= currentDirectory @@ "build_output";
  artifacts="./build_artifacts";
  testDlls= !! (sprintf "./**/bin/%s/*.Tests.dll" buildMode);
  integrationDlls= !! (sprintf "./**/bin/%s/*.IntegrationTests.dll" buildMode);
  solutionFiles= !! ("./*.sln");
  ndependFiles= !! ("./*.ndproj");
  ndepend= (sprintf "%s/NDepend.Console.exe" (environVarOrDefault "NDEPEND" "C:/Program Files/NDepend"));
  ndependProj = "./CommonServices.ndproj";
  dotCover= (sprintf "%s/dotCover.exe" (environVarOrDefault "DOTCOVER" ((environVar "LOCALAPPDATA") + "/JetBrains/Installations/dotCover01")));
  roundhouse= "./packages/roundhouse/bin/rh.exe";
  packages="packages"; //nuget package folder - DO NOT EDIT
  dbs=[||];
  nugets=[||];
  useCoverage=true;
}

let branch = (fun _ ->
  let b = (environVarOrDefault "TEAMCITY_BUILD_BRANCH" "")
  let m = Regex.Match(b, @"(\d+)\/(head|merge)")
  if (m.Success) then m.Groups.[1].Value else "master");

let nugetVersion = (fun baseVersion ->
  let buildNumber = (environVarOrDefault "BUILD_NUMBER" "0")
  let branchName = (branch ".")
  let label = if branchName="master" then "" else "-pr" + branchName
  (baseVersion + "." + buildNumber + label));

let runDotCover buildParams =
  let snapshotFile = buildParams.output @@ "NUnitDotCover.snapshot";
  let outputFile = buildParams.output @@ "dotCoverReport.xml"
  let nunitFile = buildParams.output @@ "TestResults.xml"

  buildParams.testDlls
    |> DotCoverNUnit
      (fun dotCoverOptions -> { dotCoverOptions with
        Output = snapshotFile;
        ToolPath = buildParams.dotCover
        })
      (fun nunitOptions -> { nunitOptions with
        DisableShadowCopy = true;
        ToolPath = "packages/NUnit.Runners/tools";
        Framework = "net-4.5";
        OutputFile = nunitFile
        })

  DotCoverReport (fun p -> { p with
    ToolPath = buildParams.dotCover;
    Source = snapshotFile;
    Output = outputFile;
    ReportType = DotCoverReportType.Xml;
    })

  trace (sprintf "##teamcity[importData type='nunit' path='%s']" nunitFile)
  trace (sprintf "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='%s']" snapshotFile)

let runNUnit buildParams =
  let nunitFile = buildParams.output @@ "TestResults.xml"

  buildParams.testDlls
    |> NUnit (fun p -> { p with
      DisableShadowCopy = true;
      ToolPath = "packages/NUnit.Runners/tools";
      Framework = "net-4.5";
      OutputFile = nunitFile
      })
  trace (sprintf "##teamcity[importData type='nunit' path='%s']" nunitFile)

let Build setParams =
    let settings = setParams buildDefaults

    Target "Clean" (fun _ ->
      [settings.output;settings.artifacts]
        |> List.iter FileHelper.CleanDir
    )


    Target "Test-Integration" (fun _ ->
      trace "Test-Integration"
    )

    Target "Run-Tests" (fun _ ->
      if settings.useCoverage then (runDotCover settings)
        else (runNUnit settings)
    )

    Target "Database-Migrate" (fun _ ->

      settings.dbs
        |> Array.iter (fun db  ->
          Roundhouse (fun rhParams -> { rhParams with
            SqlFilesDirectory = db.rhFolder;
            ConnectionString = db.connectionString;
            WarnOnOneTimeScriptChanges = true ;
            ToolPath = settings.roundhouse
            }))
    )


    Target "Build" (fun _ ->
      MSBuild null "Build" [
          "Configuration", settings.mode
          "RunOctoPack", "True"
          "OctoPackPackageVersion", (nugetVersion "1.0")
      ] settings.solutionFiles
        |> Log "AppBuild-Output"
    )



    Target "Analyze-NDepend" (fun _ ->
      NDepend (fun p -> { p with
        ToolPath = settings.ndepend;
        ProjectFile = Path.GetFullPath(settings.ndependProj);
        CoverageFiles = [(settings.output @@ "NUnitDotCover.snapshot")]
      })
    )

    Target "Create-Nuget" (fun _ ->
      settings.nugets
        |> Array.iter (fun nug ->
          let dir = settings.artifacts @@ nug.name;
          CleanDir dir;
          (nug.copyFiles dir);
          NuGet (fun p -> { p with
            Authors = ["mag-dev"];
            Dependencies = nug.dependencies;
            Files = (nug.files dir);
            Project = settings.product;
            Description = settings.description;
            OutputPath = settings.output;
            Summary = settings.description;
            Version = (nugetVersion nug.version);
            WorkingDir = ".";
            })
            nug.nuspec
        )
    )

    Target "Heavy" (fun _ ->
      trace "Heavy"
    )


    Target "Database-Reboot" (fun _ ->
      trace "Database-Reboot"
    )

    Target "Light" (fun _ ->
      trace "Light"
    )


    "Clean"
      ==> "Build"
      ==> "Run-Tests"
      ==> "Create-Nuget"
      ==> "Light"

    // start build
    RunTargetOrDefault "Light"
