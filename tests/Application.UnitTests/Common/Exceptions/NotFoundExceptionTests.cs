using Application.Common.Exceptions;
using Domain.Models;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions
{
    public class NotFoundExceptionTests
    {
        [Test]
        public void CustomConstructorReturnsAppropriateMessage()
        {
            // Arrange
            var notFoundException = new NotFoundException(nameof(Guitar), 1);

            // Assert
            Assert.AreEqual(notFoundException.Message, "Entity 'Guitar' (1) was not found.");
        }
    }
}