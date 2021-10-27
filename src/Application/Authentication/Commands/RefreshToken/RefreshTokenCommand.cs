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
        private readonly HttpContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenCommandHandler(HttpContext context, UserManager<IdentityUser> userManager, TokenGenerator tokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = _context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var jwt = _context.Request.Headers.Authorization.First().Replace("Bearer ", string.Empty);

            var refreshJwt = await _tokenGenerator.RefreshTokenAsync(user, jwt, cancellationToken);

            return refreshJwt;
        }
    }
}