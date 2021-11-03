using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<string>
    {        
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenCommandHandler(IHttpContextAccessor contextAccessor, TokenGenerator tokenGenerator)
        {
            _contextAccessor = contextAccessor;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {            
            var jwt = _contextAccessor.HttpContext.Request.Headers.Authorization.First().Replace("Bearer ", string.Empty);
            var tokenGenerationResult = await _tokenGenerator.RefreshTokenAsync(jwt, cancellationToken);
            return tokenGenerationResult.Jwt;
        }
    }
}