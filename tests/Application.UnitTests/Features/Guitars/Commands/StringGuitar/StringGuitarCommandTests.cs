using Application.Common.Exceptions;
using Application.Data;
using Application.Features.Guitars.Commands.StringGuitar;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.Guitars.Commands.StringGuitar
{
    public class StringGuitarCommandTests
    {
        [Test]
        public async Task ShouldStringGuitar()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var stringGuitarCommand = new StringGuitarCommand(1, new List<StringDto> { new StringDto { Number = 6, Gauge = "DY48", Tuning = "E"} });
            var stringGuitarCommandHandler = new StringGuitarCommandHandler(guitarsContext.Object);

            // Act
            await stringGuitarCommandHandler.Handle(stringGuitarCommand, new CancellationToken());

            // Assert
            guitarsContext.Verify(x => x.SaveChangesAsync(new CancellationToken()), Times.Once());
        }

        [Test]
        public void ShouldNotStringGuitarButInsteadThrowNotFoundException()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var stringGuitarCommand = new StringGuitarCommand(999999, new List<StringDto> { new StringDto { Number = 6, Gauge = "DY48", Tuning = "E" } });
            var stringGuitarCommandHandler = new StringGuitarCommandHandler(guitarsContext.Object);

            // Act
            Task stringGuitarCommandHandlerDelegate = stringGuitarCommandHandler.Handle(stringGuitarCommand, new CancellationToken());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => stringGuitarCommandHandlerDelegate);
        }
    }
}