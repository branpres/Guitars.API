namespace Application.UnitTests.Authentication.Commands.Logout;

/// <summary>
/// Fail case unit tested with this class. Integration tests cover being able to logout.
/// </summary>
internal class LogoutCommandTests
{
    [Test]
    public void ShouldThrowTokenValidationException()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var contextAccessor = new Mock<IHttpContextAccessor>();
        contextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

        var signInManager = AuthenticationTestsHelper.GetMockedSignInManager();
        var authTokensDbSet = new List<AuthToken>().AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.AuthToken).Returns(authTokensDbSet.Object);

        var logoutCommandHandler = new LogoutCommandHandler(contextAccessor.Object, signInManager.Object, guitarsContext.Object);

        // Act
        Task logoutCommandHandlerDelegate = logoutCommandHandler.Handle(new LogoutCommand(), new CancellationToken());

        // Assert
        Assert.ThrowsAsync<TokenValidationException>(() => logoutCommandHandlerDelegate);
    }
}