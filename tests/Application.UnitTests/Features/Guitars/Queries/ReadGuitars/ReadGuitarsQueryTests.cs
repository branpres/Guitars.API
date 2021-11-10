namespace Application.UnitTests.Features.Guitars.Queries.ReadGuitars;

public class ReadGuitarsQueryTests
{
    [Test]
    public async Task ShouldReadAllGuitars()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery();
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.AreEqual(3, guitarsVM.Guitars.Count);
    }

    [Test]
    public async Task ShouldReadAllGuitarsExceptDeleted()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        guitars.Add(new Guitar(GuitarType.Electric, 6, "Gibson", "SG") { Id = 4, IsDeleted = true });
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery();
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.AreEqual(3, guitarsVM.Guitars.Count);
    }

    [Test]
    public async Task ShouldReturnEmptyCollectionBecauseThereAreNoGuitars()
    {
        // Arrange
        var guitarDbSet = new List<Guitar>().AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery();
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.IsEmpty(guitarsVM.Guitars);
    }

    [Test]
    public async Task ShouldReadGuitarsPaged()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery(null, 1, 2);
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.AreEqual(2, guitarsVM.Guitars.Count);
    }

    [Test]
    public async Task ShouldReadOnlyAcousticGuitars()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery("Acoustic");
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.AreEqual(1, guitarsVM.Guitars.Count);
    }

    [Test]
    public async Task ShouldReadOnlySixStringGuitars()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        guitars.Add(new Guitar(GuitarType.Acoustic, 12, "Gibson", "12") { Id = 4 });
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery("6");
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.AreEqual(3, guitarsVM.Guitars.Count);
    }

    [Test]
    public async Task ShouldReadOnlyGibsonGuitars()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery("Gibson");
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.AreEqual(2, guitarsVM.Guitars.Count);
    }

    [Test]
    public async Task ShouldReadOnlyStratocasterGuitars()
    {
        // Arrange
        var guitars = GuitarsTestsHelper.GetGuitars();
        var guitarDbSet = guitars.AsQueryable().BuildMockDbSet();
        var guitarsContext = new Mock<GuitarsContext>();
        guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

        var readGuitarQuery = new ReadGuitarsQuery("Strat");
        var readGuitarQueryHandler = new ReadGuitarsQueryHandler(guitarsContext.Object);

        // Act
        var guitarsVM = await readGuitarQueryHandler.Handle(readGuitarQuery, new CancellationToken());

        // Assert
        Assert.IsNotNull(guitarsVM);
        Assert.AreEqual(1, guitarsVM.Guitars.Count);
    }
}