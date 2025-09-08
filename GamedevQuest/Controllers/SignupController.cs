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
        private readonly AuthorizationService _authorizationHelper;
        private readonly IpAddressHelper _ipAddressHelper;
        public SignupController(AuthorizationService authorizationHelper, UserSignupService userServiceService)
        {
            _authorizationHelper = authorizationHelper;
            _userSignupService = userServiceService;
            _ipAddressHelper = new IpAddressHelper();
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
            OperationResult<string> findIpAddress = _ipAddressHelper.GetIpAddress(HttpContext, Request);
            if (findIpAddress.Result == null)
                return findIpAddress.ActionResultObject;
            OperationResult<bool> savePlayerSession = 
                await _authorizationHelper.SaveUserSession(newUser.Id, newUser.Email, findIpAddress.Result, Response);
            return Ok(new SignupResponseDto(newUser));
        }
    }
}