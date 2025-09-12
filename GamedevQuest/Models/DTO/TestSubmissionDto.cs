using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class TestSubmissionDto
    {
        [Required(ErrorMessage = "Lesson Id is required")]
        public int LessonId { get; set; }
        [Required(ErrorMessage ="Submissions is required")]
        public List<TestSubmission> Submissions { get; set; }
        public class TestSubmission
        {
            public int TestId { get; set; }
            public string Answer { get; set; }
        }
    }
    public class TestSubmissionResponseDto
    {
        public int Xp { get; private set; }
        public int LevelUpXp { get; private set; }
        public TestSubmissionResponseDto(User user)
        {
            Xp = user.Xp;
            LevelUpXp = user.LevelXp;
        }
    }
}
