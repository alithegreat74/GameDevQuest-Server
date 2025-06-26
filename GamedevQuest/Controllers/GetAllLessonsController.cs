using GamedevQuest.Context;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using GamedevQuest.Services;
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
        public ActionResult<List<LessonResponseDto>> Get()
        {
            var lessonRepository = new LessonRepository(_context.Lessons);
            var lessonService = new LessonService(lessonRepository);
            List<LessonResponseDto> responses = lessonService.GetAllLessons();
            return Ok(responses);
        }
    }
}
