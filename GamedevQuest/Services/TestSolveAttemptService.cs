using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Repositories;

namespace GamedevQuest.Services
{
    public class TestSolveAttemptService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TestSolveAttemptRepository _repository;
        public TestSolveAttemptService(TestSolveAttemptRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public List<TestSolveAttempt> SubmitUserTests(User user, IEnumerable<int> tests)
        {
            IEnumerable<TestSolveAttempt> testSolveAttempts = _repository.GetTestSolveAttemptsByUserIdNoTracking(user.Id, DateTime.UtcNow.AddMinutes(-10));
            List<TestSolveAttempt> filteredAttempts = testSolveAttempts.Where(attempt => tests.Contains(attempt.TestId)).ToList();
            return filteredAttempts;
        }
        public async Task AddTestSolveAttempt(int userId, int testId)
        {
            try
            {
                await _unitOfWork.StartTransaction();
                var attempt = new TestSolveAttempt(userId, testId, DateTime.UtcNow);
                await _repository.AddAttempt(attempt);
                await _unitOfWork.CommitChanges();
            }
            catch
            {
                await _unitOfWork.RollBackChanges();
            }
        }
    }
}
