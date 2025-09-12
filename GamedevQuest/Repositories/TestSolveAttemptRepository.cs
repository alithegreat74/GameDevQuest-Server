using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class TestSolveAttemptRepository
    {
        private DbSet<TestSolveAttempt> _testSolveAttempts;
        public TestSolveAttemptRepository(GameDevQuestDbContext context)
        {
            _testSolveAttempts = context.TestSolveAttempts;
        }
        public IEnumerable<TestSolveAttempt> GetTestSolveAttemptsByUserIdNoTracking(int userId, DateTime date)
        {
            return _testSolveAttempts.AsNoTracking().Where(attempt=>attempt.UserId == userId && attempt.Date>date);
        }
        public async Task AddAttempt(TestSolveAttempt attempt)
        {
            await _testSolveAttempts.AddAsync(attempt);
        }
    }
}
