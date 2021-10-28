using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<string>
    {        
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenCommandHandler(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager, TokenGenerator tokenGenerator)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var jwt = _contextAccessor.HttpContext.Request.Headers.Authorization.First().Replace("Bearer ", string.Empty);

            var refreshJwt = await _tokenGenerator.RefreshTokenAsync(user, jwt, cancellationToken);

            return refreshJwt;
        }
    }
}