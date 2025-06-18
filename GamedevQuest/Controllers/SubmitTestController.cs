using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/test/[Controller]")]
    public class SubmitTestController : ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        public SubmitTestController(GameDevQuestDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostAsync([FromBody] TestRequestDto body)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Test? test = await _context.Tests.FirstOrDefaultAsync(test => test.Id == body.TestId);
            if (test == null)
                return NotFound($"No test found with id {body.TestId}");

            if (test.Answer != body.Answer)
                return BadRequest($"Wrong Answer for test {body.TestId}");

            string? username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
                throw new NullReferenceException("The user jwt token claim is not of type name");

            User? user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
            if (user == null)
                throw new NullReferenceException($"The user with username {username} was not found");

            user.Xp += test.TestXp;
            user.Level += user.Xp >= user.LevelXp ? 1 : 0;
            _context.SaveChanges();
            return Ok(user);
        }

    }
}
