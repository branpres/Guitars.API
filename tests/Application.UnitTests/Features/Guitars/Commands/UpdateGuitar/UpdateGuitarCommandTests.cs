using Application.Common.Exceptions;
using Application.Data;
using Application.Features.Guitars.Commands.UpdateGuitar;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tests.Extensions;

namespace Application.UnitTests.Features.Guitars.Commands.UpdateGuitar
{
    public class UpdateGuitarCommandTests
    {
        [Test]
        public async Task ShouldUpdateGuitar()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            guitarDbSet.SetupFindAsync(guitars);
            var guitarContext = new Mock<GuitarsContext>();
            guitarContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var updateGuitarCommand = new UpdateGuitarCommand
            {
                Id = 1,
                Make = "Gibson2",
                Model = "J-46"
            };
            var updateGuitarCommandHandler = new UpdateGuitarCommandHandler(guitarContext.Object);

            // Act
            await updateGuitarCommandHandler.Handle(updateGuitarCommand, new CancellationToken());

            // Assert
            var guitar = guitars.First(x => x.Id == 1);
            Assert.AreEqual("Gibson2", guitar.Make);
            Assert.AreEqual("J-46", guitar.Model);
            guitarContext.Verify(x => x.SaveChangesAsync(new CancellationToken()), Times.Once());
        }

        [Test]
        public void ShouldNotUpdateGuitarButInsteadThrowNotFoundException()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            guitarDbSet.SetupFindAsync(guitars);
            var guitarContext = new Mock<GuitarsContext>();
            guitarContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var updateGuitarCommand = new UpdateGuitarCommand
            {
                Id = 999999
            };
            var updateGuitarCommandHandler = new UpdateGuitarCommandHandler(guitarContext.Object);

            // Act
            Task updateGuitarCommandHandlerDelegate = updateGuitarCommandHandler.Handle(updateGuitarCommand, new CancellationToken());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => updateGuitarCommandHandlerDelegate);
        }
    }
}