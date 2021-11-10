namespace Application.UnitTests.Features.Guitars.Commands.TuneGuitar;

public class TuneGuitarCommandValidatorTests
{
    [Test]
    public void ShouldValidate()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        guitarDbSet.SetupFindAsync(guitars);
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var tuneGuitarCommand = new TuneGuitarCommand(1, new List<TuningDto> { new TuningDto { Number = 6, Tuning = "E" } });

        var validator = new TuneGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(tuneGuitarCommand);

        // Assert
        Assert.IsEmpty(validationResult.Errors);
    }

    [Test]
    public void ShouldNotValidateBecauseOfNullStringsCollection()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        guitarDbSet.SetupFindAsync(guitars);
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var tuneGuitarCommand = new TuneGuitarCommand(1, null);

        var validator = new TuneGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(tuneGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Tunings collection cannot be null.", validationResult.Errors[0].ErrorMessage);
    }

    [Test]
    public void ShouldNotValidateBecauseOfEmptyStringsCollection()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        guitarDbSet.SetupFindAsync(guitars);
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var tuneGuitarCommand = new TuneGuitarCommand(1, new List<TuningDto>());

        var validator = new TuneGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(tuneGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Tunings collection cannot be empty.", validationResult.Errors[0].ErrorMessage);
    }

    [Test]
    public void ShouldNotValidateBecauseOfInvalidStringNumbers()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        guitarDbSet.SetupFindAsync(guitars);
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var tuneGuitarCommand = new TuneGuitarCommand(1, new List<TuningDto> { new TuningDto { Number = 60, Tuning = "E" } });

        var validator = new TuneGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(tuneGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Tunings collection has invalid numbers.", validationResult.Errors[0].ErrorMessage);
    }

    [Test]
    public void ShouldNotValidateBecauseOfInvalidData()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        guitarDbSet.SetupFindAsync(guitars);
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var tuneGuitarCommand = new TuneGuitarCommand(1, new List<TuningDto> { new TuningDto { Number = 0 } });

        var validator = new TuneGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(tuneGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Number must be greater than 0.", validationResult.Errors[0].ErrorMessage);
        Assert.AreEqual("Tuning is required.", validationResult.Errors[1].ErrorMessage);

        // Arrange
        tuneGuitarCommand = new TuneGuitarCommand(1, new List<TuningDto> { new TuningDto { Number = 6, Tuning = "42" } });

        validator = new TuneGuitarCommandValidator(guitarsContext.Object);

        // Act
        validationResult = validator.Validate(tuneGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        var stringToCheck = "Tuning must be one of the following values";
        Assert.AreEqual(stringToCheck, validationResult.Errors[0].ErrorMessage.Substring(0, stringToCheck.Length));
    }
}