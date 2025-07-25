using GamedevQuest.Helpers;
using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;

namespace GamedevQuest.Services
{
    public class UserSignupService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserRepository _userRepository;
        private readonly PasswordHelper _passwordHelper;
        public UserSignupService(UserRepository userRepository, UnitOfWork unitOfWork, PasswordHelper passwordHelper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHelper = passwordHelper;
        }
        public async Task<(bool canCreate,string errorMessage)> CanCreateUser(SignupRequestDto dto)
        {
            User? duplicateUser = await _userRepository.FindUser(dto.Username, dto.Email);
            if (duplicateUser != default(User))
                return new (false, "A user already exists with the same username or email");
            return new (true, "");
        }
        public async Task<User> CreateUser(SignupRequestDto dto)
        {
            await _unitOfWork.StartTransaction();
            var newUser = new User
            {
                Email = dto.Email,
                Username = dto.Username,
                Password = "",
                FirstName = "",
                LastName = ""
            };
            string hashedPassword = _passwordHelper.HashPassword(dto.Password);
            newUser.Password = hashedPassword;
            await _userRepository.AddUser(newUser);
            await _unitOfWork.CommitChanges();
            return newUser;
        }
    }
}
