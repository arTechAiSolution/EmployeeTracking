using System;
using System.Security.Cryptography;

namespace PharmacyField.Infrastructure.Utils
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public static string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, System.Security.Cryptography.HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);

            var result = new byte[1 + SaltSize + HashSize];
            result[0] = 0; // version
            Buffer.BlockCopy(salt, 0, result, 1, SaltSize);
            Buffer.BlockCopy(hash, 0, result, 1 + SaltSize, HashSize);
            return Convert.ToBase64String(result);
        }

        public static bool VerifyPassword(string stored, string password)
        {
            try
            {
                var data = Convert.FromBase64String(stored);
                if (data.Length != 1 + SaltSize + HashSize || data[0] != 0) return false;
                var salt = new byte[SaltSize];
                var hash = new byte[HashSize];
                Buffer.BlockCopy(data, 1, salt, 0, SaltSize);
                Buffer.BlockCopy(data, 1 + SaltSize, hash, 0, HashSize);

                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, System.Security.Cryptography.HashAlgorithmName.SHA256);
                var computed = pbkdf2.GetBytes(HashSize);
                return CryptographicOperations.FixedTimeEquals(computed, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}
