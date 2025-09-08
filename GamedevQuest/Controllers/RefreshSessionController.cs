using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshSessionController : ControllerBase
    {
        private readonly AuthorizationService _authorizationHelper;
        private readonly UserService _userService;
        public RefreshSessionController(AuthorizationService authorizationHelper, UserService userService)
        {
            _authorizationHelper = authorizationHelper;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            OperationResult<RefreshToken> findToken = await _authorizationHelper.FindUserSession(Request);
            if (findToken.Result == null)
                return findToken.ActionResultObject;
            OperationResult<User> findUser = await _userService.GetUserById(findToken.Result.UserId);
            if (findUser.Result == null)
                return findUser.ActionResultObject;
            _authorizationHelper.RefreshUserSession(findUser.Result.Email, Response);
            return Ok();
        }
    }
}
