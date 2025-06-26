using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class LessonRepository
    {
        private readonly DbSet<Lesson> _set;
        public LessonRepository(GameDevQuestDbContext context)
        {
            _set = context.Lessons;
        }
        public async Task<Lesson?> GetLesson(int id)
        {
            return await _set.FirstOrDefaultAsync(x => x.Id == id);
        }
        public List<Lesson> GetAllLessons()
        {
            return _set.AsNoTracking().ToList();
        }
    }
}
