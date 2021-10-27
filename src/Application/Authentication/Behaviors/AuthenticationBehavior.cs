using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Behaviors
{
    //public class AuthenticationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    //{
    //    private readonly HttpContext _context;
    //    private readonly TokenGenerator _tokenGenerator;

    //    public AuthenticationBehavior(HttpContext context)
    //    {
    //        _context = context;
    //    }

    //    // handle refreshing the token if not expired
    //    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    //    {
    //        var userId = _context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
    //        var jwt = _context.Request.Headers.Authorization.First().Replace("Bearer ", string.Empty);


    //    }
    //}
}