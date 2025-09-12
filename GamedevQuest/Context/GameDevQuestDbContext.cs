using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Context
{
    public class GameDevQuestDbContext: DbContext
    {
        public GameDevQuestDbContext(DbContextOptions<GameDevQuestDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TestSolveAttempt> TestSolveAttempts { get; set; }
    }
}
