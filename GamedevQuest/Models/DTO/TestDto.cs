using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class TestRequestDto
    {
        [Required(ErrorMessage = "Lesson id is required")]
        public int LessonId { get; private set; }
        [Required(ErrorMessage = "Test id is required")]
        public int TestId { get; private set; }
        [Required(ErrorMessage = "Answer is required")]
        public string? Answer {  get; private set; }

        public TestRequestDto(int lessonId, int testId, string? answer) 
        {
            this.LessonId = lessonId;
            this.TestId = testId;
            this.Answer = answer;
        }
    }
    public class TestResponseDto
    {
        public int Xp { get; private set; }
        public int LevelUpXp { get; private set; }
        public TestResponseDto(User user)
        {
            Xp = user.Xp;
            LevelUpXp = user.LevelXp;
        }
    }
}
