using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<OperationResult<Test>> GetTest(int id)
        {
            Test? findTest = await _repository.GetTest(id);
            if (findTest == null)
                return new OperationResult<Test>(new NotFoundObjectResult($"Couldn't find test with id: {id}"));
            return new OperationResult<Test>(findTest);
        }
        public async Task<OperationResult<List<TestSummaryDto>>> FindTestsForLesson(Lesson lesson)
        {
            if (lesson.RelatedTests == null || lesson.RelatedTests.Count == 0)
                return new OperationResult<List<TestSummaryDto>>(new NotFoundObjectResult("No tests available for this lesson"));

            var tasks = lesson.RelatedTests.Select(testId => GetTest(testId)).ToList();
            var results = await Task.WhenAll(tasks);

            return new OperationResult<List<TestSummaryDto>>(
                results.Select(result => result.Result)
                        .OfType<Test>()
                        .Select(test=>new TestSummaryDto(test))
                        .ToList()
                );

        }
    }
}
