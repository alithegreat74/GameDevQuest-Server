using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/test/[Controller]")]
    public class CheckTestAnswerController : ControllerBase
    {
        private TestService _testService;
        private LessonService _lessonService;
        private TestSolveAttemptService _testSolveAttemptService;
        private UserService _userService;
        public CheckTestAnswerController(TestService testService, LessonService lessonService, TestSolveAttemptService testSolveAttemptService, UserService userService)
        {
            _testService = testService;
            _lessonService = lessonService;
            _testSolveAttemptService = testSolveAttemptService;
            _userService = userService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TestRequestDto body)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            OperationResult<Test> findTest = await _testService.GetTest(body.TestId);
            if (findTest.Result == null)
                return findTest.ActionResultObject;
            OperationResult<Lesson> findLesson = await _lessonService.GetLesson(body.LessonId);
            if (findLesson.Result == null)
                return findLesson.ActionResultObject;
            if (!findLesson.Result.RelatedTests.Contains(body.TestId))
                return BadRequest($"Test {body.TestId} does not belong to lesson {body.LessonId}");
            if (!string.Equals(body.Answer.Trim(), findTest.Result.Answer.Trim(), StringComparison.OrdinalIgnoreCase))
                return BadRequest(new CheckTestAnswerDto() { TestId = body.TestId, Answer = findTest.Result.Answer });
            OperationResult<User>findUser = await _userService.GetUserFromCookie(User);
            if (findUser.Result == null)
                return findUser.ActionResultObject;
            await _testSolveAttemptService.AddTestSolveAttempt(findUser.Result.Id, body.TestId);
            return Ok();
        }

    }
}
