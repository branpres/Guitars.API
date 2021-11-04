using Application.Authentication.Exceptions;
using Application.Authentication.Extensions;
using Application.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly GuitarsContext _guitarsContext;

        public LogoutCommandHandler(
            IHttpContextAccessor contextAccessor,
            SignInManager<IdentityUser> signInManager,
            GuitarsContext guitarsContext)
        {
            _contextAccessor = contextAccessor;
            _signInManager = signInManager;
            _guitarsContext = guitarsContext;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var claimsPrincipal = _contextAccessor.HttpContext.User;

            var authToken = await _guitarsContext.AuthToken.FirstOrDefaultAsync(x => x.JwtId == claimsPrincipal.GetJwtId());
            if (authToken == null)
            {
                throw new TokenValidationException("Token not found");
            }

            authToken.IsUsable = false;
            await _guitarsContext.SaveChangesAsync(cancellationToken);

            await _signInManager.SignOutAsync();

            return Unit.Value;
        }
    }
}