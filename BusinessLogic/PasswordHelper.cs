using System;
using System.Security.Cryptography;

namespace EmployeeManagementSystem.BusinessLogic
{
    public class PasswordHelper
    {
        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Generate a random salt
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            // Hash the password with the salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // Convert the stored salt back to bytes
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            // Hash the input password with the stored salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                string hash = Convert.ToBase64String(hashBytes);
                // Compare the computed hash with the stored hash
                return hash == storedHash;
            }
        }
    }
}