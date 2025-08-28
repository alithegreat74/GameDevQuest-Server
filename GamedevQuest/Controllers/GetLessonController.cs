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
            OperationResult<List<TestSummaryDto>> findTestResult= await _testService.FindTestsForLesson(findLessonResult.Result);
            var tests = new List<TestSummaryDto>();
            if (findTestResult.Result != null)
                tests=findTestResult.Result;
            else
                tests=new List<TestSummaryDto>();
            var response = new LessonDetailResponseDto(findLessonResult.Result, tests);
            return Ok(response);
        }
    }
}
