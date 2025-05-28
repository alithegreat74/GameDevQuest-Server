using GamedevQuest.Models;

namespace GamedevQuest.Helpers
{
    public interface IPasswordHelper
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string password);
    }
}
