using Application.Common.Exceptions;
using Application.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Commands.Login
{
    public class LoginCommand : IRequest<string>
    {
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; private set; }

        public string Password { get; private set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly GuitarsContext _guitarsContext;

        public LoginCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new InvalidLoginException();
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new InvalidLoginException();
            }

            var tokenGenerator = new TokenGenerator(_configuration, _guitarsContext);
            var jwt = await tokenGenerator.GenerateTokenAsync(user.Id, cancellationToken);

            return jwt;
        }
    }
}