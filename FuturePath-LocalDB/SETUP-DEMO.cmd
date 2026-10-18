@echo off
setlocal
REM Explicitly create/prepare the local demonstration database. Existing data is preserved.
where sqllocaldb >nul 2>&1
if errorlevel 1 (
    echo Install SQL Server Express LocalDB using the Visual Studio Installer first.
    pause
    exit /b 1
)
sqllocaldb info MSSQLLocalDB >nul 2>&1
if errorlevel 1 sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
if errorlevel 1 (
    pause
    exit /b 1
)
powershell.exe -NoProfile -File "%~dp0scripts\setup-database.ps1" -SeedDemo
if errorlevel 1 (
    echo Setup failed. See docs\Database-Setup.md. If PowerShell policy blocks this script, run the SQL files in SSMS instead.
    pause
    exit /b 1
)
echo Ready. Next open FuturePath-App.sln or run RUN-FUTUREPATH.cmd.
pause
