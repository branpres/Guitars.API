using Application.Authentication.Exceptions;
using Application.Authentication.Extensions;
using Application.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Behaviors
{
    public class AuthenticationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly GuitarsContext _guitarsContext;

        public AuthenticationBehavior(IHttpContextAccessor contextAccessor, GuitarsContext guitarsContext)
        {
            _contextAccessor = contextAccessor;
            _guitarsContext = guitarsContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var claimsPrincipal = _contextAccessor.HttpContext.User;
            if (claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                var authToken = await _guitarsContext.AuthToken.FirstOrDefaultAsync(x => x.JwtId == claimsPrincipal.GetJwtId());
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
            }

            return await next();
        }
    }
}