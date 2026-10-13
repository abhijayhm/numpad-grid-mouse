$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
Set-Location $root

dotnet publish src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj `
  -c Release `
  -r win-x64 `
  /p:PublishSingleFile=true `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  /p:SelfContained=true `
  -o artifacts

Write-Host "Published: $root\artifacts\NumpadGridMouse.exe"
