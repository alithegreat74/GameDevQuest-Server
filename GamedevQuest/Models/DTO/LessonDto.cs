namespace GamedevQuest.Models.DTO
{
    public class LessonResponseDto
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int MinimumRequiredLevel { get; private set; }
        public LessonResponseDto(Lesson lesson)
        {
            Id = lesson.Id;
            Title = lesson.LessonTitle;
            MinimumRequiredLevel = lesson.MinimumRequiredLevel;
        }
    }
    public class LessonDetailResponseDto
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int MinimumRequiredLevel { get; private set; }
        public Test Test { get; private set; }
        public LessonDetailResponseDto(Lesson lesson,  Test test)
        {
            Id = lesson.Id;
            Title = lesson.LessonTitle;
            Description = lesson.LessonDescription;
            MinimumRequiredLevel = lesson.MinimumRequiredLevel;
            Test = test;
        }
    }
}
