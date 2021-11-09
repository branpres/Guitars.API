using Application.Features.Guitars.Commands.CreateGuitar;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class AuthorizationTests : TestBase
    {
        [Test]
        public async Task ShouldBeAuthorizedToAccessResourceWithReadPolicy()
        {
            // Arrange
            var client = await GetHttpClientForReadOnlyUserAsync();

            // Act
            var response = await client.GetAsync("/guitars/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldBeAuthorizedToAccessResourceWithWritePolicy()
        {
            // Arrange
            var client = await GetHttpClientAsync();

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

        [Test]
        public async Task ShouldBeForbiddenFromResource()
        {
            // Arrange
            var client = await GetHttpClientForReadOnlyUserAsync();

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
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        public async Task ShouldBeUnauthorizedToAccessResource()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/guitars/");

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}