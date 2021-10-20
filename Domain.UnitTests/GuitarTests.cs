using Domain.Enums;
using Domain.Models;
using NUnit.Framework;
using System.Linq;

namespace Domain.UnitTests
{
    public class GuitarTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StringGuitarWithNoPriorStrings()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE");

            // Act
            guitar.String(1, "010", "E");
            guitar.String(2, "013", "B");
            guitar.String(3, "017", "G");
            guitar.String(4, "DY26", "D");
            guitar.String(5, "DY36", "A");
            guitar.String(6, "DY46", "E");

            // Assert
            Assert.AreEqual(6, guitar.GuitarStrings.Count);
        }

        [Test]
        public void ReStringGuitar()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE");

            // Act
            guitar.String(1, "010", "E");
            guitar.String(2, "013", "B");
            guitar.String(3, "017", "G");
            guitar.String(4, "DY26", "D");
            guitar.String(5, "DY36", "A");
            guitar.String(6, "DY46", "E");
            guitar.String(6, "DY46", "D");

            // Assert
            Assert.AreEqual(6, guitar.GuitarStrings.Count);

            var guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 6);
            Assert.AreEqual("D", guitarString.Tuning);
        }

        [Test]
        public void TuneGuitar()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE");

            // Act
            guitar.String(1, "010", "E");
            guitar.String(2, "013", "B");
            guitar.String(3, "017", "G");
            guitar.String(4, "DY26", "D");
            guitar.String(5, "DY36", "A");
            guitar.String(6, "DY46", "E");
            guitar.Tune(1, "D");
            guitar.Tune(2, "A");
            guitar.Tune(6, "D");

            // Assert
            var guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 1);
            Assert.AreEqual("D", guitarString.Tuning);
            guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 2);
            Assert.AreEqual("A", guitarString.Tuning);
            guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 6);
            Assert.AreEqual("D", guitarString.Tuning);
        }
    }
}