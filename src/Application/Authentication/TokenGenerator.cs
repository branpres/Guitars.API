using Application.Authentication.Exceptions;
using Application.Data;
using Application.Data.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Authentication
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly GuitarsContext _guitarsContext;
        private readonly TokenValidationParameters _tokenValidationParameters;

        private const int TOKEN_EXPIRE_TIME_IN_MINUTES = 30;
        private const int REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES = 120;

        internal TokenGenerator(IConfiguration configuration, GuitarsContext guitarsContext, TokenValidationParameters tokenValidationParameters)
        {
            _configuration = configuration;
            _guitarsContext = guitarsContext;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<string> GenerateTokenAsync(string userId, CancellationToken cancellationToken)
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

            var authToken = new AuthToken
            {
                UserId = userId,
                JwtId = token.Id,
                RefreshTokenExpiresOn = DateTime.UtcNow.AddMinutes(REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES)
            };
            await _guitarsContext.AuthToken.AddAsync(authToken);
            await _guitarsContext.SaveChangesAsync(cancellationToken);

            return jwt;
        }

        public async Task<string> RefreshTokenAsync(string userId, string jwt)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // validate current token
                var claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(jwt, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        throw new TokenValidationException("Invalid token");
                    }
                }

                var exp = long.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiresOn = ConvertUnixTimeToDateTime(exp);

                if (DateTime.UtcNow > expiresOn)
                {
                    throw new TokenValidationException("Token has expired");
                }

                // retrieve info from database to refresh
                return "";
            }
            catch (Exception ex)
            {
                throw new TokenValidationException("Invalid token", ex);
            }
        }

        private static DateTime ConvertUnixTimeToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
        }
    }
}