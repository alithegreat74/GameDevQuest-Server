using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<LoginResponseDto>> PostAsync([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            (User? user, string errorMessage) = await _userLoginService.ValidateUserLogin(request);
            if (user==null)
                return Unauthorized(errorMessage);
            string token = _jwtTokenGenerator.GenerateToken(request.Username);
            return Ok(new LoginResponseDto(user, token));
        }
    }
}
