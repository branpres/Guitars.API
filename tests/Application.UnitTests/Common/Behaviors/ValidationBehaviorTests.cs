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
            var createGuitar = new CreateGuitar();
            var validators = new List<CreateGuitarValidator> { new CreateGuitarValidator() };
            var validationBehavior = new ValidationBehavior<CreateGuitar, int>(validators);

            // Act
            Task<int> requestHandlerDelegate()
            {
                var guitarContext = new Mock<GuitarsContext>();
                var createGuitarHandler = new CreateGuitarHandler(guitarContext.Object);
                return createGuitarHandler.Handle(createGuitar, new CancellationToken());
            }

            // Assert
            Assert.ThrowsAsync<ValidationException>(() => validationBehavior.Handle(createGuitar, new CancellationToken(), requestHandlerDelegate));
        }

        [Test]
        public void ShouldNotThrowValidationException()
        {
            // Arrange
            var createGuitar = new CreateGuitar
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };
            var validators = new List<CreateGuitarValidator> { new CreateGuitarValidator() };
            var validationBehavior = new ValidationBehavior<CreateGuitar, int>(validators);

            // Act
            Task<int> requestHandlerDelegate()
            {
                var guitarDbSet = new List<Guitar>().AsQueryable().BuildMockDbSet();
                var guitarContext = new Mock<GuitarsContext>();
                guitarContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

                var createGuitarHandler = new CreateGuitarHandler(guitarContext.Object);
                return createGuitarHandler.Handle(createGuitar, new CancellationToken());
            }

            // Assert
            Assert.DoesNotThrowAsync(() => validationBehavior.Handle(createGuitar, new CancellationToken(), requestHandlerDelegate));
        }
    }
}