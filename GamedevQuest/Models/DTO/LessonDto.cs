namespace GamedevQuest.Models.DTO
{
    public class LessonResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int MinimumRequiredLevel { get; set; }

        public LessonResponseDto(Lesson lesson)
        {
            Id = lesson.Id;
            Title = lesson.LessonTitle;
            MinimumRequiredLevel = lesson.MinimumRequiredLevel;
        }
    }
}
