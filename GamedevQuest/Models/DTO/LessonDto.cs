namespace GamedevQuest.Models.DTO
{
    public class LessonResponseDto
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int MinimumRequiredLevel { get; private set; }
        public string ShortDescription { get; private set; }
        public int Xp { get; private set; }
        public string LessonImageUrl { get; private set; }
        public int UserStars { get; private set; }
        public LessonResponseDto(Lesson lesson, User user)
        {
            Id = lesson.Id;
            Title = lesson.LessonTitle;
            MinimumRequiredLevel = lesson.MinimumRequiredLevel;
            Xp = lesson.Xp;
            ShortDescription = lesson.LessonDescription.Length > 30 ? lesson.LessonDescription.Substring(0, 30) : lesson.LessonDescription;
            LessonImageUrl = lesson.LessonImageUrl;
            int doneLessons = user.SolvedTests != null ? lesson.RelatedTests.Intersect(user.SolvedTests).Count() : 0;
            UserStars = lesson.RelatedTests.Count > 0 ? (doneLessons * 3) / lesson.RelatedTests.Count() : 0;
        }
    }
    public class LessonDetailResponseDto
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int MinimumRequiredLevel { get; private set; }
        public List<TestSummaryDto> Tests { get; private set; }
        public LessonDetailResponseDto(Lesson lesson,  List<TestSummaryDto> tests)
        {
            Id = lesson.Id;
            Title = lesson.LessonTitle;
            Description = lesson.LessonDescription;
            MinimumRequiredLevel = lesson.MinimumRequiredLevel;
            Tests = tests;
        }
    }
}
