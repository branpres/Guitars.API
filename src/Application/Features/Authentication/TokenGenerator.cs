using Application.Data;
using Application.Features.Authentication.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Features.Authentication
{
    internal class TokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly GuitarsContext _guitarsContext;

        private const int TOKEN_EXPIRE_TIME_IN_MINUTES = 30;
        private const int REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES = 120;

        internal TokenGenerator(IConfiguration configuration, GuitarsContext guitarsContext)
        {
            _configuration = configuration;
            _guitarsContext = guitarsContext;
        }

        internal async Task<string> GenerateTokenAsync(string userId, CancellationToken cancellationToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfiguration")["Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // add claims
                //Subject = new ClaimsIdentity(new[]
                //{
                //    new Claim("Id", user.Id),
                //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //}),
                Expires = DateTime.UtcNow.AddMinutes(TOKEN_EXPIRE_TIME_IN_MINUTES),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var jwt = jwtSecurityTokenHandler.WriteToken(token);

            await _guitarsContext.AuthToken.AddAsync(new AuthToken
            {
                UserId = userId,
                JwtId = token.Id,
                Token = jwt,
                IsUsable = true,
                IsRevoked = false,
                ExpiresOn = DateTime.UtcNow.AddMinutes(REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES)
            });
            await _guitarsContext.SaveChangesAsync(cancellationToken);

            return jwt;
        }
    }
}