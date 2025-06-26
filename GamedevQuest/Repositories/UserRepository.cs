using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class UserRepository
    {
        private readonly DbSet<User> _set;
        public UserRepository(DbSet<User> set)
        {
            _set = set;
        }
        public async Task<User?> FindUser(string username, string email) 
        {
            return await _set.FirstOrDefaultAsync
                (user => user.Email.ToLower().Equals(email.ToLower()) || user.Username.ToLower().Equals(username.ToLower()));
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
    }
}
