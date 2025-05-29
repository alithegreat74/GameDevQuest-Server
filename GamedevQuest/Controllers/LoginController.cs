using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        private readonly IPasswordHelper _passwordHelper;

        public LoginController(GameDevQuestDbContext context, IPasswordHelper passwordHelper)
        {
            _context = context;
            _passwordHelper = passwordHelper;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponseDTO>> PostAsync([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMatch = await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower().Equals(request.Username.ToLower()));
            if (userMatch == null || !_passwordHelper.VerifyPassword(userMatch.Password, request.Password))
                return Unauthorized("No User found");

            return Ok(
                new LoginResponseDTO
                {
                    Id = userMatch.Id,
                    Username = userMatch.Username,
                    Email = userMatch.Email,
                    FirstName = userMatch.FirstName,
                    LastName = userMatch.LastName,
                }
            );

        }
    }
}
