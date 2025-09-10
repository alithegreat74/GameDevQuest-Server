using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthorizationService _authorizationHelper;
        private readonly UserLoginService _userLoginService;
        private readonly IpAddressHelper _ipAddressHelper;
        public LoginController(AuthorizationService authorizationHelper, UserLoginService userLoginService)
        {
            _authorizationHelper = authorizationHelper;
            _userLoginService = userLoginService;
            _ipAddressHelper = new IpAddressHelper();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            OperationResult<User> result = await _userLoginService.ValidateUserLogin(request);
            if (result.Result == null)
                return result.ActionResultObject;
            OperationResult<string> findIpAddress = _ipAddressHelper.GetIpAddress(HttpContext, Request);
            if (findIpAddress.Result == null)
                return findIpAddress.ActionResultObject;
            OperationResult<bool> savePlayerSessionResult = 
                await _authorizationHelper.SaveUserSession(result.Result.Id, result.Result.Email, findIpAddress.Result, Response);
            if (savePlayerSessionResult.Result == false)
                return savePlayerSessionResult.ActionResultObject;
            return Ok(new LoginResponseDto(result.Result));
        }
    }
}
