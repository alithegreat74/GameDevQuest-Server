namespace GamedevQuest.Helpers
{
    public class XpSystem
    {
        private static readonly int XpRatioBase = 100;
        private static readonly float XpExponent = 1.5f;
        public static int GetLevelXp(int level)
        {
            if (level < 1)
                throw new ArgumentOutOfRangeException(nameof(level), "Level must be 1 or greater");
            return (int)(XpRatioBase * Math.Pow(XpExponent, level - 1));
        }
    }
}