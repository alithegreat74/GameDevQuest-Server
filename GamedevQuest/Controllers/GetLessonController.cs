using GamedevQuest.Context;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
        public async Task<ActionResult<LessonDetailResponseDto>> GetAsync(int id)
        {
            Lesson find = await _context.Lessons.AsNoTracking().FirstOrDefaultAsync(lesson => lesson.Id == id);
            if (find == null)
                return NotFound("No lesson found with this id");
            int relatedTestSize = find.RelatedTests.Count;
            int randomTest = RandomNumberGenerator.GetInt32(1,relatedTestSize+1);
            Test findTest = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(test => test.Id == randomTest);
            var response = new LessonDetailResponseDto(find, findTest);
            return Ok(response);
        }
    }
}
