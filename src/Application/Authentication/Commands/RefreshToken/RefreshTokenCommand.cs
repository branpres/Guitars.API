namespace Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<string>
{
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TokenGenerator _tokenGenerator;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public RefreshTokenCommandHandler(IHttpContextAccessor contextAccessor, TokenGenerator tokenGenerator, TokenValidationParameters tokenValidationParameters)
    {
        _contextAccessor = contextAccessor;
        _tokenGenerator = tokenGenerator;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwt = _contextAccessor.HttpContext.Request.Headers.Authorization.First().Replace("Bearer ", string.Empty);
        var tokenGenerationResult = await _tokenGenerator.RefreshTokenAsync(jwt, _tokenValidationParameters, cancellationToken);
        return tokenGenerationResult.Jwt;
    }
}