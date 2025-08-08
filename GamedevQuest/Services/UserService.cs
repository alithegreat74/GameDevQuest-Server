using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly UnitOfWork _unitOfWork;
        public UserService(UserRepository userRepository, UnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult<User>> CompleteUserInfo(string email, CompleteInfoRequestDto info)
        {
            await _unitOfWork.StartTransaction();
            User? user = await _userRepository.FindUserByEmail(email);
            if (user == null)
                return new OperationResult<User>(new NotFoundObjectResult($"Couldn't find user with email: {email}"));
            User? userWithSameUsername = await _userRepository.FindUserByUsernameNoTracking(info.Username);
            if (userWithSameUsername != null && user.Id != userWithSameUsername.Id)
                return new OperationResult<User>(new ConflictObjectResult($"A user with the same username exists"));
            user.UpdateInfo(info);
            await _unitOfWork.CommitChanges();
            return new OperationResult<User>(user);
        }
        public async Task<OperationResult<User>> AddUserXp(string email, int xp, int lessonId)
        {
            await _unitOfWork.StartTransaction();
            User? user = await _userRepository.FindUserByEmail(email);
            if (user == null)
            {
                await _unitOfWork.RollBackChanges();
                return new OperationResult<User>(new NotFoundObjectResult($"Could not find user with email: {email}"));
            } 
            if(!TryAddSolvedLesson(user, lessonId))
                return new OperationResult<User>(new ConflictObjectResult($"The user has already done this lesson"));
            user.UpdateXp(xp);
            await _unitOfWork.CommitChanges();
            return new OperationResult<User>(user);
        }
        private bool TryAddSolvedLesson(User user, int lessonId)
        {
            user.SolvedLessons ??= new List<int>();
            if (user.SolvedLessons.Contains(lessonId))
                return false;
            user.SolvedLessons.Add(lessonId);
            return true;
        }
    }
}
