using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using CGC.Advent.Core.Helpers;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayOneTests
    {
        private static readonly List<Tuple<int, int>> TestTups = new List<Tuple<int, int>>()
        {
            Tuple.Create(12, 2),
            Tuple.Create(14, 2),
            Tuple.Create(1969, 654),
            Tuple.Create(100756, 33583),
        };

        private static readonly List<Tuple<int, int>> TestTupsTwo = new List<Tuple<int, int>>()
        {
            Tuple.Create(12, 2),
            Tuple.Create(14, 2),
            Tuple.Create(1969, 966),
            Tuple.Create(100756, 50346),
        };

        [TestMethod]
        public void Test_KnownMasses()
        {
            foreach (var testTup in TestTups)
            {
                var fuel = FuelCalculator.CalculateFuelFromMass(testTup.Item1);
                Assert.IsTrue(fuel == testTup.Item2);
            }
        }

        [TestMethod]
        public void Test_KnownMassesAndFuels()
        {
            foreach (var testTup in TestTupsTwo)
            {
                var fuel = FuelCalculator.CalculateFuelForMassAndFuel(testTup.Item1);
                Assert.IsTrue(fuel == testTup.Item2);
            }
        }

        [TestMethod]
        public void Test_DayOne_PartOne()
        {
            //< Parse the test module masses into memory
            var testFile = Path.Combine(TestHelper.TestDir, @"Day1.Input.txt");
            var masses = File.ReadLines(testFile).Where(line => line != "").Select(line => int.Parse(line));
            //< Calculate the fuel needs for each module
            var fuelNeeds = masses.Select(mass => FuelCalculator.CalculateFuelFromMass(mass));
            //< Calculate the total sum of fuel requirements
            var totalFuel = fuelNeeds.Sum();

            Assert.IsTrue(totalFuel == 3511949);
        }

        [TestMethod]
        public void Test_DayOne_PartTwo()
        {
            //< Parse the test module masses into memory
            var testFile = Path.Combine(TestHelper.TestDir, @"Day1.Input.txt");
            var masses = File.ReadLines(testFile).Where(line => line != "").Select(line => int.Parse(line));
            //< Calculate the fuel needs for each module
            var fuelNeeds = masses.Select(mass => FuelCalculator.CalculateFuelForMassAndFuel(mass));
            //< Calculate the total sum of fuel requirements
            var totalFuel = fuelNeeds.Sum();

            Assert.IsTrue(totalFuel == 5265045);
        }
    }
}
