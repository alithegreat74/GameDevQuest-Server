using GamedevQuest.Helpers;
using GamedevQuest.Models.DTO;

namespace GamedevQuest.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string? Email {  get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Username { get; private set; }
        public string? Password { get; private set; }
        public int Level { get; private set; }
        public int Xp { get; private set; }
        public int LevelXp => XpSystem.GetLevelXp(Level);
        public List<int>? SolvedLessons { get; set; }
        public User(string? email, string? firstName, string? lastName, string? username, string? password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Level = 1;
            Xp = 0;
            SolvedLessons = new List<int>();
        }

        public void UpdateXp(int xp)
        {
            Xp += xp;
            while (Xp > LevelXp)
            {
                Xp-=LevelXp;
                Level++;
            }
        }
        public void UpdateInfo(string username, string firstName, string lastName)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
