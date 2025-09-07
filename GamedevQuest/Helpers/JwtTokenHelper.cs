using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamedevQuest.Helpers
{
    public class JwtTokenHelper
    {
        public IConfiguration Config { get; private set; }

        public JwtTokenHelper(IConfiguration config)
        {
            Config = config;
        }
        public string GenerateToken(string email)
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
    }
}
