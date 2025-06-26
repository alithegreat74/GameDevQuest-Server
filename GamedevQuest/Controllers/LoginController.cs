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
        private readonly GameDevQuestDbContext _context;
        private readonly JwtTokenHelper _jwtTokenGenerator;

        public LoginController(GameDevQuestDbContext context, JwtTokenHelper jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponseDto>> PostAsync([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userRepository = new UserRepository(_context.Users);
            var userService = new UserLoginService(userRepository);
            (User? user, string errorMessage) = await userService.ValidateUserLogin(request);
            if (user==null)
                return Unauthorized(errorMessage);
            string token = _jwtTokenGenerator.GenerateToken(request.Username);
            return Ok(new LoginResponseDto(user, token));
        }
    }
}
