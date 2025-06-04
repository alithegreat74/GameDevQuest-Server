namespace GamedevQuest.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string LessonTitle { get; set; }
        public string LessonDescription { get; set; }
        public int MinimumRequiredLevel {  get; set; }
    }
}
