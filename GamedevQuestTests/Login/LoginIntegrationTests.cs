using GamedevQuest.Context;
using GamedevQuest.Controllers;
using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using GamedevQuest.Services;
using GamedevQuestTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Xunit;

namespace GamedevQuestTests.Login
{
    [Collection("IntegrationTests")]
    public class LoginIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task LoginController_HappyPath_Success()
        {
            //Arrange
            var request = new LoginRequestDto
            {
                Username = "username",
                Password = "password",
            };
            GameDevQuestDbContext context = Utility.GetContext();
            var sut = CreateSystemUnderTest(context);

            //Act
            ActionResult<LoginResponseDto> response =  await sut.PostAsync(request);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var loginResponseDto = Assert.IsType<LoginResponseDto>(okResult.Value);
            Assert.True(loginResponseDto.Username?.Equals(request.Username));
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task LoginController_NoUsername_Fail()
        {
            //Arrange
            var request = new LoginRequestDto
            {
                Username = "",
                Password = "password",
            };
            GameDevQuestDbContext context = Utility.GetContext();
            var sut = CreateSystemUnderTest(context);
            SetupModelValidation(sut, request);

            //Act
            ActionResult<LoginResponseDto> response = await sut.PostAsync(request);

            //Assert
            Assert.False(response.Result is OkObjectResult);
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task LoginController_NoPassword_Fail()
        {
            //Arrange
            var request = new LoginRequestDto
            {
                Username = "username",
                Password = "",
            };
            GameDevQuestDbContext context = Utility.GetContext();
            var sut = CreateSystemUnderTest(context);
            SetupModelValidation(sut, request);

            //Act
            ActionResult<LoginResponseDto> response = await sut.PostAsync(request);

            //Assert
            Assert.False(response.Result is OkObjectResult);
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task LoginController_WrongPassword_Fail()
        {
            //Arrange
            var request = new LoginRequestDto
            {
                Username = "username",
                Password = "wrongPassword",
            };
            GameDevQuestDbContext context = Utility.GetContext();
            var sut = CreateSystemUnderTest(context);
            await RemoveAllUsers(context);
            User dummyUser = CreateDummyUser(request.Username, "password");
            await context.Users.AddAsync(dummyUser);
            await context.SaveChangesAsync();

            //Act
            ActionResult<LoginResponseDto> response = await sut.PostAsync(request);

            //Assert
            Assert.False(response.Result is OkObjectResult);
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task LoginController_WrongUsername_Fail()
        {
            //Arrange
            var request = new LoginRequestDto
            {
                Username = "wrongUsername",
                Password = "password",
            };
            GameDevQuestDbContext context = Utility.GetContext();
            var sut = CreateSystemUnderTest(context);
            await RemoveAllUsers(context);
            User dummyUser = CreateDummyUser("username", request.Password);
            await context.Users.AddAsync(dummyUser);
            await context.SaveChangesAsync();

            //Act
            ActionResult<LoginResponseDto> response = await sut.PostAsync(request);

            //Assert
            Assert.False(response.Result is OkObjectResult);
        }

        private void SetupModelValidation(LoginController sut, LoginRequestDto request)
        {
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                sut.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }
        private async Task RemoveAllUsers(GameDevQuestDbContext context)
        {
            var allUsers = await context.Users.ToListAsync();
            context.Users.RemoveRange(allUsers);
            await context.SaveChangesAsync();
        }
        private User CreateDummyUser(string username, string password)
        {
            var passwordHelper = new PasswordHelper();
            return new User()
            {
                Email = "",
                Username = username,
                Password = passwordHelper.HashPassword(password),
                FirstName = "",
                LastName = "",
                Level = 0
            };
        }
        private LoginController CreateSystemUnderTest(GameDevQuestDbContext context)
        {
            var passwordHelper = new PasswordHelper();
            var userRepository = new UserRepository(context);
            var userLoginService = new UserLoginService(userRepository, passwordHelper);
            var jwtTokenHelper = Utility.GetJwtTokenHelper();
            return new LoginController(jwtTokenHelper, userLoginService);
        }
    }
}
