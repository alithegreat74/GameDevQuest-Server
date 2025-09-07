using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly UserSignupService _userSignupService;
        private readonly AuthorizationHelper _authorizationHelper;
        public SignupController(AuthorizationHelper authorizationHelper, UserSignupService userServiceService)
        {
            _authorizationHelper = authorizationHelper;
            _userSignupService = userServiceService;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SignupRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            OperationResult<bool> result = await _userSignupService.CanCreateUser(request);
            if(!result.Result)
                return result.ActionResultObject;
            User newUser = await _userSignupService.CreateUser(request);
            if (HttpContext.Connection.RemoteIpAddress == null)
                return UnprocessableEntity("User ip is null");
            string ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            OperationResult<bool> savePlayerSession = 
                await _authorizationHelper.SaveUserSession(newUser.Id, newUser.Email, ipAddress, Response);
            return Ok(new SignupResponseDto(newUser));
        }
    }
}