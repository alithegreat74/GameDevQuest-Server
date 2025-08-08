using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace GamedevQuest.Services
{
    public class LessonService
    {
        private readonly LessonRepository _repository;
       
        public LessonService(LessonRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<Lesson>> GetLesson(int id)
        {
            Lesson? find = await _repository.GetLesson(id);
            if (find == null)
                return new OperationResult<Lesson>(new NotFoundObjectResult("No lesson found with this id"));

            return new OperationResult<Lesson>(find);
        }
        public List<LessonResponseDto> GetAllLessons()
        {
            List<Lesson> lessons =  _repository.GetAllLessons();
            return lessons.Select(lesson => new LessonResponseDto(lesson)).ToList();
        }
    }
}
