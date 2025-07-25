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
    public class GetLessonController : ControllerBase
    {
        private readonly LessonService _lessonService;
        private readonly TestService _testService;
        public GetLessonController(LessonService lessonService, TestService testService)
        {
            _lessonService = lessonService;
            _testService = testService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDetailResponseDto>> GetAsync(int id)
        {
            (Lesson? lesson, string errorMessage) = await _lessonService.GetLesson(id);
            if (lesson == null)
                return BadRequest(errorMessage);
            (Test? test, errorMessage) = await _testService.FindTestForLesson(lesson);
            if(test == null)
                return BadRequest(errorMessage);
            var response = new LessonDetailResponseDto(lesson, test);
            return Ok(response);
        }
    }
}
