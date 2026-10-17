using System.Security.Cryptography;

namespace FuturePathStudentAdvisingSystem.Security;

/// <summary>Versioned PBKDF2-SHA256 hashes. No plaintext compatibility fallback.</summary>
public static class PasswordHasher
{
    public const int Iterations = 600_000;
    public static string Hash(string password)
    {
        Validate(password);
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
        return $"PBKDF2-SHA256${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }
    public static bool Verify(string password, string encoded)
    {
        if (password.Length > 128) return false;
        try
        {
            var parts = encoded.Split('$');
            if (parts.Length != 4 || parts[0] != "PBKDF2-SHA256" ||
                !int.TryParse(parts[1], out int iterations) || iterations < 100_000 || iterations > 2_000_000) return false;
            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);
            if (salt.Length != 16 || expected.Length != 32) return false;
            var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch (FormatException) { return false; }
    }
    public static void Validate(string password)
    {
        if (password.Length is < 10 or > 128)
            throw new ArgumentException("Password must contain 10 to 128 characters.");
    }
}
