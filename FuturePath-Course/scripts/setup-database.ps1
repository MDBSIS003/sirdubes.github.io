# Explicit SQL Server LocalDB setup. Runs with the current Windows user's integrated identity.
[CmdletBinding()]
param([switch]$SeedDemo,[string]$DatabaseName='FuturePathStudentAdvisingSystem')
$ErrorActionPreference = 'Stop'
if ($DatabaseName -notmatch '^[A-Za-z][A-Za-z0-9_]{0,127}$') { throw 'Use a database name, not a file path.' }
if (@('master','model','msdb','tempdb') -contains $DatabaseName.ToLowerInvariant()) { throw 'Choose a separate application database, not a SQL Server system database.' }
$root = Split-Path $PSScriptRoot -Parent
$connection = [System.Data.SqlClient.SqlConnection]::new('Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15')
try {
    $connection.Open()
    $files = @('01_CreateDatabase.sql','02_CreateTables.sql')
    if ($SeedDemo) { $files += '03_SeedData.sql' }
    $files += '04_VerifyDatabase.sql'
    foreach ($file in $files) {
        Write-Host "Running $file on $DatabaseName"
        $text = [System.IO.File]::ReadAllText((Join-Path $root "Database/$file"))
        $text = $text.Replace('FuturePathStudentAdvisingSystem',$DatabaseName)
        foreach ($batch in [regex]::Split($text,'(?im)^\s*GO\s*$')) {
            if ([string]::IsNullOrWhiteSpace($batch)) { continue }
            $command = $connection.CreateCommand()
            try { $command.CommandText=$batch; $command.CommandTimeout=120; [void]$command.ExecuteNonQuery() }
            finally { $command.Dispose() }
        }
    }
    Write-Host 'Setup finished. Existing databases and records were retained.'
    if (-not $SeedDemo) { Write-Host 'No sample data/accounts were requested. Use -SeedDemo only for a demonstration database.' }
} finally { $connection.Dispose() }
