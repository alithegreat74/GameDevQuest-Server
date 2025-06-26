using GamedevQuest.Context;
using GamedevQuest.Controllers;
using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuestTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace GamedevQuestTests.Signup
{
    public class SignupIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task SignupController_FullData_Success()
        {
            //Arrange
            GameDevQuestDbContext context = Utility.GetContext();
            JwtTokenHelper jwtTokenHelper = Utility.GetJwtTokenHelper();
            var sut = new SignupController(context, jwtTokenHelper);
            await RemoveAllUsers(context);
            var request = new SignupRequestDto
            {
                Email = "test@example.com",
                Username = "testuser",
                Password = "password123"
            };
            //Act
            var response = await sut.PostAsync(request);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var signupResponse = Assert.IsType<SignupResponseDto>(okResult.Value);
            Assert.True(signupResponse.Username?.Equals(request.Username));
            Assert.True(signupResponse.Email?.Equals(request.Email));
            var userInDb = context.Users.FirstOrDefault(u => u.Email == request.Email);
            Assert.NotNull(userInDb);
        }
        public async Task SignupController_NoEmail_Fail()
        {
            //Arrange
            GameDevQuestDbContext context = Utility.GetContext();
            JwtTokenHelper jwtTokenHelper = Utility.GetJwtTokenHelper();
            await RemoveAllUsers(context);
            var sut = new SignupController(context, jwtTokenHelper);
            var request = new SignupRequestDto
            {
                Email = "",
                Username = "testuser",
                Password = "password123"
            };
            SetupModelValidation(sut, request);
            //Act
            ActionResult<SignupResponseDto> result = await sut.PostAsync(request);
            //Assert
            Assert.False(result.Result is OkObjectResult);
            Assert.Null(context.Users.FirstOrDefault(u => u.Username == request.Username));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_NoUsername_Fail()
        {
            //Arrange
            GameDevQuestDbContext context = Utility.GetContext();
            JwtTokenHelper jwtTokenHelper = Utility.GetJwtTokenHelper();
            await RemoveAllUsers(context);
            var sut = new SignupController(context, jwtTokenHelper);
            var request = new SignupRequestDto
            {
                Email = "test@example.com",
                Username = "",
                Password = "password123"
            };
            SetupModelValidation(sut, request);
            //Act
            ActionResult<SignupResponseDto> result = await sut.PostAsync(request);
            //Assert
            Assert.False(result.Result is OkObjectResult);
            Assert.Null(context.Users.FirstOrDefault(u => u.Email == request.Email));
        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_NoPassword_Fail()
        {
            //Arrange
            GameDevQuestDbContext context = Utility.GetContext();
            JwtTokenHelper jwtTokenHelper = Utility.GetJwtTokenHelper();
            await RemoveAllUsers(context);
            var sut = new SignupController(context, jwtTokenHelper);
            var request = new SignupRequestDto
            {
                Email = "test@example.com",
                Username = "testuser",
                Password = ""
            };
            SetupModelValidation(sut, request);
            //Act
            ActionResult<SignupResponseDto> result = await sut.PostAsync(request);
            //Assert
            Assert.False(result.Result is OkObjectResult);
            Assert.Null(context.Users.FirstOrDefault(u => u.Username == request.Username));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Signup_DuplicateUser_Fail()
        {
            //Arrange
            var request = new SignupRequestDto
            {
                Email = "test@example.com",
                Username = "testuser",
                Password = "password123"
            };
            var oldUser = new User()
            {
                Email = request.Email,
                Username = request.Username,
                Password = request.Password,
                FirstName = "",
                LastName = "",
                Level = 0
            };
            GameDevQuestDbContext context = Utility.GetContext();
            JwtTokenHelper jwtTokenHelper = Utility.GetJwtTokenHelper();
            var sut = new SignupController(context, jwtTokenHelper);
            await context.Users.AddAsync(oldUser);
            await context.SaveChangesAsync();
            //Act
            var response = await sut.PostAsync(request);

            //Assert
            Assert.False(response.Result is OkObjectResult);
        }
        private void SetupModelValidation(SignupController sut, SignupRequestDto request)
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
    }
}
