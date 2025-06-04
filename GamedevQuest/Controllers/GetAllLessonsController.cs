using GamedevQuest.Context;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetAllLessonsController:ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        public GetAllLessonsController(GameDevQuestDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<LessonResponseDto>> Get()
        {
            List<LessonResponseDto> responses = new List<LessonResponseDto>();
            foreach (var lesson in _context.Lessons)
            {
                responses.Add(new LessonResponseDto(lesson));   
            }
            return Ok(responses);
        }
    }
}
