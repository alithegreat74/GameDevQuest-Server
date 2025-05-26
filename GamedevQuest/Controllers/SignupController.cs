using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly GameDevQuestDbContext _context;

        public SignupController(GameDevQuestDbContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "signup")]
        public async Task<ActionResult<User>> PostAsync([FromBody]SignupRequest request)
        {
            if(string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Make sure you fill all the necessary elements");
            var userWithSameUsernameOrEmail = _context.Users.FirstOrDefault(user => user.Email.Equals(request.Email) || user.Username.Equals(request.Username));
            if(userWithSameUsernameOrEmail!=default(User))
                return Conflict("A user already exists with the same username or email");

            var newUser = new User
            {
                Email = request.Email,
                Username = request.Username,
                Password = request.Password,
                FirstName = "",
                LastName = ""
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return Ok(newUser);
        }
        public class SignupRequest
        {
            public string? Email { get; set; }
            public string? Username{ get; set; }
            public string? Password { get; set; }
        }
    }
}