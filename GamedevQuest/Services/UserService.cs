using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Models.DTO;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

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
        public async Task<OperationResult<User>> GetUserByEmail(string email)
        {
            User? user = await _userRepository.FindUserByEmailNoTracking(email);
            if (user == null)
                return new OperationResult<User>(new NotFoundObjectResult($"Could not find user"));

            return new OperationResult<User>(user);
        }
        public async Task<OperationResult<User>> GetUserById(int userId)
        {
            User? user = await _userRepository.FindUserByIdNoTracking(userId);
            if (user == null)
                return new OperationResult<User>(new NotFoundObjectResult($"Could not find user"));

            return new OperationResult<User>(user);
        }
        public async Task<OperationResult<User>> CompleteUserInfo(User user, CompleteInfoRequestDto info)
        {
            await _unitOfWork.StartTransaction();
            try
            {
                User? userWithSameUsername = await _userRepository.FindUserByUsernameNoTracking(info.Username);
                if (userWithSameUsername != null && user.Id != userWithSameUsername.Id)
                    return new OperationResult<User>(new ConflictObjectResult($"A user with the same username exists"));
                user.UpdateInfo(info.Username, info.FirstName, info.LastName);
                await _unitOfWork.CommitChanges();
                return new OperationResult<User>(user);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackChanges();
                return new OperationResult<User>(new UnprocessableEntityObjectResult(ex.Message));
            }
        }
        public async Task<OperationResult<User>> GetUserFromCookie(ClaimsPrincipal user)
        {
            string? email = user.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email))
                return new OperationResult<User>(new NotFoundObjectResult("There was a problem in retrieving the user data"));
            return await GetUserByEmail(email);
        }
        public async Task<OperationResult<User>> AddUserXp(User user, int xp)
        {
            await _unitOfWork.StartTransaction();
            user.UpdateXp(xp);
            await _unitOfWork.CommitChanges();
            return new OperationResult<User>(user);
        }
        public async Task<OperationResult<bool>> AddSolvedTests(User user, IEnumerable<TestSolveAttempt> attempts)
        {
            try
            {
                await _unitOfWork.StartTransaction();
                user.SolvedTests ??= new List<int>();
                HashSet<int> existingTests = user.SolvedTests.ToHashSet();
                IEnumerable<int> newIds = attempts.Select(attempts => attempts.Id).Where(attempts => !existingTests.Contains(attempts)).Distinct();
                user.SolvedTests.AddRange(newIds);
                await _unitOfWork.CommitChanges();
                return new OperationResult<bool>(true);
            }
            catch
            {
                await _unitOfWork.RollBackChanges();
                return new OperationResult<bool>(false);
            }
        }
    }
}
