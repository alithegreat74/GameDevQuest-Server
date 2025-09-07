using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class RefreshTokenRepository
    {
        private readonly DbSet<RefreshToken> _refreshTokens;
        public RefreshTokenRepository(GameDevQuestDbContext dbContext)
        {
            _refreshTokens = dbContext.RefreshTokens;
        }
        public async Task CreateRefreshToken(RefreshToken token)
        {
            await _refreshTokens.AddAsync(token);
        }
        public async Task DeleteRefreshToken(int userId, string ipAddress)
        {
            await _refreshTokens
                .Where(token => token.IpAddress == ipAddress && token.UserId == userId)
                .ExecuteDeleteAsync();
        }
        public async Task<RefreshToken?> FindRefreshTokenNoTracking(string token)
        {
            return await _refreshTokens.AsNoTracking().FirstOrDefaultAsync(refreshToken => refreshToken.Token == token);
        }
    }
}
