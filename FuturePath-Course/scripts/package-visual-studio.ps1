$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem
$projectRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$downloadDirectory = Join-Path $projectRoot 'downloads'
[System.IO.Directory]::CreateDirectory($downloadDirectory) | Out-Null
$destination = Join-Path $downloadDirectory 'FuturePath_Course_VisualStudio.zip'
$excludedDirectories = @('.git', '.github', '.vs', 'bin', 'obj', 'downloads', 'artifacts', 'TestResults')
$sourceFiles = Get-ChildItem -LiteralPath $projectRoot -File -Recurse -Force | Where-Object {
    $relative = $_.FullName.Substring($projectRoot.Length + 1)
    $parts = $relative -split '[\\/]'
    -not ($parts | Where-Object { $excludedDirectories -contains $_ }) -and $_.Extension -notin @('.user', '.db', '.db-wal', '.db-shm', '.mdf', '.ldf', '.bak')
}
if (Test-Path -LiteralPath $destination) { Remove-Item -LiteralPath $destination }
$archive = [System.IO.Compression.ZipFile]::Open($destination, [System.IO.Compression.ZipArchiveMode]::Create)
try {
    foreach ($file in $sourceFiles) {
        $relative = $file.FullName.Substring($projectRoot.Length + 1).Replace('\', '/')
        $entry = 'FuturePath_Course_VisualStudio/' + $relative
        [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $file.FullName, $entry, [System.IO.Compression.CompressionLevel]::Optimal) | Out-Null
    }
} finally { $archive.Dispose() }
Write-Host "Source project ZIP: $destination"
Write-Host 'Extract All, open FuturePath.sln, press F5. First run: Prepare demonstration database inside the app.'
