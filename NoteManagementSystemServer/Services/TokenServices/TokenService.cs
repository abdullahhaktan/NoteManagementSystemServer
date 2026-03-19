using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NoteManagemenSystemServer.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteManagementSystemServer.Services.TokenServices
{
    public class TokenService(UserManager<AppUser> userManager, IConfiguration configuration) : ITokenService
    {
        public async Task<string> CreateTokenAsync(AppUser user)
        {
            // Build the claims list — these are the pieces of info embedded inside the token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FirstName+" "+user.LastName),
                new Claim("UserId", user.Id.ToString()) // SENİN PROJENİN KALBİ BURASI!
            };

            // Fetch user roles and add each one as a separate claim
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Sign the token with our secret key using HMAC-SHA512
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(double.Parse(configuration["Jwt:ExpireInMinutes"])),
                SigningCredentials = creds,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            // Create and return the token as a string
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}