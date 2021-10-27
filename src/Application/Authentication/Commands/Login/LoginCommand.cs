using Application.Authentication.Exceptions;
using Application.Data;
using Application.Data.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Authentication.Commands.Login
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

        private const int TOKEN_EXPIRE_TIME_IN_MINUTES = 30;
        private const int REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES = 120;

        public LoginCommandHandler(UserManager<IdentityUser> userManager, IConfiguration configuration, GuitarsContext guitarsContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _guitarsContext = guitarsContext;
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

            var jwt = await GenerateTokenAsync(user.Id, cancellationToken);

            return jwt;
        }

        private async Task<string> GenerateTokenAsync(string userId, CancellationToken cancellationToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfiguration")["Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // add claims
                //Subject = new ClaimsIdentity(new[]
                //{
                //    new Claim("Id", user.Id),
                //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //}),
                Expires = DateTime.UtcNow.AddMinutes(TOKEN_EXPIRE_TIME_IN_MINUTES),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var jwt = jwtSecurityTokenHandler.WriteToken(token);

            var authToken = new AuthToken
            {
                UserId = userId,
                JwtId = token.Id,
                RefreshTokenExpiresOn = DateTime.UtcNow.AddMinutes(REFRESH_TOKEN_EXPIRE_TIME_IN_MINUTES)
            };
            await _guitarsContext.AuthToken.AddAsync(authToken);
            await _guitarsContext.SaveChangesAsync(cancellationToken);

            return jwt;
        }
    }
}