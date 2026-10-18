# Run from the solution root on Windows. Requires SQL Server command-line tools (sqlcmd).
[CmdletBinding()]
param([switch]$SeedSampleData,[switch]$DemoStaff)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
if (-not (Get-Command sqlcmd -ErrorAction SilentlyContinue)) { throw 'Install SQL Server command-line tools (sqlcmd) or run the scripts in SSMS in the documented order.' }
$files = @('01_CreateDatabase.sql','02_CreateTables.sql','05_UpgradeIntegration.sql')
if ($SeedSampleData) { $files += '03_InsertSampleData.sql' }
if ($DemoStaff) { $files += '04_SetupDemoStaffAccounts.sql' }
$files += '06_NormaliseSampleRules.sql'
foreach ($file in $files) {
    Write-Host "Running $file"
    & sqlcmd -S '(localdb)\MSSQLLocalDB' -E -b -i (Join-Path $root "Database/$file")
    if ($LASTEXITCODE -ne 0) { throw "Database setup failed at $file. Later scripts were not run." }
}
Write-Host 'Setup finished. Existing records were retained. Demo seed is skipped on nonempty databases.'
