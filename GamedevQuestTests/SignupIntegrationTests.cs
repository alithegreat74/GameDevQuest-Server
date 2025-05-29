using GamedevQuest.Context;
using GamedevQuest.Controllers;
using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace GamedevQuestTests
{
    public class SignupIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_FullData_Success()
        {
            // Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new SignupController(dbContext, passwordHelper);

            var request = new SignupRequestDTO
            {
                Email = "test@example.com",
                Username = "testuser",
                Password = "password123"
            };

            // Act
            var result = await sut.PostAsync(request);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var signupResponse = Assert.IsType<SignupResponseDTO>(okResult.Value);
            Assert.True(signupResponse.Username?.Equals(request.Username));
            Assert.True(signupResponse.Email?.Equals(request.Email));
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            Assert.NotNull(userInDb);
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_NoEmail_Fail()
        {
            //Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new SignupController(dbContext, passwordHelper);

            var request = new SignupRequestDTO
            {
                Email = "",
                Username = "testuser",
                Password = "password123"
            };
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                sut.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            // Act
            var result = await sut.PostAsync(request);

            //Assert
            Assert.False(result.Result is OkObjectResult);
            Assert.Null(dbContext.Users.FirstOrDefault(u => u.Username == request.Username));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_NoUsername_Fail()
        {
            //Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new SignupController(dbContext, passwordHelper);

            var request = new SignupRequestDTO
            {
                Email = "test@example.com",
                Username = "",
                Password = "password123"
            };
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                sut.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            // Act
            var result = await sut.PostAsync(request);

            //Assert
            Assert.False(result.Result is OkObjectResult);
            Assert.Null(dbContext.Users.FirstOrDefault(u => u.Email == request.Email));
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_NoPassword_Fail()
        {
            //Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new SignupController(dbContext, passwordHelper);

            var request = new SignupRequestDTO
            {
                Email = "test@example.com",
                Username = "testuser",
                Password = ""
            };
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                sut.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            // Act
            var result = await sut.PostAsync(request);

            //Assert
            Assert.False(result.Result is OkObjectResult);
            Assert.Null(dbContext.Users.FirstOrDefault(u => u.Email == request.Email));
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_DuplicateUser_Fail()
        {
            // Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new SignupController(dbContext, passwordHelper);

            var request = new SignupRequestDTO
            {
                Email = "test@example.com",
                Username = "testuser",
                Password = "password123"
            };

            dbContext.Add(new User { Email = request.Email, Username = request.Username, FirstName = "", LastName = "", Password = "" });
            await dbContext.SaveChangesAsync();

            // Act
            var result = await sut.PostAsync(request);

            // Assert
            Assert.False(result.Result is OkObjectResult);
        }
    }
}
