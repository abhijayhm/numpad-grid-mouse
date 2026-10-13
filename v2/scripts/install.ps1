# Rebuild, publish, and start Numpad Grid Mouse v2
$ErrorActionPreference = "Stop"
$v2 = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
Set-Location $v2

Stop-Process -Name NumpadGridMouse -Force -ErrorAction SilentlyContinue
Start-Sleep -Milliseconds 800

dotnet test src\NumpadGridMouse.Win.Tests\NumpadGridMouse.Win.Tests.csproj -c Release
dotnet publish src\NumpadGridMouse.Win\NumpadGridMouse.Win.csproj `
  -c Release -r win-x64 `
  /p:PublishSingleFile=true `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  /p:SelfContained=true `
  -o artifacts

$exe = Join-Path $v2 "artifacts\NumpadGridMouse.exe"
Write-Host "Installed: $exe"
Start-Process -FilePath $exe -WorkingDirectory (Split-Path $exe)
