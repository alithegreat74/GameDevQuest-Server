namespace GamedevQuest.Helpers
{
    public class XpSystem
    {
        private static readonly int XpRatioBase = 100;
        private static readonly float XpExponent = 1.5f;
        public static int GetLevelXp(int level)
        {
            return (int)(XpRatioBase * Math.Pow(XpExponent, level - 1));
        }

    }
}
