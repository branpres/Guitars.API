namespace Application.UnitTests.Features.Guitars.Commands.StringGuitar;

public class StringGuitarCommandValidatorTests
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

        var stringGuitarCommand = new StringGuitarCommand(1, new List<StringDto> { new StringDto { Number = 6, Gauge = "DY48", Tuning = "E" } });

        var validator = new StringGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(stringGuitarCommand);

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

        var stringGuitarCommand = new StringGuitarCommand(1, null);

        var validator = new StringGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(stringGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Strings collection cannot be null.", validationResult.Errors[0].ErrorMessage);
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

        var stringGuitarCommand = new StringGuitarCommand(1, new List<StringDto>());

        var validator = new StringGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(stringGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Strings collection cannot be empty.", validationResult.Errors[0].ErrorMessage);
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

        var stringGuitarCommand = new StringGuitarCommand(1, new List<StringDto> { new StringDto { Number = 60, Gauge = "DY48", Tuning = "E" } });

        var validator = new StringGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(stringGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Strings collection has invalid numbers.", validationResult.Errors[0].ErrorMessage);
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

        var stringGuitarCommand = new StringGuitarCommand(1, new List<StringDto> { new StringDto { Number = 0 } });

        var validator = new StringGuitarCommandValidator(guitarsContext.Object);

        // Act
        var validationResult = validator.Validate(stringGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        Assert.AreEqual("Number must be greater than 0.", validationResult.Errors[0].ErrorMessage);
        Assert.AreEqual("Gauge is required.", validationResult.Errors[1].ErrorMessage);
        Assert.AreEqual("Tuning is required.", validationResult.Errors[2].ErrorMessage);

        // Arrange
        stringGuitarCommand = new StringGuitarCommand(1, new List<StringDto> { new StringDto { Number = 6, Gauge = "DY46", Tuning = "42" } });

        validator = new StringGuitarCommandValidator(guitarsContext.Object);

        // Act
        validationResult = validator.Validate(stringGuitarCommand);

        // Assert
        Assert.IsNotEmpty(validationResult.Errors);
        var stringToCheck = "Tuning must be one of the following values";
        Assert.AreEqual(stringToCheck, validationResult.Errors[0].ErrorMessage.Substring(0, stringToCheck.Length));
    }
}