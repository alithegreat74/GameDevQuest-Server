using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Helpers.DatabaseHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GamedevQuestTests.Utilities
{
    public class Utility
    {
        public static GameDevQuestDbContext GetContext()
        {
            string? connectionString = new ConnectionHelper().GenerateConnectionString();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>().
                UseNpgsql(connectionString)
                .Options;
            return new GameDevQuestDbContext(options);
        }
        public static TokenHelper GetJwtTokenHelper()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"jwt:Key", "G@meDevQuestSecretKeyWhichIsBig123456789"},
                {"jwt:Issuer", "GamedevQuest"},
                {"jwt:Audience", "GamedevQuesters"},
                {"jwt:ExpireMinutes", "120"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return new TokenHelper(configuration);
        }
    }
}
