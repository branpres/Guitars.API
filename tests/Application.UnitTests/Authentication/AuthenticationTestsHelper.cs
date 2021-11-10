namespace Application.UnitTests.Authentication;

public static class AuthenticationTestsHelper
{
    public static Mock<UserManager<IdentityUser>> GetMockedUserManager()
    {
        var userStore = new Mock<IUserStore<IdentityUser>>();
        return new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    }

    public static Mock<SignInManager<IdentityUser>> GetMockedSignInManager()
    {
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

        return new Mock<SignInManager<IdentityUser>>(GetMockedUserManager().Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
    }

    public static Mock<TokenGenerator> GetMockedTokenGenerator(Mock<GuitarsContext> guitarsContext)
    {
        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();
        configurationSection.Setup(x => x["Key"]).Returns("key");
        configuration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);
        var jwtSecurityTokenHandler = new Mock<JwtSecurityTokenHandler>();

        return new Mock<TokenGenerator>(configuration.Object, guitarsContext.Object, jwtSecurityTokenHandler.Object);
    }
}