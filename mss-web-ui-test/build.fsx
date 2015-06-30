module Build

#r @"packages/FAKE/tools/FakeLib.dll"
#load "mag.fsx"

open Fake
open Mag

Mag.Build (fun b -> { b with
    product="mss-web-test";
    description="MSS Web Tests";
    useCoverage=false;
  })
