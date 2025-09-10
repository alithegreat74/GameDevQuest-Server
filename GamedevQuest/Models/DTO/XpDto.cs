namespace GamedevQuest.Models.DTO
{
    public class XpDto
    {
        public int Xp {  get; set; }
        public int LevelUpXp {  get; set; }
        public XpDto(User user)
        {
            Xp = user.Xp;
            LevelUpXp = user.LevelXp;
        }
    }
}
