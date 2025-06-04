using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JwtTestController:ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("You are ok");
        }
    }
}
