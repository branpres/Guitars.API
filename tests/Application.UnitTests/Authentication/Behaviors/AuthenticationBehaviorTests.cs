

namespace Application.UnitTests.Authentication.Behaviors;

public class AuthenticationBehaviorTests
{
    private readonly string _jwtId = Guid.NewGuid().ToString();

    [Test]
    public void ShouldThrowTokenValidationExceptionBecauseTokenNotFound()
    {
        // Arrange
        var contextAccessor = GetMockedHttpContextAccessor();
        var authTokensDbSet = new List<AuthToken>().AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.AuthToken).Returns(authTokensDbSet.Object);
        var signInManager = AuthenticationTestsHelper.GetMockedSignInManager();
        var authenticationBehavior = new AuthenticationBehavior<LogoutCommand, Unit>(contextAccessor.Object, guitarsContext.Object);

        // Act and Assert
        Assert.ThrowsAsync<TokenValidationException>(() => authenticationBehavior.Handle(new LogoutCommand(), new CancellationToken(), async () => { return Unit.Value; }));
    }

    [Test]
    public void ShouldThrowTokenValidationExceptionBecauseTokenIsNotUsable()
    {
        // Arrange
        var contextAccessor = GetMockedHttpContextAccessor();
        var authTokensDbSet = new List<AuthToken> { new AuthToken { JwtId = _jwtId, IsUsable = false } }.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.AuthToken).Returns(authTokensDbSet.Object);
        var signInManager = AuthenticationTestsHelper.GetMockedSignInManager();
        var authenticationBehavior = new AuthenticationBehavior<LogoutCommand, Unit>(contextAccessor.Object, guitarsContext.Object);

        // Act and Assert
        Assert.ThrowsAsync<TokenValidationException>(() => authenticationBehavior.Handle(new LogoutCommand(), new CancellationToken(), async () => { return Unit.Value; }));
    }

    [Test]
    public void ShouldThrowTokenValidationExceptionBecauseTokenIsRevoked()
    {
        // Arrange
        var contextAccessor = GetMockedHttpContextAccessor();
        var authTokensDbSet = new List<AuthToken> { new AuthToken { JwtId = _jwtId, IsRevoked = true } }.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.AuthToken).Returns(authTokensDbSet.Object);
        var signInManager = AuthenticationTestsHelper.GetMockedSignInManager();
        var authenticationBehavior = new AuthenticationBehavior<LogoutCommand, Unit>(contextAccessor.Object, guitarsContext.Object);

        // Act and Assert
        Assert.ThrowsAsync<TokenValidationException>(() => authenticationBehavior.Handle(new LogoutCommand(), new CancellationToken(), async () => { return Unit.Value; }));
    }

    private Mock<IHttpContextAccessor> GetMockedHttpContextAccessor()
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, _jwtId)
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));

        var contextAccessor = new Mock<IHttpContextAccessor>();
        contextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

        return contextAccessor;
    }
}