using System.Security.Cryptography;
using System.Text;

namespace GamedevQuest.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        
        //TODO: use a better hashing algorithm for passwords (like PBKDF2)
        //Used SHA256 to get an up and running hasher fast
        public string HashPassword(string password)
        {
            try
            {   
                using (SHA256 hashAlgorithm = SHA256.Create())
                {
                    byte[] hashedPasswordBytes = hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(password));
                    string hashedPassword = Encoding.ASCII.GetString(hashedPasswordBytes);
                    return hashedPassword;
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine($"There was a problem with hashing password ex: {e}");
                return "";
            }
        }
        public bool VerifyPassword(string hashedPassword, string password)
        {
            try
            {
                using (SHA256 hashAlgorithm = SHA256.Create())
                {
                    byte[] hashedPasswordBytes = hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(password));
                    string hashedComparePassword = Encoding.ASCII.GetString(hashedPasswordBytes);
                    return hashedComparePassword == hashedPassword;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was a problem with the password verification : {e}");
                return false;
            }
        }
    }
}
