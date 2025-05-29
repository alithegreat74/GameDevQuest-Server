
using GamedevQuest.Context;
using GamedevQuest.Controllers;
using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace GamedevQuestTests.Login
{
    public class LoginIntegrationTests
    {
        [Fact]
        [Trait("Category","IntegrationTests")]
        public async Task Login_FullData_Success()
        {
            // Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new LoginController(dbContext, passwordHelper);

            var request = new LoginRequestDTO
            {
                Username = "username",
                Password = "password",
            };
            dbContext.Users.Add(
                new User
                {
                    Email = "",
                    Username = request.Username,
                    Password = passwordHelper.HashPassword(request.Password),
                    FirstName = "",
                    LastName = ""
                }
            );
            dbContext.SaveChanges();
            //Act
            var result = await sut.PostAsync(request);


            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var loginResponse = Assert.IsType<LoginResponseDTO>(okResult.Value);
            Assert.True(loginResponse.Username?.Equals(request.Username));
        }


        [Fact]
        [Trait("Category", "IntegrationTests")]
        public async Task Login_UserDoesntExist_Fail()
        {
            // Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new LoginController(dbContext, passwordHelper);

            var request = new LoginRequestDTO
            {
                Username = "username",
                Password = "password",
            };
            //Act
            var result = await sut.PostAsync(request);

            //Assert
            Assert.IsNotType<OkObjectResult>(result.Result);
        }
        [Fact]
        [Trait("Category", "IntegrationTests")]
        public async Task Login_NoUsername_Fail()
        {
            // Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new LoginController(dbContext, passwordHelper);

            var request = new LoginRequestDTO
            {
                Username = "",
                Password = "password",
            };

            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                sut.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            //Act
            var result = await sut.PostAsync(request);

            //Assert
            Assert.IsNotType<OkObjectResult>(result.Result);
        }

        [Fact]
        [Trait("Category", "IntegrationTests")]
        public async Task Login_NoPassword_Fail()
        {
            // Arrange
            var passwordHelper = new PasswordHelper();
            var options = new DbContextOptionsBuilder<GameDevQuestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new GameDevQuestDbContext(options);
            var sut = new LoginController(dbContext, passwordHelper);

            var request = new LoginRequestDTO
            {
                Username = "username",
                Password = "",
            };

            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                sut.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            //Act
            var result = await sut.PostAsync(request);

            //Assert
            Assert.IsNotType<OkObjectResult>(result.Result);
        }
    }
}
