using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using GKS.Core.IServices;

namespace GKS.Service.Services
{
    public class AuthService:IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string username, string[] roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

            // הוספת תפקידים כ-Claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // פונקציה לפירוק טוקן
        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // המפתח לצורך אימות

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // אם הטוקן תקין, מחזירים את המידע מתוך הטוקן (claims)
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // אם הטוקן לא תקין, הוספת טיפול בשגיאות (לא הכרחי, תלוי בצורך שלך)
                if (!(validatedToken is JwtSecurityToken jwtToken) || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception ex)
            {
                // אם הטוקן לא תקין או שקרה משהו, החזרת null או טיפול בשגיאה לפי הצורך
                return null;
            }
        }

        // לדוגמה: פונקציה לשליפת המידע מתוך הטוקן (כגון שם המשתמש)
        public string GetUsernameFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.Identity?.Name;
        }

        // לדוגמה: פונקציה לשליפת תפקידים מתוך הטוקן
        public IEnumerable<string> GetRolesFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindAll(ClaimTypes.Role).Select(c => c.Value);
        }
    }
}


