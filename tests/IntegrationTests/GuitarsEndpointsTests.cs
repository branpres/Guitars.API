using Application.Features.Guitars.Commands.CreateGuitar;
using Domain.Enums;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class GuitarsEndpointsTests : TestsBase
    {
        [Test]
        public async Task ShouldCreateGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new GuitarsApplication();
            var client = guitarsApplication.CreateClient();

            var createGuitarCommand = new CreateGuitarCommand
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };

            // Act
            var response = await client.PostAsJsonAsync("/guitars", createGuitarCommand);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}