using Application.Common.Exceptions;
using Application.Data;
using Application.Features.Guitars.Commands.TuneGuitar;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.Guitars.Commands.TuneGuitar
{
    public class TuneGuitarCommandTests
    {
        [Test]
        public async Task ShouldTuneGuitar()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var tuneGuitarCommand = new TuneGuitarCommand(1, new List<TuningDto> { new TuningDto { Number = 6, Tuning = "E" } });
            var tuneGuitarCommandHandler = new TuneGuitarCommandHandler(guitarsContext.Object);

            // Act
            await tuneGuitarCommandHandler.Handle(tuneGuitarCommand, new CancellationToken());

            // Assert
            guitarsContext.Verify(x => x.SaveChangesAsync(new CancellationToken()), Times.Once());
        }

        [Test]
        public void ShouldNotTuneGuitarButInsteadThrowNotFoundException()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var tuneGuitarCommand = new TuneGuitarCommand(999999, new List<TuningDto> { new TuningDto { Number = 6, Tuning = "E" } });
            var tuneGuitarCommandHandler = new TuneGuitarCommandHandler(guitarsContext.Object);

            // Act
            Task tuneGuitarCommandHandlerDelegate = tuneGuitarCommandHandler.Handle(tuneGuitarCommand, new CancellationToken());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => tuneGuitarCommandHandlerDelegate);
        }
    }
}