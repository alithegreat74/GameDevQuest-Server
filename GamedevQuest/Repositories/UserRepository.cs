using GamedevQuest.Context;
using GamedevQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GamedevQuest.Repositories
{
    public class UserRepository
    {
        private readonly GameDevQuestDbContext _context;
        public UserRepository(GameDevQuestDbContext context)
        {
            _context = context;
        }
        public async Task<User?> FindUser(string username, string email) 
        {
            return await _context.Users.FirstOrDefaultAsync
                (user => user.Email.ToLower().Equals(email.ToLower()) || user.Username.ToLower().Equals(username.ToLower()));
        }
        public async Task<User?> FindUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync
                (user => user.Username.ToLower().Equals(username.ToLower()));
        }
        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }
    }
}
