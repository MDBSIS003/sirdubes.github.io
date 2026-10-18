$ErrorActionPreference = 'Stop'
Set-Location (Join-Path $PSScriptRoot '..')
dotnet restore FuturePath.Development.sln --locked-mode
if ($LASTEXITCODE -ne 0) { throw 'Restore failed' }
dotnet build FuturePath.Development.sln -c Release --no-restore
if ($LASTEXITCODE -ne 0) { throw 'Build failed' }
dotnet test FuturePath.Development.sln -c Release --no-build --logger trx --results-directory TestResults
if ($LASTEXITCODE -ne 0) { throw 'Tests failed' }
dotnet publish src/FuturePath.Desktop -c Release -r win-x64 --self-contained true -o artifacts/windows
if ($LASTEXITCODE -ne 0) { throw 'Publish failed' }
Copy-Item SETUP-DEMO.cmd artifacts/windows/SETUP-DEMO.cmd
Copy-Item -Recurse -Force Database artifacts/windows/Database
New-Item -ItemType Directory -Force artifacts/windows/scripts | Out-Null
Copy-Item scripts/setup-database.ps1 artifacts/windows/scripts/
Copy-Item README.md artifacts/windows/README.md
Compress-Archive -Path artifacts/windows/* -DestinationPath artifacts/FuturePath-Windows-x64.zip -Force
Write-Host 'Package: artifacts/FuturePath-Windows-x64.zip. Extract the whole folder and run FuturePath.exe.'
