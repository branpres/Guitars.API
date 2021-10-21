using Application.Common.Exceptions;
using Application.Data;
using Application.Features.Guitars.Queries.ReadGuitar;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.Guitars.Queries.ReadGuitar
{
    public class ReadGuitarQueryTests
    {
        [Test]
        public async Task ShouldReadGuitar()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var readGuitarQuery = new ReadGuitarQuery(1);
            var readGuitarQueryHandler = new ReadGuitarQueryHandler(guitarsContext.Object);

            // Act
            var guitarDto = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

            // Assert
            Assert.IsNotNull(guitarDto);
            Assert.AreEqual(1, guitarDto.Id);
        }

        [Test]
        public void ShouldNotReadGuitarButInsteadThrowNotFoundException()
        {
            // Arrange
            var guitars = GuitarsTestsHelper.GetGuitars();
            var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var readGuitarQuery = new ReadGuitarQuery(999999);
            var readGuitarQueryHandler = new ReadGuitarQueryHandler(guitarsContext.Object);

            // Act
            Task readGuitarQueryHandlerDelegate = readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => readGuitarQueryHandlerDelegate);
        }
    }
}