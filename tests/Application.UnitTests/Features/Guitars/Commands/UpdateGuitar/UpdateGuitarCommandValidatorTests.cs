using Application.Features.Guitars.Commands.UpdateGuitar;
using NUnit.Framework;

namespace Application.UnitTests.Features.Guitars.Commands.UpdateGuitar
{
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

        [Test]
        public void ShouldNotValidate()
        {
            // Arrange
            var updateGuitarCommand = new UpdateGuitarCommand();

            var validator = new UpdateGuitarCommandValidator();

            // Act
            var validationResult = validator.Validate(updateGuitarCommand);

            // Assert
            Assert.IsNotEmpty(validationResult.Errors);
            Assert.AreEqual("Id is invalid.", validationResult.Errors[0].ErrorMessage);
            Assert.AreEqual("Make is required.", validationResult.Errors[1].ErrorMessage);
            Assert.AreEqual("Model is required.", validationResult.Errors[2].ErrorMessage);
        }
    }
}