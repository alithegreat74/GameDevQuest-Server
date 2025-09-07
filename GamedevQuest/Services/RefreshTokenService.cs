using GamedevQuest.Helpers;
using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Models;
using GamedevQuest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Services
{
    public class RefreshTokenService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RefreshTokenRepository _refreshTokenRepository;
        private readonly TokenHelper _tokenHelper;
        public RefreshTokenService(UnitOfWork unitOfWork, RefreshTokenRepository refreshTokenRepository, TokenHelper tokenHelper)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenHelper = tokenHelper;
        }
        public async Task<OperationResult<RefreshToken>> CreateToken(int userId, string ipAddress)
        {
            await _unitOfWork.StartTransaction();
            RefreshToken refreshToken = _tokenHelper.GenerateRefreshToken(userId, ipAddress);
            await _refreshTokenRepository.CreateRefreshToken(refreshToken);
            await _unitOfWork.CommitChanges();
            return new OperationResult<RefreshToken>(refreshToken);
        }
        public async Task<OperationResult<bool>> RemoveToken(int userId, string ipAddress)
        {
            await _unitOfWork.StartTransaction();
            await _refreshTokenRepository.DeleteRefreshToken(userId, ipAddress);
            await _unitOfWork.CommitChanges();
            return new OperationResult<bool>(true);
        }
        public async Task<OperationResult<RefreshToken>> FindRefreshToken(string token)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.FindRefreshTokenNoTracking(token);
            if (refreshToken == null)
                return new OperationResult<RefreshToken>(new NotFoundObjectResult("Could not find you token"));
            return new OperationResult<RefreshToken>(refreshToken);
        }
    }
}
