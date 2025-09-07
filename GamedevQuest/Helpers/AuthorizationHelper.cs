using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GamedevQuest.Helpers
{
    public class AuthorizationHelper
    {
        private readonly JwtTokenHelper _jwtTokenHelper;
        public static readonly string AuthorizationKey = "jwt_token";
        public AuthorizationHelper(JwtTokenHelper jwtTokenHelper)
        {
            _jwtTokenHelper = jwtTokenHelper;
        }

        public void SavePlayerSession(string email, HttpResponse response)
        {
            string token = _jwtTokenHelper.GenerateToken(email);
            var cookieOption = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(double.TryParse(_jwtTokenHelper.Config["jwt:ExpireMinutes"], out double minutes)?minutes:0)
            };
            response.Cookies.Append(AuthorizationKey, token, cookieOption);
        }

        public Task AuthorizeCookies(MessageReceivedContext context)
        {
            if (context.Request.Cookies.ContainsKey(AuthorizationKey))
            {
                context.Token = context.Request.Cookies[AuthorizationKey];
            }
            return Task.CompletedTask;
        }
    }
}
