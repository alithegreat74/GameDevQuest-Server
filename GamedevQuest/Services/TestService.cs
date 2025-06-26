using GamedevQuest.Models;
using GamedevQuest.Repositories;
using System.Security.Cryptography;

namespace GamedevQuest.Services
{
    public class TestService
    {
        private readonly TestRepository _repository;

        public TestService(TestRepository repository)
        {
            _repository = repository;
        }
        public async Task<(Test? test, string errorMessage)> GetTest(int id)
        {
            Test? findTest = await _repository.GetTest(id);
            if (findTest == null)
                return (findTest, $"Couldn't find test with id: {id}");
            return (findTest,"");
        }
        public async Task<(Test? test, string errorMessage)> FindTestForLesson(Lesson lesson)
        {
            if (lesson.RelatedTests == null || lesson.RelatedTests.Count == 0)
                return (null, "No tests available for this lesson");

            int relatedTestSize = lesson.RelatedTests.Count;
            int randomTest = RandomNumberGenerator.GetInt32(0, relatedTestSize);
            return await GetTest(lesson.RelatedTests[randomTest]);
        }
    }
}
