using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class TestRepository
    {
        private readonly DbSet<Test> _tests;
        public TestRepository(DbSet<Test> tests)
        {
            this._tests = tests;
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
