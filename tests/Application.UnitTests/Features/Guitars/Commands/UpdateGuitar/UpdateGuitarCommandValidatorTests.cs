namespace Application.UnitTests.Features.Guitars.Commands.UpdateGuitar;

public class UpdateGuitarCommandValidatorTests
{
    [Test]
    public void ShouldValidate()
    {
        // Arrange
        var updateGuitarCommand = new UpdateGuitarCommand
        {
            Id = 1,
            Make = "Taylor",
            Model = "314-CE"
        };

        var validator = new UpdateGuitarCommandValidator();

        // Act
        var validationResult = validator.Validate(updateGuitarCommand);

        // Assert
        Assert.IsEmpty(validationResult.Errors);
    }

    [TestCase(null, null)]
    [TestCase("test", null)]
    [TestCase(null, "test")]
    public void ShouldNotValidate(string make, string model)
    {
        // Arrange
        var updateGuitarCommand = new UpdateGuitarCommand { Make = make, Model = model };

        var validator = new UpdateGuitarCommandValidator();

        // Act
        var validationResult = validator.Validate(updateGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
    }
}