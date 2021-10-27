using Application.Authentication.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Authentication.Behaviors
{
    public class AuthenticationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly HttpContext _context;

        public AuthenticationBehavior(HttpContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_context.User.Identity != null && _context.User.Identity.IsAuthenticated)
            {
                return await next();
            }

            throw new UnauthenticatedException();
        }
    }
}