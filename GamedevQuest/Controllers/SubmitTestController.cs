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
        public SubmitTestController(TestService testService, UserService userService)
        {
            _testService = testService;
            _userService = userService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostAsync([FromBody] TestRequestDto body)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            (Test? test, string errorMessage) = await _testService.GetTest(body.TestId);
            if (test == null)
                return NotFound(errorMessage);
            if (test.Answer != body.Answer)
                return BadRequest($"Wrong Answer for test {body.TestId}");
            string? username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
                return NotFound("There was a problem in retrieving the user data");
            (User? user, errorMessage) = await _userService.AddUserXp(username, test.TestXp, body.LessonId);
            if (user == null)
                return BadRequest(errorMessage);
            return Ok(user);
        }

    }
}
