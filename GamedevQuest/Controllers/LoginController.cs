using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Mvc;
using static GamedevQuest.Helpers.AuthorizationHelper;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthorizationHelper _authorizationHelper;
        private readonly UserLoginService _userLoginService;
        public LoginController(AuthorizationHelper authorizationHelper, UserLoginService userLoginService)
        {
            _authorizationHelper = authorizationHelper;
            _userLoginService = userLoginService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            OperationResult<User> result = await _userLoginService.ValidateUserLogin(request);
            if (result.Result == null)
                return result.ActionResultObject;
            _authorizationHelper.SavePlayerSession(result.Result.Email, Response);
            return Ok(new LoginResponseDto(result.Result));
        }
    }
}
