using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Repositories;

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
        public async Task<User?> FindUser(string username) => await _userRepository.FindUserByUsernameNoTracking(username);
        public async Task<(User?, string)> AddUserXp(string email, int xp, int lessonId)
        {
            await _unitOfWork.StartTransaction();
            User? user = await _userRepository.FindUserByEmail(email);
            if (user == null)
            {
                await _unitOfWork.RollBackChanges();
                return (null,$"Could not find user with email: {email}");
            } 
            if(!TryAddSolvedLesson(user, lessonId))
                return (null, $"The user has already done this lesson");
            user.UpdateXp(xp);
            await _unitOfWork.CommitChanges();
            return (user,"");
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
