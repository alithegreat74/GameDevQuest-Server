using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetTestsController:ControllerBase
    {
        private readonly LessonService _lessonService;
        private readonly TestService _testService;
        public GetTestsController(LessonService lessonService, TestService testService)
        {
            _lessonService = lessonService;
            _testService = testService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAsync(int lessonId)
        {
            OperationResult<Lesson> findLesson = await _lessonService.GetLesson(lessonId);
            if (findLesson.Result == null)
                return findLesson.ActionResultObject;
            OperationResult<List<TestSummaryDto>> findTestResult = await _testService.FindTestsForLesson(findLesson.Result);
            if (findTestResult.Result == null)
                return findTestResult.ActionResultObject;
            return Ok(findTestResult.Result);
        }
    }
}
