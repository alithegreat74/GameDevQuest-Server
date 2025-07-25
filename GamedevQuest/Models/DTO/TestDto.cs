using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class TestRequestDto
    {
        [Required(ErrorMessage = "Lesson id is required")]
        public int LessonId { get; set; }
        [Required(ErrorMessage = "Test id is required")]
        public int TestId { get; set; }
        [Required(ErrorMessage = "Answer is required")]
        public string Answer {  get; set; }
    }
}
