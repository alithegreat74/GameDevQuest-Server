using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
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
        public async Task<IActionResult> GetAsync(int id)
        {
            OperationResult<Lesson> findLessonResult = await _lessonService.GetLesson(id);
            if (findLessonResult.Result == null)
                return findLessonResult.ActionResultObject;
            OperationResult<Test> findTestResult= await _testService.FindTestForLesson(findLessonResult.Result);
            if (findTestResult.Result == null)
                return findTestResult.ActionResultObject;
            var response = new LessonDetailResponseDto(findLessonResult.Result, findTestResult.Result);
            return Ok(response);
        }
    }
}
