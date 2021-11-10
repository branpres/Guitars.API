namespace Application.UnitTests.Features.Guitars.Commands.CreateGuitar;

public class CreateGuitarCommandValidatorTests
{
    [Test]
    public void ShouldValidate()
    {
        // Arrange
        var createGuitarCommand = new CreateGuitarCommand
        {
            GuitarType = GuitarType.AcousticElectric,
            MaxNumberOfStrings = 6,
            Make = "Taylor",
            Model = "314-CE"
        };

        var validator = new CreateGuitarCommandValidator();

        // Act
        var validationResult = validator.Validate(createGuitarCommand);

        // Assert
        Assert.IsEmpty(validationResult.Errors);
    }

    [Test]
    public void ShouldNotValidateWithEmptyCommand()
    {
        // Arrange
        var createGuitarCommand = new CreateGuitarCommand();

        var validator = new CreateGuitarCommandValidator();

        // Act
        var validationResult = validator.Validate(createGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual(4, validationResult.Errors.Count);
    }

    [TestCase(1, 6, "test", null)]
    [TestCase(1, 6, null, null)]
    [TestCase(1, 0, null, null)]
    [TestCase(4, 0, null, null)]
    public void ShouldNotValidateWithInvalidCommand(int guitarType, int maxNumberOfStrings, string make, string model)
    {
        // Arrange
        var createGuitarCommand = new CreateGuitarCommand
        {
            GuitarType = (GuitarType)guitarType,
            MaxNumberOfStrings = maxNumberOfStrings,
            Make = make,
            Model = model
        };

        var validator = new CreateGuitarCommandValidator();

        // Act
        var validationResult = validator.Validate(createGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
    }
}