using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtTokenGenerator;
        private readonly UserSignupService _userSignupService;
        public SignupController(JwtTokenHelper jwtTokenGenerator, UserSignupService userServiceService)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userSignupService = userServiceService;
        }
        [HttpPost]
        public async Task<ActionResult<SignupResponseDto>> PostAsync([FromBody] SignupRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            (bool canCreateUser, string errorMessage) = await _userSignupService.CanCreateUser(request);
            if(!canCreateUser)
                return BadRequest(errorMessage);
            User newUser = await _userSignupService.CreateUser(request);
            string token = _jwtTokenGenerator.GenerateToken(newUser.Email);
            return Ok(new SignupResponseDto(newUser, token));
        }
    }
}