using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Context
{
    public class GameDevQuestDbContext: DbContext
    {
        public GameDevQuestDbContext(DbContextOptions<GameDevQuestDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
