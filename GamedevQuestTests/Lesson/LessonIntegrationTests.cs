using GamedevQuest.Context;
using GamedevQuest.Controllers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuestTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GamedevQuestTests.Lesson
{
    [Collection("IntegrationTests")]
    public class LessonIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetLessonController_HappyPath_Success()
        {
            //Arrange
            GameDevQuestDbContext context = Utility.GetContext();
            await RemoveAllLessonsAndTests(context);
            await CreateDummyLessonAndTest(context);
            var sut = new GetLessonController(context);
            //Act
            ActionResult<LessonDetailResponseDto> response = await sut.GetAsync(1);
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var lessonResponse = Assert.IsType<LessonDetailResponseDto>(okResult.Value);
            Assert.NotNull(lessonResponse);
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetLessonController_WrongId_Fail()
        {
            //Arrange
            GameDevQuestDbContext context = Utility.GetContext();
            await RemoveAllLessonsAndTests(context);
            var sut = new GetLessonController(context);
            //Act
            ActionResult<LessonDetailResponseDto> response = await sut.GetAsync(1);
            //Assert
            Assert.False(response.Result is OkObjectResult);
        }
        private async Task CreateDummyLessonAndTest(GameDevQuestDbContext context)
        {
            var lesson = new GamedevQuest.Models.Lesson()
            {
                Id = 1,
                LessonTitle = "Test",
                LessonDescription = "Test Description",
                MinimumRequiredLevel = 1,
                RelatedTests = new List<int> { 1}
            };
            var test = new Test()
            {
                Id = 1,
                TestDescription = "Test",
                Answer = "Test"
            };
            context.Lessons.Add(lesson);
            context.Tests.Add(test);

            await context.SaveChangesAsync();
        }
        private async Task RemoveAllLessonsAndTests(GameDevQuestDbContext context)
        {
            var allLessons = await context.Lessons.ToListAsync();
            context.Lessons.RemoveRange(allLessons);

            var allTests = await context.Tests.ToListAsync();
            context.Tests.RemoveRange(allTests);

            await context.SaveChangesAsync();
        }
    }
}
