using Application.Authentication.Exceptions;
using Application.Authentication.Extensions;
using Application.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.Login
{
    public class LoginCommand : IRequest<string>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly GuitarsContext _guitarsContext;
        private readonly TokenGenerator _tokenGenerator;

        public LoginCommandHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, GuitarsContext guitarsContext, TokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _guitarsContext = guitarsContext;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new InvalidLoginException();
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (signInResult.IsLockedOut)
            {
                throw new UserLockedOutException();
            }
            else if (!signInResult.Succeeded)
            {
                throw new InvalidLoginException();
            }

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var tokenGenerationResult = await _tokenGenerator.GenerateTokenAsync(claimsPrincipal, cancellationToken);

            _guitarsContext.UserLogins.Add(new IdentityUserLogin<string>
            {
                LoginProvider = "Identity",
                ProviderKey = tokenGenerationResult.Token.Id,
                ProviderDisplayName = claimsPrincipal.GetUserName(),
                UserId = user.Id
            });
            await _guitarsContext.SaveChangesAsync(cancellationToken);

            return tokenGenerationResult.Jwt;
        }
    }
}