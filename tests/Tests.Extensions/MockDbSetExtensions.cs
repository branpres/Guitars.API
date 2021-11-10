using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Extensions;

public static class MockDbSetExtensions
{
    /// <summary>
    /// Sets up the DbSet.Find method as it does not work properly when DbContext is mocked.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mockedDbSet"></param>
    /// <param name="queryableData"></param>
    public static void SetupFind<T>(this Mock<DbSet<T>> mockedDbSet, List<T> queryableData) where T : class
    {
        mockedDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] ids) =>
        {
            var id = (int)ids[0];
            return queryableData.FirstOrDefault(x => (int)x.GetType().GetProperty("Id").GetValue(x) == id);
        });
    }

    /// <summary>
    /// Sets up the DbSet.FindAsync method as it does not work properly when DbContext is mocked.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mockedDbSet"></param>
    /// <param name="queryableData"></param>
    public static void SetupFindAsync<T>(this Mock<DbSet<T>> mockedDbSet, List<T> queryableData) where T : class
    {
        mockedDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) =>
        {
            var id = (int)ids[0];
            return queryableData.FirstOrDefault(x => (int)x.GetType().GetProperty("Id").GetValue(x) == id);
        });
    }
}