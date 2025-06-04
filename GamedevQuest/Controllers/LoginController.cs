using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public LoginController(GameDevQuestDbContext context, IPasswordHelper passwordHelper, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponseDto>> PostAsync([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMatch = await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower().Equals(request.Username.ToLower()));
            if (userMatch == null || !_passwordHelper.VerifyPassword(userMatch.Password, request.Password))
                return Unauthorized("No User found");

            string token = _jwtTokenGenerator.GenerateToken(request.Username);
            return Ok(
                new LoginResponseDto
                {
                    Id = userMatch.Id,
                    Token = token,
                    Username = userMatch.Username,
                    Email = userMatch.Email,
                    FirstName = userMatch.FirstName,
                    LastName = userMatch.LastName,
                }
            );

        }
    }
}
