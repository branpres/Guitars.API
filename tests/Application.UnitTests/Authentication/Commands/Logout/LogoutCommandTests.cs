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
}