using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public SignupController(GameDevQuestDbContext context, IPasswordHelper passwordHelper, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        [HttpPost]
        public async Task<ActionResult<SignupResponseDTO>> PostAsync([FromBody] SignupRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWithSameUsernameOrEmail = await _context.Users.FirstOrDefaultAsync
                (user => user.Email.Equals(request.Email.ToLower()) || user.Username.Equals(request.Username.ToLower()));
            if (userWithSameUsernameOrEmail != default(User))
                return Conflict("A user already exists with the same username or email");

            User newUser = CreateNewUser(request);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            string token = _jwtTokenGenerator.GenerateToken(newUser.Username);
            return Ok(
                new SignupResponseDTO
                {
                    Id = newUser.Id,
                    Token = token,
                    Email = newUser.Email,
                    Username = newUser.Username,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName
                }
            );
        }
        private User CreateNewUser(SignupRequestDTO request)
        {

            var newUser = new User
            {
                Email = request.Email,
                Username = request.Username,
                Password = "",
                FirstName = "",
                LastName = ""
            };
            string hashedPassword = _passwordHelper.HashPassword(request.Password);
            newUser.Password = hashedPassword;
            return newUser;
        }
    }
}