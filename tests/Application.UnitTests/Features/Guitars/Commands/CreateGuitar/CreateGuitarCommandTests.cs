﻿using Application.Data;
using Application.Features.Guitars.Commands.CreateGuitar;
using Domain.Enums;
using Domain.Models;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.Guitars.Commands.CreateGuitar
{
    public class CreateGuitarCommandTests
    {
        [Test]
        public async Task ShouldCreateGuitar()
        {
            // Arrange
            var guitarDbSet = new List<Guitar>().AsQueryable().BuildMockDbSet();
            var guitarsContext = new Mock<GuitarsContext>();
            guitarsContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            var createGuitarCommand = new CreateGuitarCommand
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };

            var createGuitarCommandHandler = new CreateGuitarCommandHandler(guitarsContext.Object);

            // Act
            await createGuitarCommandHandler.Handle(createGuitarCommand, new CancellationToken());

            // Assert
            guitarsContext.Verify(x => x.Guitar.AddAsync(It.IsAny<Guitar>(), new CancellationToken()), Times.Once());
            guitarsContext.Verify(x => x.SaveChangesAsync(new CancellationToken()), Times.Once());
        }
    }
}