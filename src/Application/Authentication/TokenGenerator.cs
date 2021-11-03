using Application.Authentication.Exceptions;
using Application.Authentication.Extensions;
using Application.Data;
using Application.Data.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public TokenGenerator(
            IConfiguration configuration,
            GuitarsContext guitarsContext,
            TokenValidationParameters tokenValidationParameters)
        {
            _configuration = configuration;
            _guitarsContext = guitarsContext;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<TokenGenerationResult> GenerateTokenAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var jwtConfiguration = _configuration.GetSection("JwtConfiguration");

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration["Key"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // add claims
                Subject = new ClaimsIdentity(GetClaims(claimsPrincipal)),
                Expires = DateTime.UtcNow.AddMinutes(TOKEN_EXPIRE_TIME_IN_MINUTES),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.UtcNow,
                Issuer = jwtConfiguration["Issuer"]
            };

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var jwt = jwtSecurityTokenHandler.WriteToken(token);

            var authToken = new AuthToken
            {
                UserId = claimsPrincipal.GetUserId(),
                JwtId = token.Id,
                RefreshTokenExpiresOn = DateTime.UtcNow.AddMinutes(REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES)
            };
            await _guitarsContext.AuthToken.AddAsync(authToken);
            await _guitarsContext.SaveChangesAsync(cancellationToken);

            return new TokenGenerationResult
            {
                Token = token,
                Jwt = jwt,
            };
        }

        public async Task<TokenGenerationResult> RefreshTokenAsync(string jwt, CancellationToken cancellationToken)
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
                        throw new TokenValidationException("Invalid token. Please login again to receive a new token.");
                    }
                }

                var exp = long.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiresOn = ConvertUnixTimeToDateTime(exp);

                if (DateTime.UtcNow > expiresOn)
                {
                    throw new TokenValidationException("Token has expired. Please login again to receive a new token.");
                }

                // retrieve info from database to refresh
                var authToken = _guitarsContext.AuthToken.FirstOrDefault(x => x.UserId == claimsPrincipal.GetUserId() && x.JwtId == validatedToken.Id);
                if (authToken == null)
                {
                    throw new TokenValidationException("Token not found");
                }

                if (!authToken.IsUsable)
                {
                    throw new TokenValidationException("Token no longer usable. Please login again to receive a new token.");
                }

                if (authToken.IsRevoked)
                {
                    throw new TokenValidationException("Token has been revoked");
                }

                authToken.IsUsable = false;
                await _guitarsContext.SaveChangesAsync(cancellationToken);

                // validations have passed, so we can generate a new token for the user
                return await GenerateTokenAsync(claimsPrincipal, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static DateTime ConvertUnixTimeToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
        }

        private static List<Claim> GetClaims(ClaimsPrincipal claimsPrincipal)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(claimsPrincipal.Claims);

            return claims;
        }        
    }

    public class TokenGenerationResult
    {
        public SecurityToken Token { get; set; }

        public string Jwt { get; set; }
    }
}