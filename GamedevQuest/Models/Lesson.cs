namespace GamedevQuest.Models
{
    public class Lesson
    {
        public int Id { get; private set; }
        public string LessonTitle { get; private set; }
        public string LessonDescription { get; private set; }
        public int MinimumRequiredLevel { get; private set; }
        public int Xp { get; private set; }
        public List<int> RelatedTests { get; private set; }
        public string LessonImageUrl { get; private set; }
        public Lesson(int id, string lessonTitle, string lessonDescription, int minimumRequiredLevel, int xp, List<int> relatedTests, string lessonImageUrl, List<string> tags)
        {
            Id = id;
            LessonTitle = lessonTitle;
            LessonDescription = lessonDescription;
            MinimumRequiredLevel = minimumRequiredLevel;
            Xp = xp;
            RelatedTests = relatedTests;
            LessonImageUrl = lessonImageUrl;
            Tags = tags;
        }
    }
}
