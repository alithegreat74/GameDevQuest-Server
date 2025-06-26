using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class TestRepository
    {
        private readonly DbSet<Test> _tests;
        public TestRepository(GameDevQuestDbContext context)
        {
            this._tests = context.Tests;
        }
        public int GetNumberOfTests()
        {
            return _tests.Count();
        }
        public async Task<Test?> GetTest(int id)
        {
            return await _tests.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
