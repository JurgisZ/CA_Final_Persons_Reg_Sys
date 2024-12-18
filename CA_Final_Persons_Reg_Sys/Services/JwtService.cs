using CA_Final_Persons_Reg_Sys.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CA_Final_Persons_Reg_Sys.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetJwtToken(string username, string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var secretToken = _configuration.GetSection("Jwt:SecretKey").Value;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretToken));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),   //TOKEN VALIDITY DURATION!
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
