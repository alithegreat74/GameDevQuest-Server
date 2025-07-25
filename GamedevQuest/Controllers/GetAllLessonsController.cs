using GamedevQuest.Models.DTO;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetAllLessonsController : ControllerBase
    {
        private readonly LessonService _lessonService;

        public GetAllLessonsController(LessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<LessonResponseDto>> Get()
        {
            List<LessonResponseDto> responses = _lessonService.GetAllLessons();
            return Ok(responses);
        }
    }
}
