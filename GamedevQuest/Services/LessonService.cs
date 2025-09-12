using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Services
{
    public class LessonService
    {
        private readonly LessonRepository _repository;
        private readonly UserService _userService;
       
        public LessonService(LessonRepository repository, UserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public async Task<OperationResult<Lesson>> GetLesson(int id)
        {
            Lesson? find = await _repository.GetLesson(id);
            if (find == null)
                return new OperationResult<Lesson>(new NotFoundObjectResult($"No lesson found with id {id}"));

            return new OperationResult<Lesson>(find);
        }
        public List<LessonResponseDto> GetAllLessons(User user)
        {
            List<Lesson> lessons =  _repository.GetAllLessons();

            return lessons.Select(lesson => new LessonResponseDto(lesson, user)).ToList();
        }
        public int CalculateUserXpGain(Lesson lesson, int correctTestNumber)
        {
            return (lesson.Xp * correctTestNumber) / lesson.RelatedTests.Count;
        }
    }
}
