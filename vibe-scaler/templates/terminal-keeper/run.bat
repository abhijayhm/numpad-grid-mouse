@echo off
REM Runs one named target. Usage: run.bat backend
if "%~1"=="" exit /b 1
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0run.ps1" %1
