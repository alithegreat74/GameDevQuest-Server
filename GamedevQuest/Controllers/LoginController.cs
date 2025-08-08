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
        private readonly JwtTokenHelper _jwtTokenGenerator;
        private readonly UserLoginService _userLoginService;
        public LoginController(JwtTokenHelper jwtTokenGenerator, UserLoginService userLoginService)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
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
            string token = _jwtTokenGenerator.GenerateToken(result.Result.Email);
            return Ok(new LoginResponseDto(result.Result, token));
        }
    }
}
