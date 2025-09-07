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
        private readonly AuthorizationHelper _authorizationHelper;
        private readonly UserService _userService;
        public LogoutController(AuthorizationHelper authorizationHelper, UserService userService)
        {
            _authorizationHelper = authorizationHelper;
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email))
                return NotFound("There was a problem in retrieving the user data");
            OperationResult<User?> findUser = await _userService.GetUserByEmail(email);
            if (findUser.Result == null)
                return findUser.ActionResultObject;
            if (HttpContext.Connection.RemoteIpAddress == null)
                return UnprocessableEntity("User ip is null");
            string ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            OperationResult<bool> removePlayerSession= await _authorizationHelper.RemoveUserSession(findUser.Result.Id, ipAddress, Response);
            if (removePlayerSession.Result == null)
                return removePlayerSession.ActionResultObject;
            return Ok();
        }
    }
}
