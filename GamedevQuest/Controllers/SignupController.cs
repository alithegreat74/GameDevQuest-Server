using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        private readonly JwtTokenHelper _jwtTokenGenerator;
        public SignupController(GameDevQuestDbContext context, JwtTokenHelper jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        [HttpPost]
        public async Task<ActionResult<SignupResponseDto>> PostAsync([FromBody] SignupRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var unitOfWork = new UnitOfWork(_context);
            await unitOfWork.StartTransaction();
            var userRepository = new UserRepository(_context.Users);
            var userService= new UserSignupService(userRepository);
            (bool canCreateUser, string errorMessage) = await userService.CanCreateUser(request);
            if(!canCreateUser)
                return BadRequest(errorMessage);
            User newUser = await userService.CreateUser(request);
            await unitOfWork.CommitChanges();
            string token = _jwtTokenGenerator.GenerateToken(newUser.Username);
            return Ok(new SignupResponseDto(newUser, token));
        }
    }
}