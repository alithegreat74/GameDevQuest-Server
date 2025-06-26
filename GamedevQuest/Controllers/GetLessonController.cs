using GamedevQuest.Context;
using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetLessonController:ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        public GetLessonController(GameDevQuestDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDetailResponseDto>> GetAsync(int id)
        {
            var lessonRepository = new LessonRepository(_context.Lessons);
            var testRepository = new TestRepository(_context.Tests);
            var testService = new TestService(testRepository);
            var lessonService = new LessonService(lessonRepository);
            (Lesson? lesson, string errorMessage) = await lessonService.GetLesson(id);
            if (lesson == null)
                return BadRequest(errorMessage);
            (Test? test, errorMessage) = await testService.FindTestForLesson(lesson);
            if(test == null)
                return BadRequest(errorMessage);
            var response = new LessonDetailResponseDto(lesson, test);
            return Ok(response);
        }
    }
}
