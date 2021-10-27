using Application.Authentication.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.Login
{
    public class LoginCommand : IRequest<string>
    {
        public string Email { get; private set; }

        public string Password { get; private set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;

        public LoginCommandHandler(UserManager<IdentityUser> userManager, TokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
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

            var jwt = await _tokenGenerator.GenerateTokenAsync(user, cancellationToken);

            return jwt;
        }
    }
}