
using GamedevQuest.Context;
using GamedevQuest.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace GamedevQuest.Helpers.DatabaseHelpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameDevQuestDbContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(GameDevQuestDbContext context)
        {
            _context = context;
        }
        public async Task StartTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
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
                await RollBackChanges();
            }
        }

        public async Task RollBackChanges()
        {
            await _transaction.RollbackAsync();
        }

        
    }
}
