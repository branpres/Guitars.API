namespace Application.UnitTests.Authentication.Commands.Login;

public class LoginCommandValidatorTests
{
    [Test]
    public void ShouldValidate()
    {
        // Arrange
        var loginCommand = new LoginCommand { UserName = "test", Password = "password" };

        var validator = new LoginCommandValidator();

        // Act
        var validationResult = validator.Validate(loginCommand);

        // Assert
        Assert.IsEmpty(validationResult.Errors);
    }

    [TestCase("", "")]
    [TestCase("test", "")]
    [TestCase("", "password")]
    public void ShouldNotValidate(string userName, string password)
    {
        // Arrange
        var loginCommand = new LoginCommand { UserName = userName, Password = password };

        var validator = new LoginCommandValidator();

        // Act
        var validationResult = validator.Validate(loginCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
    }
}