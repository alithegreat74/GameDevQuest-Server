using GamedevQuest.Models;
using GamedevQuest.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Helpers
{
    public class AuthorizationHelper
    {
        private readonly TokenHelper _jwtTokenHelper;
        private readonly RefreshTokenService _refreshTokenService;
        public static readonly string JwtAuthorizationKey = "jwt_token";
        public static readonly string RefreshAuthorizationKey = "refresh_token";
        public AuthorizationHelper(TokenHelper jwtTokenHelper, RefreshTokenService refreshTokenService)
        {
            _jwtTokenHelper = jwtTokenHelper;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<OperationResult<bool>> SaveUserSession(int userId, string email, string ipAddress, HttpResponse response)
        {
            OperationResult<RefreshToken> refreshToken = await _refreshTokenService.CreateToken(userId, ipAddress);
            if (refreshToken.Result == null)
                return new OperationResult<bool>(refreshToken.ActionResultObject);
            AddJwtToken(email, response);
            var RefreshTokenCookieOption = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.Result.ExpiresAt
            };
            response.Cookies.Append(RefreshAuthorizationKey, refreshToken.Result.Token, RefreshTokenCookieOption);
            return new OperationResult<bool>(true);
        }
        public async Task<OperationResult<bool>> RemoveUserSession(int userId, string ipAddress, HttpResponse response)
        {
            OperationResult<bool> refreshTokenRemoval = await _refreshTokenService.RemoveToken(userId, ipAddress);
            if (refreshTokenRemoval.Result == null)
                return new OperationResult<bool>(refreshTokenRemoval.ActionResultObject);
            response.Cookies.Delete(JwtAuthorizationKey);
            response.Cookies.Delete(RefreshAuthorizationKey);
            return new OperationResult<bool>(true);
        }
        public async Task<OperationResult<RefreshToken>> FindUserSession(HttpRequest request)
        {
            KeyValuePair<string, string> refreshToken = request.Cookies.FirstOrDefault(pair => pair.Key == RefreshAuthorizationKey);
            if (refreshToken.Equals(default(KeyValuePair<string, string>)))
                return new OperationResult<RefreshToken>(new NotFoundObjectResult("couldn't find a refresh token"));
            return await _refreshTokenService.FindRefreshToken(refreshToken.Value);
        }
        public void RefreshUserSession(string email, HttpResponse response)
        {
            response.Cookies.Delete(JwtAuthorizationKey);
            AddJwtToken(email, response);
        }
        private void AddJwtToken(string email, HttpResponse response)
        {
            string jwtToken = _jwtTokenHelper.GenerateJwtToken(email);
            var jwtCookieOption = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(double.TryParse(_jwtTokenHelper.Config["jwt:ExpireMinutes"], out double minutes) ? minutes : 0)
            };
            response.Cookies.Append(JwtAuthorizationKey, jwtToken, jwtCookieOption);
        }
    }
}
