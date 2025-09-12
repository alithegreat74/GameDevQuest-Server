using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetAllLessonsController : ControllerBase
    {
        private readonly LessonService _lessonService;
        private readonly UserService _userService;
        public GetAllLessonsController(LessonService lessonService, UserService userService)
        {
            _lessonService = lessonService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            OperationResult<User> findUserResult = await _userService.GetUserFromCookie(User);
            if (findUserResult.Result == null)
                return findUserResult.ActionResultObject;
            List<LessonResponseDto> responses = _lessonService.GetAllLessons(findUserResult.Result);
            return Ok(responses);
        }
    }
}
