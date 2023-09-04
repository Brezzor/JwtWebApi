using JwtWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtWebApi.Helpers
{
        public class Authenticator
        {
                private readonly IConfiguration _configuration;

                public Authenticator(IConfiguration configuration)
                {
                        _configuration = configuration;
                }

                public string? CreateToken(User user)
                {
                        var issuer = _configuration["JWT:ValidIssuer"];

                        var audience = _configuration["JWT:ValidAudience"];

                        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

                        var expires = DateTime.Now.AddHours(24);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration!["JWT:Secret"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                        var token = new JwtSecurityToken(
                            issuer: issuer,
                            audience: audience,
                            claims: claims,
                            expires: expires,
                            signingCredentials: credentials
                            );

                        return new JwtSecurityTokenHandler().WriteToken(token);
                }

                public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
                {
                        using (var hmac = new HMACSHA512())
                        {
                                passwordSalt = hmac.Key;
                                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                        }
                }

                public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
                {
                        using (var hmac = new HMACSHA512(passwordSalt))
                        {
                                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                                return computedHash.SequenceEqual(passwordHash);
                        }
                }
        }
}
