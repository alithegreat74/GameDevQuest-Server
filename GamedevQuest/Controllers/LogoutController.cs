using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogoutController : ControllerBase
    {
        private readonly AuthorizationService _authorizationHelper;
        private readonly UserService _userService;
        private readonly IpAddressHelper _ipAddressHelper;
        public LogoutController(AuthorizationService authorizationHelper, UserService userService)
        {
            _authorizationHelper = authorizationHelper;
            _userService = userService;
            _ipAddressHelper = new IpAddressHelper();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        { 
            OperationResult<User?> findUser = await _userService.GetUserFromCookie(User);
            if (findUser.Result == null)
                return findUser.ActionResultObject;
            OperationResult<string> findIpAddress = _ipAddressHelper.GetIpAddress(HttpContext, Request);
            if (findIpAddress.Result == null)
                return findIpAddress.ActionResultObject;
            OperationResult<bool> removePlayerSession= await _authorizationHelper.RemoveUserSession(findUser.Result.Id, findIpAddress.Result, Response);
            if (removePlayerSession.Result == null)
                return removePlayerSession.ActionResultObject;
            return Ok();
        }
    }
}
