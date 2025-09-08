using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class UserRepository
    {
        private readonly DbSet<User> _set;
        public UserRepository(GameDevQuestDbContext context)
        {
            _set = context.Users;
        }
        public async Task<User?> FindUserByEmailNoTracking(string email)
        {
            return await _set.AsNoTracking().FirstOrDefaultAsync
                (user => user.Email != null && user.Email.ToLower().Equals(email.ToLower()));
        }
        public async Task<User?> FindUserByUsernameNoTracking(string username)
        {
            return await _set.AsNoTracking().FirstOrDefaultAsync
                (user => user.Username != null && user.Username.ToLower().Equals(username.ToLower()));
        }
        public async Task<User?> FindUserByIdNoTracking(int userId)
        {
            return await _set.AsNoTracking().FirstOrDefaultAsync
                (user => user.Id==userId);
        }
        public async Task<User?> FindUserByEmailOrUsernameNoTracking(string userIdentifier)
        {
            return await _set.AsNoTracking().FirstOrDefaultAsync
                (user => (user.Email != null && user.Email.ToLower().Equals(userIdentifier.ToLower())) ||
                         (user.Username != null && user.Username.ToLower().Equals(userIdentifier.ToLower())));
        }
        public async Task<User?> FindUserByUsername(string username)
        {
            return await _set.FirstOrDefaultAsync
                (user => user.Username != null && user.Username.ToLower().Equals(username.ToLower()));
        }
        public async Task<User?> FindUserByEmail(string email)
        {
            return await _set.FirstOrDefaultAsync
                (user => user.Email != null && user.Email.ToLower().Equals(email.ToLower()));
        }
        public async Task AddUser(User user)
        {
            await _set.AddAsync(user);
        }
    }
}
