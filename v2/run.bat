@echo off
cd /d "%~dp0"
taskkill /IM NumpadGridMouse.exe /F >nul 2>&1
dotnet run --project src\NumpadGridMouse.Win\NumpadGridMouse.Win.csproj -c Release
