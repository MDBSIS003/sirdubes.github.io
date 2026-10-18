@echo off
setlocal
where dotnet >nul 2>&1
if errorlevel 1 (
    echo Install the .NET 10 SDK and the .NET desktop development workload first.
    pause
    exit /b 1
)
dotnet build "%~dp0src\FuturePath.Desktop\FuturePath.Desktop.csproj" -c Release --nologo
if errorlevel 1 (
    pause
    exit /b 1
)
start "" "%~dp0src\FuturePath.Desktop\bin\Release\net10.0-windows\FuturePath.exe" %*
exit /b 0
