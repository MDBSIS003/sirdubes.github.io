# Explicit local DBA recovery for an EXISTING account. Does not create/promote accounts.
[CmdletBinding()]
param([Parameter(Mandatory=$true)][string]$Username,[switch]$Enable)
$ErrorActionPreference = 'Stop'
$secure = Read-Host 'New password (10-128 characters)' -AsSecureString
$confirm = Read-Host 'Confirm password' -AsSecureString
$plain = [System.Net.NetworkCredential]::new('', $secure).Password
$confirmation = [System.Net.NetworkCredential]::new('', $confirm).Password
if ($plain -cne $confirmation -or $plain.Length -lt 10 -or $plain.Length -gt 128) { throw 'Passwords must match and contain 10-128 characters.' }
$salt = New-Object byte[] 16
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rng.GetBytes($salt)
$kdf = [System.Security.Cryptography.Rfc2898DeriveBytes]::new($plain,$salt,600000,[System.Security.Cryptography.HashAlgorithmName]::SHA256)
$hash = 'PBKDF2-SHA256$600000$' + [Convert]::ToBase64String($salt) + '$' + [Convert]::ToBase64String($kdf.GetBytes(32))
$plain = $null; $confirmation = $null; $kdf.Dispose(); $rng.Dispose()
$connection = [System.Data.SqlClient.SqlConnection]::new('Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FuturePathAdvisingSystem;Integrated Security=True;TrustServerCertificate=True')
try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = 'UPDATE dbo.UserAccount SET [Password]=@hash,IsActive=CASE WHEN @enable=1 THEN 1 ELSE IsActive END WHERE Username=@username;'
    [void]$command.Parameters.AddWithValue('@hash',$hash)
    [void]$command.Parameters.AddWithValue('@enable',[bool]$Enable)
    [void]$command.Parameters.AddWithValue('@username',$Username)
    if ($command.ExecuteNonQuery() -ne 1) { throw 'Existing account not found; no account was created.' }
    Write-Host 'Password reset. The account role and existing records were retained.'
} finally { $connection.Dispose(); $secure.Dispose(); $confirm.Dispose() }
