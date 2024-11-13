using System.Text;
using System.Security.Claims;
using ECommerceShopApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerceShopApi.Utils {

    public class JwtTokenGenerator {

        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration) {

            _configuration = configuration;
        }



        public string GenerateToken(ApplicationUser user, IList<string> roles) {

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? throw new ArgumentException("UserName")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            foreach (var role in roles) {

                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentException("Key")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}