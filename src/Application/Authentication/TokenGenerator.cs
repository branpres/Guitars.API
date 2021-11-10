namespace Application.Authentication;

public class TokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly GuitarsContext _guitarsContext;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    private const int TOKEN_EXPIRE_TIME_IN_MINUTES = 30;
    private const int REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES = 120;

    public TokenGenerator(
        IConfiguration configuration,
        GuitarsContext guitarsContext,
        JwtSecurityTokenHandler jwtSecurityTokenHandler)
    {
        _configuration = configuration;
        _guitarsContext = guitarsContext;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public async Task<TokenGenerationResult> GenerateTokenAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
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

        var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        var jwt = _jwtSecurityTokenHandler.WriteToken(token);

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

    public async Task<TokenGenerationResult> RefreshTokenAsync(string jwt, TokenValidationParameters tokenValidationParameters, CancellationToken cancellationToken)
    {
        // validate current token
        var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);

        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        {
            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            if (!result)
            {
                throw new TokenValidationException("Invalid token. Please login again to receive a new token.");
            }
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