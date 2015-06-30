@echo off

IF EXIST packages\NUnit.Runners\tools\nunit-console.exe (
    REM do nothing
) ELSE (
    ".nuget\NuGet.exe" "Install" "roundhouse" "-OutputDirectory" "packages" "-ExcludeVersion"
    ".nuget\NuGet.exe" "Install" "NUnit.Runners" "-OutputDirectory" "packages" "-ExcludeVersion"
    ".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
)

"packages\FAKE\tools\Fake.exe" build.fsx %*
exit /b %errorlevel%