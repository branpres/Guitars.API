using Application.Common.Exceptions;
using Application.Data;
using Application.Features.Guitars.Commands.DeleteGuitar;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.Guitars.Commands.DeleteGuitar
{
    public class DeleteGuitarCommandTests
    {
        [Test]
        public async Task ShouldDeleteGuitar()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var deleteGuitarCommand = new DeleteGuitarCommand(1);
            var deleteGuitarCommandHandler = new DeleteGuitarCommandHandler(guitarsContext.Object);

            // Act
            await deleteGuitarCommandHandler.Handle(deleteGuitarCommand, new CancellationToken());

            // Assert
            var guitar = guitars.First(x => x.Id == 1);
            Assert.AreEqual(true, guitar.IsDeleted);
            
            foreach (var guitarString in guitar.GuitarStrings)
            {
                Assert.AreEqual(true, guitarString.IsDeleted);
            }

            guitarsContext.Verify(x => x.SaveChangesAsync(new CancellationToken()), Times.Once());
        }

        [Test]
        public void ShouldNotDeleteGuitarButInsteadThrowNotFoundException()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var deleteGuitarCommand = new DeleteGuitarCommand(999999);
            var deleteGuitarCommandHandler = new DeleteGuitarCommandHandler(guitarsContext.Object);

            // Act
            Task deleteGuitarCommandHandlerDelegate = deleteGuitarCommandHandler.Handle(deleteGuitarCommand, new CancellationToken());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => deleteGuitarCommandHandlerDelegate);
        }
    }
}