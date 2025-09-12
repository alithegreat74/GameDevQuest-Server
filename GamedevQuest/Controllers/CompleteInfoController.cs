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
    
    public class CompleteInfoController : ControllerBase
    {
        private UserService _userService;
        public CompleteInfoController(UserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CompleteInfoRequestDto body)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            OperationResult<User> findUser = await _userService.GetUserFromCookie(User);
            if (findUser.Result == null)
                return findUser.ActionResultObject;
            OperationResult<User> result = await _userService.CompleteUserInfo(findUser.Result, body);
            if (result.Result == null)
                return result.ActionResultObject;
            return Ok(new CompleteInfoResponseDto(result.Result));
        }
    }
}
