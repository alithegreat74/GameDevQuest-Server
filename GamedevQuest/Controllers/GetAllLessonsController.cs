using GamedevQuest.Context;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<List<LessonResponseDto>>> Get()

        {
            List<Lesson> lessons = _context.Lessons.AsNoTracking().ToList();
            List<LessonResponseDto> responses = lessons.Select(lesson => new LessonResponseDto(lesson)).ToList();
            return Ok(responses);
        }
    }
}
