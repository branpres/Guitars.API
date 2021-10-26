﻿using Application.Features.Guitars.Commands.CreateGuitar;
using Application.Features.Guitars.Commands.StringGuitar;
using Application.Features.Guitars.Commands.TuneGuitar;
using Application.Features.Guitars.Commands.UpdateGuitar;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class GuitarsEndpointsTests : TestBase
    {
        [Test]
        public async Task ShouldCreateGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
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

        [Test]
        public async Task ShouldReceiveBadRequestResultWhenTryingToCreateGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            var createGuitarCommand = new CreateGuitarCommand { };

            // Act
            var response = await client.PostAsJsonAsync("/guitars", createGuitarCommand);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task ShouldReadGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            var createGuitarCommand = new CreateGuitarCommand
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };
            var response = await client.PostAsJsonAsync("/guitars", createGuitarCommand);
            var location = response.Headers.Location.OriginalString;

            // Act
            response = await client.GetAsync(location);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldReceiveNotFoundResultWhenTryingToReadGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            // Act
            var response = await client.GetAsync("/guitars/0");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task ShouldReadGuitarsAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            // Act
            var response = await client.GetAsync("/guitars");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldUpdateGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            var createGuitarCommand = new CreateGuitarCommand
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };
            var response = await client.PostAsJsonAsync("/guitars", createGuitarCommand);
            var id = Convert.ToInt32(await response.Content.ReadAsStringAsync());

            // Act
            response = await client.PutAsJsonAsync("/guitars", new UpdateGuitarCommand { Id = id, Make = "New", Model = "New" });
             
            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task ShouldReceiveNotFoundResultWhenTryingToUpdateGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            // Act
            var response = await client.PutAsJsonAsync("/guitars", new UpdateGuitarCommand { Id = 0, Make = "New", Model = "New" });

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task ShouldDeleteGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            var createGuitarCommand = new CreateGuitarCommand
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };
            var response = await client.PostAsJsonAsync("/guitars", createGuitarCommand);
            var id = await response.Content.ReadAsStringAsync();

            // Act
            response = await client.DeleteAsync($"/guitars/{id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task ShouldReceiveNotFoundResultWhenTryingToDeleteGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            // Act
            var response = await client.DeleteAsync($"/guitars/0");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task ShouldStringGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            var createGuitarCommand = new CreateGuitarCommand
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };
            var response = await client.PostAsJsonAsync("/guitars", createGuitarCommand);
            var id = Convert.ToInt32(await response.Content.ReadAsStringAsync());

            // Act
            response = await client.PostAsJsonAsync("/guitars/string", new StringGuitarCommand(id, new List<StringDto>
            {
                new StringDto{ Number = 6, Gauge = "DY46", Tuning = "E" }
            }));

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task ShouldReceiveNotFoundResultWhenTryingToStringGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/guitars/string", new StringGuitarCommand(0, new List<StringDto>
            {
                new StringDto{ Number = 6, Gauge = "DY46", Tuning = "E" }
            }));

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task ShouldTuneGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            var createGuitarCommand = new CreateGuitarCommand
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };
            var response = await client.PostAsJsonAsync("/guitars", createGuitarCommand);
            var id = Convert.ToInt32(await response.Content.ReadAsStringAsync());

            // Act
            response = await client.PostAsJsonAsync("/guitars/tune", new TuneGuitarCommand(id, new List<TuningDto>
            {
                new TuningDto{ Number = 6, Tuning = "E" }
            }));

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task ShouldReceiveNotFoundResultWhenTryingToTuneGuitarAsync()
        {
            // Arrange
            var guitarsApplication = new WebApplicationFactory<Program>();
            var client = guitarsApplication.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/guitars/tune", new TuneGuitarCommand(0, new List<TuningDto>
            {
                new TuningDto{ Number = 6, Tuning = "E" }
            }));

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}