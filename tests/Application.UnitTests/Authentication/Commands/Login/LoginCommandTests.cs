namespace Application.UnitTests.Authentication.Commands.Login;

public class LoginCommandTests
{
    [Test]
    public void ShouldThrowInvalidLoginExceptionIfUserNotFound()
    {
        // Arrange
        var userManager = AuthenticationTestsHelper.GetMockedUserManager();
        userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);
        var signInManager = AuthenticationTestsHelper.GetMockedSignInManager();
        var guitarsContext = new Mock<GuitarsContext>();
        var tokenGenerator = AuthenticationTestsHelper.GetMockedTokenGenerator(guitarsContext);

        var loginCommand = new LoginCommand { UserName = "test", Password = "password" };
        var loginCommandHandler = new LoginCommandHandler(userManager.Object, signInManager.Object, guitarsContext.Object, tokenGenerator.Object);

        // Act
        Task loginCommandHandlerDelegate = loginCommandHandler.Handle(loginCommand, new CancellationToken());

        // Assert
        Assert.ThrowsAsync<InvalidLoginException>(() => loginCommandHandlerDelegate);
    }

    [Test]
    public void ShouldThrowUserLockedOutException()
    {
        // Arrange
        var userManager = AuthenticationTestsHelper.GetMockedUserManager();
        userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => new IdentityUser());
        var signInManager = AuthenticationTestsHelper.GetMockedSignInManager();
        signInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<IdentityUser>(), "password", It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.LockedOut);
        var guitarsContext = new Mock<GuitarsContext>();
        var tokenGenerator = AuthenticationTestsHelper.GetMockedTokenGenerator(guitarsContext);

        var loginCommand = new LoginCommand { UserName = "test", Password = "password" };
        var loginCommandHandler = new LoginCommandHandler(userManager.Object, signInManager.Object, guitarsContext.Object, tokenGenerator.Object);

        // Act
        Task loginCommandHandlerDelegate = loginCommandHandler.Handle(loginCommand, new CancellationToken());

        // Assert
        Assert.ThrowsAsync<UserLockedOutException>(() => loginCommandHandlerDelegate);
    }

    [Test]
    public void ShouldThrowInvalidLoginException()
    {
        // Arrange
        var userManager = AuthenticationTestsHelper.GetMockedUserManager();
        userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => new IdentityUser());
        var signInManager = AuthenticationTestsHelper.GetMockedSignInManager();
        signInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<IdentityUser>(), "password", It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Failed);
        var guitarsContext = new Mock<GuitarsContext>();
        var tokenGenerator = AuthenticationTestsHelper.GetMockedTokenGenerator(guitarsContext);

        var loginCommand = new LoginCommand { UserName = "test", Password = "password" };
        var loginCommandHandler = new LoginCommandHandler(userManager.Object, signInManager.Object, guitarsContext.Object, tokenGenerator.Object);

        // Act
        Task loginCommandHandlerDelegate = loginCommandHandler.Handle(loginCommand, new CancellationToken());

        // Assert
        Assert.ThrowsAsync<InvalidLoginException>(() => loginCommandHandlerDelegate);
    }
}