# Stop Numpad Grid Mouse v2 and remove autostart + published binary
$ErrorActionPreference = "Stop"
$v2 = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

Write-Host "Stopping Numpad Grid Mouse..."
Stop-Process -Name NumpadGridMouse -Force -ErrorAction SilentlyContinue
Start-Sleep -Milliseconds 500

$runKey = "HKCU:\Software\Microsoft\Windows\CurrentVersion\Run"
if (Get-ItemProperty -Path $runKey -Name NumpadGridMouse -ErrorAction SilentlyContinue) {
    Remove-ItemProperty -Path $runKey -Name NumpadGridMouse -Force
    Write-Host "Removed Run-at-logon registry entry."
} else {
    Write-Host "No Run-at-logon entry."
}

$artifacts = Join-Path $v2 "artifacts"
if (Test-Path $artifacts) {
    Remove-Item -Path $artifacts -Recurse -Force
    Write-Host "Removed: $artifacts"
}

Set-Location $v2
dotnet clean NumpadGridMouse.sln -c Release --nologo -v q
Write-Host "Cleaned build outputs (bin/obj)."

Write-Host "Done. Source code in v2\src is untouched. Config remains at %APPDATA%\NumpadGridMouse (delete manually if wanted)."
