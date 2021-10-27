using Application.Common.Behaviors;
using Application.Data;
using Application.Features.Guitars.Commands.CreateGuitar;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Common.Behaviors
{
    public class ValidationBehaviorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            // Arrange
            var createGuitarCommand = new CreateGuitarCommand();
            var validators = new List<LoginCommandValidator> { new CreateGuitarCommandValidator() };
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
            var validators = new List<LoginCommandValidator> { new CreateGuitarCommandValidator() };
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
}