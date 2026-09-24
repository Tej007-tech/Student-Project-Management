using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(SPM_User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,user.UserType.UserTypeName)
            };

            var addedRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role != null && !string.IsNullOrWhiteSpace(userRole.Role.RoleName))
                    {
                        var roleName = userRole.Role.RoleName.Trim();
                        if (addedRoles.Add(roleName))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roleName));
                        }
                    }
                }
            } 

            if (user.UserType != null && !string.IsNullOrWhiteSpace(user.UserType.UserTypeName))
            {
                var userTypeName = user.UserType.UserTypeName.Trim();

                claims.Add(new Claim("UserType", userTypeName));

                if (addedRoles.Add(userTypeName))
                {
                    claims.Add(new Claim(ClaimTypes.Role, userTypeName));


                }

            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expires;

            if (double.TryParse(_config["Jwt:ExpiresInMinutes"], out double minutes))
            {
                expires = DateTime.UtcNow.AddMinutes(minutes);
            }
            else if (double.TryParse(_config["Jwt:ExpirationInDays"], out double days))
            {
                expires = DateTime.UtcNow.AddDays(days);
            }
            else
            {
                expires = DateTime.UtcNow.AddMinutes(10);
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials // the key + algorithm combo used to sign the token
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
