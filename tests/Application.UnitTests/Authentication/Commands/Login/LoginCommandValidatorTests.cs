namespace Application.UnitTests.Authentication.Commands.Login;

public class LoginCommandValidatorTests
{
    [Test]
    public void ShouldValidate()
    {
        // Arrange
        var loginCommand = new LoginCommand
        {
            UserName = "test",
            Password = "password"
        };

        var validator = new LoginCommandValidator();

        // Act
        var validationResult = validator.Validate(loginCommand);

        // Assert
        Assert.IsEmpty(validationResult.Errors);
    }

    [Test]
    public void ShouldNotValidate()
    {
        // Arrange
        var loginCommand = new LoginCommand();

        var validator = new LoginCommandValidator();

        // Act
        var validationResult = validator.Validate(loginCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("UserName is required.", validationResult.Errors[0].ErrorMessage);
        Assert.AreEqual("Password is required.", validationResult.Errors[1].ErrorMessage);
    }
}