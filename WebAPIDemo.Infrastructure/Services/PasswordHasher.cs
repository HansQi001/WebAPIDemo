using System.Security.Cryptography;
using WebAPIDemo.Application.Common.Interfaces;

namespace WebAPIDemo.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        public string Hash(string password)
        {
            // Generate a salt
            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            // Derive a key from the password
            var key = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                Algorithm,
                KeySize);

            // Return as: {iterations}.{salt}.{key}
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            var parts = hashedPassword.Split('.', 3);
            if (parts.Length != 3)
                return false;

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
                providedPassword,
                salt,
                iterations,
                Algorithm,
                key.Length);

            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }
    }

}
