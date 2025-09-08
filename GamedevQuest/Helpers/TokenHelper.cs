using GamedevQuest.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GamedevQuest.Helpers
{
    public class TokenHelper
    {
        public IConfiguration Config { get; private set; }

        public TokenHelper(IConfiguration config)
        {
            Config = config;
        }
        public string GenerateJwtToken(string email)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: Config["jwt:Issuer"],
                audience: Config["jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(Config["jwt:ExpireMinutes"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public RefreshToken GenerateRefreshToken(int userId, string ipAddress)
        {
            byte[] randomNumber = RandomNumberGenerator.GetBytes(64);
            string token = Convert.ToBase64String(randomNumber);
            double expireTime = double.TryParse(Config["RefreshToken:ExpiresMinutes"], out double minutes) ? minutes : 0;
            return new RefreshToken(userId, token, ipAddress, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(expireTime));
        }
    }
}
