using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using static GamedevQuest.Models.DTO.TestSubmissionDto;

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
                return new OperationResult<List<TestSummaryDto>>(new List<TestSummaryDto>());

            var summaries = new List<TestSummaryDto>(lesson.RelatedTests.Count);
            foreach(var test in lesson.RelatedTests)
            {
                var operationResult = await GetTest(test);
                if (operationResult.Result == null)
                    continue;

                summaries.Add(new TestSummaryDto(operationResult.Result));
            }

            return new OperationResult<List<TestSummaryDto>>(summaries);
        }
        public async Task<bool> CheckTestAnswer(int id, string answer)
        {
            OperationResult<Test> findTest = await GetTest(id);
            return findTest.Result!=null && findTest.Result.Answer.Equals(answer);
        }
        public async Task<List<int>> FilterTestSubmissions(List<TestSubmission> submissions, User user, Lesson lesson)
        {
            var filteredTests = new List<int>();

            foreach (var submission in submissions)
            {
                bool alreadySolved = user.SolvedTests.Contains(submission.TestId);
                bool related = lesson.RelatedTests.Contains(submission.TestId);
                bool answerCorrect = await CheckTestAnswer(submission.TestId, submission.Answer);

                if (!alreadySolved && related && answerCorrect)
                {
                    filteredTests.Add(submission.TestId);
                }
            }
            return filteredTests;
        }
    }
}
