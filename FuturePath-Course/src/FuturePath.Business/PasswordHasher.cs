using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

namespace FuturePath.Business
{
    public sealed class PasswordHasher
    {
        private const int Iterations = 210_000;
        public string Hash(string password)
        {
            Validate(password);
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
            return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string encoded)
        {
            try
            {
                string[] parts = encoded.Split(':');
                if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations) || iterations < 100_000 || iterations > 1_000_000)
                {
                    return false;
                }

                byte[] expected = Convert.FromBase64String(parts[2]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                if (expected.Length != 32 || salt.Length != 16)
                {
                    return false;
                }

                byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
                return CryptographicOperations.FixedTimeEquals(expected, actual);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public void Validate(string password)
        {
            bool letter = false;
            bool digit = false;
            foreach (char character in password)
            {
                if (char.IsLetter(character))
                {
                    letter = true;
                }

                if (char.IsDigit(character))
                {
                    digit = true;
                }
            }

            if (password.Length < 12 || password.Length > 128 || !letter || !digit)
            {
                throw new ValidationException("Use a password of 12–128 characters with a letter and a number.");
            }
        }
    }
}
