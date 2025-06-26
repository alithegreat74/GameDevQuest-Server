namespace GamedevQuest.Helpers.DatabaseHelpers
{
    public interface IUnitOfWork
    {
        public Task StartTransaction();
        public Task CommitChanges();
        public void RollBackChanges();
    }
}
