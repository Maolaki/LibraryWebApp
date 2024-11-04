using LibraryWebApp.AuthService.Domain.Interfaces;
using System.Security.Cryptography;

namespace LibraryWebApp.AuthService.Application.Entities
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; 
        private const int HashSize = 20; 
        private const int Iterations = 100; 

        public string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var hash = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hashBytes = hash.GetBytes(HashSize);

                var hashWithSalt = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashWithSalt, 0, SaltSize);
                Array.Copy(hashBytes, 0, hashWithSalt, SaltSize, HashSize);

                return Convert.ToBase64String(hashWithSalt);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashWithSalt = Convert.FromBase64String(hashedPassword);
            var salt = new byte[SaltSize];
            Array.Copy(hashWithSalt, 0, salt, 0, SaltSize);

            using (var hash = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hashBytes = hash.GetBytes(HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (hashWithSalt[i + SaltSize] != hashBytes[i])
                        return false;
                }
                return true;
            }
        }
    }
}
