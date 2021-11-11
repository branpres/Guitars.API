namespace Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    [Test]
    public void ShouldThrowValidationException()
    {
        // Arrange
        var createGuitarCommand = new CreateGuitarCommand();
        var validators = new List<CreateGuitarCommandValidator> { new CreateGuitarCommandValidator() };
        var validationBehavior = new ValidationBehavior<CreateGuitarCommand, int>(validators);

        // Act and Assert
        Assert.ThrowsAsync<ValidationException>(() => validationBehavior.Handle(createGuitarCommand, new CancellationToken(), async () => { return 1; } ));
    }

    [Test]
    public void ShouldNotThrowValidationException()
    {
        // Arrange
        var createGuitarCommand = new CreateGuitarCommand
        {
            GuitarType = GuitarType.AcousticElectric,
            MaxNumberOfStrings = 6,
            Make = "Taylor",
            Model = "314-CE"
        };
        var validators = new List<CreateGuitarCommandValidator> { new CreateGuitarCommandValidator() };
        var validationBehavior = new ValidationBehavior<CreateGuitarCommand, int>(validators);

        // Act and Assert
        Assert.DoesNotThrowAsync(() => validationBehavior.Handle(createGuitarCommand, new CancellationToken(), async () => { return 1; }));
    }
}