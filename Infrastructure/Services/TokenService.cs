using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));


        }
public string CreateToken(AppUser user)
{
    // Claims: Associate user data with the token
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.GivenName, user.DisplayName)
    };

    // Validate the key is correct and long enough
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));

    // Define signing credentials
    var creds = new SigningCredentials(key, SecurityAlgorithms.Aes128CbcHmacSha256);

    // Define the token descriptor
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(7), // Use UtcNow for expiration
        SigningCredentials = creds,
        Issuer = _config["Token:Issuer"] // Ensure this is set in appsettings.json
    };

    // Create the token
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    // Return the serialized token
    return tokenHandler.WriteToken(token);
}

    }
}
