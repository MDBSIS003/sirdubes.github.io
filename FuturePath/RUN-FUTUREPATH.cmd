@echo off
setlocal
REM Launch the Windows Forms application, never the automated test project.
where dotnet >nul 2>&1
if errorlevel 1 (
    echo Install the .NET 10 SDK and Windows desktop development tools first.
    echo See README.md for installation and LocalDB setup.
    pause
    exit /b 1
)
echo Building the Future Path Windows application...
dotnet build "%~dp0FuturePathStudentAdvisingSystem\FuturePathStudentAdvisingSystem.csproj" --nologo
if errorlevel 1 (
    echo The application could not be built. Review the errors above.
    pause
    exit /b 1
)
start "" "%~dp0FuturePathStudentAdvisingSystem\bin\Debug\net10.0-windows\FuturePathStudentAdvisingSystem.exe"
if errorlevel 1 (
    echo The application could not be started. See README.md.
    pause
    exit /b 1
)
exit /b 0
