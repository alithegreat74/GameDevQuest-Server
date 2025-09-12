using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmitTestsController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly LessonService _lessonService;
        private readonly TestService _testService;
        private readonly TestSolveAttemptService _testSolveAttemptService;
        public SubmitTestsController(UserService userService, LessonService lessonService, TestService testService, TestSolveAttemptService testSolveAttemptService)
        {
            _userService = userService;
            _lessonService = lessonService;
            _testService = testService;
            _testSolveAttemptService = testSolveAttemptService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TestSubmissionDto body)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            OperationResult<User> findUser = await _userService.GetUserFromCookie(User);
            if (findUser.Result == null)
                return findUser.ActionResultObject;
            OperationResult<Lesson> findLesson = await _lessonService.GetLesson(body.LessonId);
            if (findLesson.Result == null)
                return findLesson.ActionResultObject;
            List<int> filteredTests = await _testService.FilterTestSubmissions(body.Submissions, findUser.Result, findLesson.Result);
            List<TestSolveAttempt> attempts = _testSolveAttemptService.SubmitUserTests(findUser.Result, filteredTests);
            await _userService.AddUserXp(findUser.Result, _lessonService.CalculateUserXpGain(findLesson.Result, attempts.Count));
            await _userService.AddSolvedTests(findUser.Result, attempts);
            return Ok(new TestSubmissionResponseDto(findUser.Result));
        }
    }
}
