using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.Logout;
using Application.Authentication.Commands.RefreshToken;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class AuthenticationTests : TestBase
    {
        [TestCase("admin", "guitarsAdmin1!")]
        [TestCase("readonlyuser", "guitarsReadonlyuser1!")]
        public async Task ShouldLoginAsync(string userName, string password)
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Act
            var loginCommand = new LoginCommand { UserName = userName, Password = password };
            var response = await client.PostAsJsonAsync("/authentication/login", loginCommand);
            var jwt = await response.Content.ReadFromJsonAsync<string>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(jwt);
        }

        [Test]
        public async Task ShouldReceiveBadRequestWithNoLoginCredentialsAsync()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/authentication/login", new LoginCommand());

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task ShouldReceiveBadRequestWithInvalidLoginAsync()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Act
            var loginCommand = new LoginCommand { UserName = "username", Password = "password" };
            var response = await client.PostAsJsonAsync("/authentication/login", loginCommand);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task ShouldLogoutAsync()
        {
            // Arrange
            var client = await GetHttpClientAsync();

            // Act
            var response = await client.PostAsJsonAsync("/authentication/logout", new LogoutCommand());

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task ShouldRefreshTokenAsync()
        {
            // Arrange
            var client = await GetHttpClientAsync();

            // Act
            var response = await client.PostAsJsonAsync("/authentication/refreshtoken", new RefreshTokenCommand());
            var jwt = await response.Content.ReadFromJsonAsync<string>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(jwt);
        }
    }
}