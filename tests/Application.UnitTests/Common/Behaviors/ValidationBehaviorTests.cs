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

        // Act
        Task<int> createGuitarHandlerDelegate()
        {
            var guitarContext = new Mock<GuitarsContext>();
            var createGuitarCommandHandler = new CreateGuitarCommandHandler(guitarContext.Object);
            return createGuitarCommandHandler.Handle(createGuitarCommand, new CancellationToken());
        }

        // Assert
        Assert.ThrowsAsync<ValidationException>(() => validationBehavior.Handle(createGuitarCommand, new CancellationToken(), createGuitarHandlerDelegate));
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

        // Act
        Task<int> createGuitarHandlerDelegate()
        {
            var guitarDbSet = new List<Guitar>().AsQueryable().BuildMockDbSet();
            var guitarContext = new Mock<GuitarsContext>();
            guitarContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var createGuitarCommandHandler = new CreateGuitarCommandHandler(guitarContext.Object);
            return createGuitarCommandHandler.Handle(createGuitarCommand, new CancellationToken());
        }

        // Assert
        Assert.DoesNotThrowAsync(() => validationBehavior.Handle(createGuitarCommand, new CancellationToken(), createGuitarHandlerDelegate));
    }
}