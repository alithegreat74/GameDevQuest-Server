using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/test/[Controller]")]
    public class SubmitTestController : ControllerBase
    {
        private TestService _testService;
        private UserService _userService;
        private LessonService _lessonService;
        public SubmitTestController(TestService testService, UserService userService, LessonService lessonService)
        {
            _testService = testService;
            _userService = userService;
            _lessonService = lessonService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TestRequestDto body)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            OperationResult<Test> findTestResult = await _testService.GetTest(body.TestId);
            if (findTestResult.Result == null)
                return findTestResult.ActionResultObject;
            OperationResult<Lesson> findLessonResult = await _lessonService.GetLesson(body.LessonId);
            if (findLessonResult.Result == null)
                return findLessonResult.ActionResultObject;
            if (!string.Equals(body.Answer.Trim(), findTestResult.Result.Answer.Trim(), StringComparison.OrdinalIgnoreCase))
                return BadRequest($"Wrong Answer for test {body.TestId}");
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email))
                return NotFound("There was a problem in retrieving the user data");
            OperationResult<User> result = await _userService.AddUserXp(email, findLessonResult.Result.Xp, body.LessonId);;
            if (result.Result == null)
                return result.ActionResultObject;
            return Ok(new TestResponseDto(result.Result));
        }

    }
}
