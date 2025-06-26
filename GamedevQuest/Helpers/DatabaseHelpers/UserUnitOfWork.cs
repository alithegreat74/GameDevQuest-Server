using GamedevQuest.Context;
using GamedevQuest.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace GamedevQuest.Helpers.DatabaseHelpers
{
    public class UserUnitOfWork : IUnitOfWork
    {
        private readonly GameDevQuestDbContext _context;
        private readonly UserRepository _userRepository;
        private IDbContextTransaction _transaction;

        public UserUnitOfWork(GameDevQuestDbContext context)
        {
            _context = context;
            _userRepository = new UserRepository(context);
        }
        public async Task StartTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public UserRepository GetRepository()
        {
            return _userRepository;
        }
        public async Task CommitChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                RollBackChanges();
            }
        }
        public void RollBackChanges()
        {
            _transaction.Rollback();
        }
    }
}
