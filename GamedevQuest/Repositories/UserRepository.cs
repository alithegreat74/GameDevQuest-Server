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
        public async Task<User?> FindUserNoTracking(string username, string email) 
        {
            return await _set.AsNoTracking().FirstOrDefaultAsync
                (user => user.Email.ToLower().Equals(email.ToLower()) || user.Username.ToLower().Equals(username.ToLower()));
        }
        public async Task<User?> FindUserNoTracking(string username)
        {
            return await _set.AsNoTracking().FirstOrDefaultAsync
                (user => user.Username.ToLower().Equals(username.ToLower()));
        }
        public async Task<User?> FindUser(string username)
        {
            return await _set.FirstOrDefaultAsync
                (user => user.Username.ToLower().Equals(username.ToLower()));
        }
        public async Task AddUser(User user)
        {
            await _set.AddAsync(user);
        }
        public async Task<User?> AddUserXp(string username, int xp)
        {
            User? user = await FindUser(username);
            if (user == null)
                return null;
            user.UpdateXp(xp);
            return user;
        }
    }
}
