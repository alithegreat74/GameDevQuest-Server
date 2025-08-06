using GamedevQuest.Helpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;

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
        public async Task<(User? user, string errorMessage)> ValidateUserLogin(LoginRequestDto request)
        {
            var userMatch = await _repository.FindUserByUsernameNoTracking(request.Username);
            if (userMatch == null || !_passwordHelper.VerifyPassword(userMatch.Password, request.Password))
                return (null, "No User found");

            return (userMatch, "");
        }
    }
}
