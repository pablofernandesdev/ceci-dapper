using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service responsible for generating JWT tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration used to access JWT token settings.</param>
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token based on the provided user details.
        /// </summary>
        /// <param name="model">The object containing the user details.</param>
        /// <returns>The generated JWT token.</returns>
        public string GenerateToken(UserResultDTO model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Get the JWT token secret key from the configuration
            var key = Encoding.ASCII.GetBytes(_configuration["JwtToken:Secret"]);

            // Set the token information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(model.UserId)),
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, model.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create the JWT token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the JWT token as a string
            return tokenHandler.WriteToken(token);
        }
    }
}
