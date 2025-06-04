using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetLessonController:ControllerBase
    {
        private readonly GameDevQuestDbContext _context;
        public GetLessonController(GameDevQuestDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetAsync(int id)
        {
            Lesson find = await _context.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == id);

            if(find == null)
                return NotFound("No lesson found with this id");

            return Ok(find);
        }
    }
}
