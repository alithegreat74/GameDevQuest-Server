using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Services
{
    public class UserLoginService
    {
        private readonly PasswordHelper _passwordHelper;
        private readonly UserRepository _repository;
        public UserLoginService(UserRepository repository, PasswordHelper passwordHelper)
        {
            _passwordHelper = new PasswordHelper();
            _repository = repository;
            _passwordHelper = passwordHelper;
        }
        public async Task<OperationResult<User>> ValidateUserLogin(LoginRequestDto request)
        {
            var userMatch = await _repository.FindUserByEmailOrUsernameNoTracking(request.UserIdentifier);
            if (userMatch == null)
                return new OperationResult<User>(new UnauthorizedObjectResult("Invalid username or password"));

            try
            {
                if (!_passwordHelper.VerifyPassword(userMatch.Password, request.Password))
                {
                    return new OperationResult<User>(new UnauthorizedObjectResult("Invalid username or password"));
                }
            }
            catch(FormatException e)
            {
                return new OperationResult<User>(new UnauthorizedObjectResult("Invalid username or password"));
            }
            return new OperationResult<User>(userMatch);
        }
    }
}
