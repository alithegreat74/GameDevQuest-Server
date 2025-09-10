using GamedevQuest.Models;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GamedevQuest.Models.DTO;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetXpController: ControllerBase
    {
        private readonly UserService _userService;

        public GetXpController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (email == null)
                return NotFound("Could not find the user's email");
            OperationResult<User> findUser = await _userService.GetUserByEmail(email);
            if (findUser.Result == null)
                return findUser.ActionResultObject;
            return Ok(new XpDto(findUser.Result));
        }
    }
}
