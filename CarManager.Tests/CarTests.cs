using CarManager;
using NUnit.Framework;
using System;

namespace Tests
{
    public class CarTests
    {
        private const string make = "VW";
        private const string model = "Golf";
        private const double fuelConsumption = 0.07;
        private const double fuelCapacity = 50;
        private Car car;

        [SetUp]
        public void Setup()
        {
            car = new Car(make, model, fuelConsumption, fuelCapacity);
        }

        [Test]
        public void Constructor_WithValidParameters_ShouldSetAllFieldsCorrectly()
        {
            bool actual = false;
            if (car.Make == make && car.Model == model && car.FuelConsumption == fuelConsumption && car.FuelCapacity == fuelCapacity && car.FuelAmount == 0)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
        [TestCase("")]
        [TestCase(null)]
        public void Constructor_WithEmptyOrNullMakeParameter_ShouldThrowArgumentException(string make)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Car(make, model, fuelConsumption, fuelCapacity));
            StringAssert.Contains("Make cannot be null or empty", ex.Message);
        }
        [TestCase("")]
        [TestCase(null)]
        public void Constructor_WithEmptyModelParameter_ShouldThrowArgumentException(string model)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Car(make, model, fuelConsumption, fuelCapacity));
            StringAssert.Contains("Model cannot be null or empty", ex.Message);
        }
        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_WithZeroOrNegativeFuelConsumption_ShouldThrowArgumentException(double fuelConsumption)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Car(make, model, fuelConsumption, fuelCapacity));
            StringAssert.Contains("Fuel consumption cannot be zero or negative", ex.Message);
        }
        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_WithZeroOrNegativeFuelCapacity_ShouldThrowArgumentException(double fuelCapacity)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Car(make, model, fuelConsumption, fuelCapacity));
            StringAssert.Contains("Fuel capacity cannot be zero or negative", ex.Message);
        }
        [TestCase(0)]
        [TestCase(-1)]
        public void Refuel_WithZeroOrNegativeParameter_ShoulThrowArgumentException(double fuel)
        {
            var ex = Assert.Throws<ArgumentException>(() => car.Refuel(fuel));
            StringAssert.Contains("Fuel amount cannot be zero or negative", ex.Message);
        }
        [Test]
        public void Refuel_WithValidParameter_FuelAmountIsUpdatedCorrectly()
        {
            car.Refuel(28.6);
            var actual = car.FuelAmount;
            Assert.AreEqual(28.6, actual);
        }
        [Test]
        public void Refuel_WhenFuelAmountIsGreaterThanFuelCapacity_FuelAmountShouldBeSetToFuelCapacity()
        {
            car.Refuel(58);
            var actual = car.FuelAmount;
            Assert.AreEqual(fuelCapacity, actual);
        }
        [Test]
        public void Drive_EnoughFuelToDriveTheDistance_FuelAmountShouldBeUpdated()
        {
            double distance = 4500; // in meters
            car.Refuel(40);            
            var expected = car.FuelAmount - (distance / 100) * fuelConsumption;
            car.Drive(distance);
            var actual = car.FuelAmount;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Drive_NotEnoughFuelToDriveTheDistance_ShouldThrowInvalidOperationException()
        {
            double distance = 15000; // in meters
            car.Refuel(5);
            var ex = Assert.Throws<InvalidOperationException>(() => car.Drive(distance));
            StringAssert.Contains("You don't have enough fuel to drive", ex.Message);
        }
    }
}