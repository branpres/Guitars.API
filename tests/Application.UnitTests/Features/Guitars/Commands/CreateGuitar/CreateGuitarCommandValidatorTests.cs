using Application.Features.Guitars.Commands.CreateGuitar;
using Domain.Enums;
using NUnit.Framework;

namespace Application.UnitTests.Features.Guitars.Commands.CreateGuitar
{
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
        public void ShouldNotValidate()
        {
            // Arrange
            var createGuitarCommand = new CreateGuitarCommand();

            var validator = new CreateGuitarCommandValidator();

            // Act
            var validationResult = validator.Validate(createGuitarCommand);

            // Assert
            Assert.IsNotEmpty(validationResult.Errors);
        }
    }
}