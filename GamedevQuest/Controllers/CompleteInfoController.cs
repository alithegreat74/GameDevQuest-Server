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
    [Authorize]
    public class CompleteInfoController : ControllerBase
    {
        private UserService _userService;
        public CompleteInfoController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CompleteInfoRequestDto body)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (email == null)
                return NotFound("Could not find the user's email");
            OperationResult<User> result = await _userService.CompleteUserInfo(email, body);
            if (result.Result == null)
                return result.ActionResultObject;
            return Ok(new CompleteInfoResponseDto(result.Result));
        }
    }
}
