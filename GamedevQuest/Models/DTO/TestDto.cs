using System.ComponentModel.DataAnnotations;
using static GamedevQuest.Models.Test;

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
    public class CheckTestAnswerDto
    {
        public int TestId { get; set; }
        public string? Answer { get; set; }
    }
    public class TestSummaryDto
    {
        public int Id { get; set; }
        public string TestDescription { get; private set; }
        public TestType Type { get; private set; }
        public string? Payload { get; private set; }
        public TestSummaryDto(Test test)
        {
            this.Id = test.Id;
            this.TestDescription = test.TestDescription;
            this.Type = test.Type;
            this.Payload = test.Payload;
        }
    }
}
