using Domain.Enums;
using Domain.Models;
using NUnit.Framework;
using System.Linq;

namespace Domain.UnitTests.Models
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
            
            var guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 1);
            Assert.AreEqual("010", guitarString.Gauge);
            Assert.AreEqual("E", guitarString.Tuning);

            guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 2);
            Assert.AreEqual("013", guitarString.Gauge);
            Assert.AreEqual("B", guitarString.Tuning);

            guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 3);
            Assert.AreEqual("017", guitarString.Gauge);
            Assert.AreEqual("G", guitarString.Tuning);

            guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 4);
            Assert.AreEqual("DY26", guitarString.Gauge);
            Assert.AreEqual("D", guitarString.Tuning);

            guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 5);
            Assert.AreEqual("DY36", guitarString.Gauge);
            Assert.AreEqual("A", guitarString.Tuning);

            guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 6);
            Assert.AreEqual("DY46", guitarString.Gauge);
            Assert.AreEqual("E", guitarString.Tuning);

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
            guitar.String(6, "DY48", "D");

            // Assert
            Assert.AreEqual(6, guitar.GuitarStrings.Count);

            var guitarString = guitar.GuitarStrings.FirstOrDefault(x => x.Number == 6);
            Assert.AreEqual("DY48", guitarString.Gauge);
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