using System.Security.Cryptography;
using System.Text;

namespace GamedevQuest.Helpers
{
    public class PasswordHelper
    {
        private const int _saltSize = 16;        
        private const int _keySize = 32;        
        private const int _iterations = 600_000;        
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(_keySize);
            return $"{_iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }
        public bool VerifyPassword(string hashedPassword, string password)
        {
            var hashParts = hashedPassword.Split('.');
            if (hashParts.Length != 3)
                throw new FormatException("Wrong hash format");

            int iterations = int.Parse(hashParts[0]);
            byte[] salt = Convert.FromBase64String(hashParts[1]);
            byte[] storedKey = Convert.FromBase64String(hashParts[2]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(_keySize);

            return CryptographicOperations.FixedTimeEquals(storedKey, key); 
        }
    }
}
