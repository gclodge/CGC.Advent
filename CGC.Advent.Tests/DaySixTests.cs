using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DaySixTests
    {
        //< I'm lazy right now, get at me
        public static readonly string TestDir = @"D:\_dev_test\CGC.Advent";

        [TestMethod]
        public void Test_KnownOrbits()
        {
            //< From: https://adventofcode.com/2019/day/6
            var testFile = Path.Combine(TestDir, @"Day6.KnownOrbits.txt");

            //< Parse the orbits
            var orbits = File.ReadLines(testFile).Select(x => new Core.Classes.Orbit(x)).ToList();

            //< Count the direct and indirect orbits
            var totalOrbits = Core.Helpers.OrbitHelper.CountTotalOrbits(orbits);

            Assert.IsTrue(totalOrbits == 42);
        }

        [TestMethod]
        public void Test_DaySix_PartOne()
        {
            //< From: https://adventofcode.com/2019/day/6
            var testFile = Path.Combine(TestDir, @"Day6.Input.txt");

            //< Parse the orbits
            var orbits = File.ReadLines(testFile).Select(x => new Core.Classes.Orbit(x)).ToList();

            //< Count the direct and indirect orbits
            var totalOrbits = Core.Helpers.OrbitHelper.CountTotalOrbits(orbits);

            Assert.IsTrue(totalOrbits == 344238);
        }

        [TestMethod]
        public void Test_KnownOrbitalTransfers()
        {
            //< From: https://adventofcode.com/2019/day/6
            var testFile = Path.Combine(TestDir, @"Day6.KnownTransfers.txt");

            //< Parse the orbits
            var orbits = File.ReadLines(testFile).Select(x => new Core.Classes.Orbit(x)).ToList();

            //< Get the minimal orbital transfers from source to target
            var sourceBody = "YOU";
            var targetBody = "SAN";
            var xfers = Core.Helpers.OrbitHelper.GetOrbitalTransfers(sourceBody, targetBody, orbits);

            Assert.IsTrue(xfers == 4);
        }

        [TestMethod]
        public void Test_DaySix_PartTwo()
        {
            //< From: https://adventofcode.com/2019/day/6
            var testFile = Path.Combine(TestDir, @"Day6.Input.txt");

            //< Parse the orbits
            var orbits = File.ReadLines(testFile).Select(x => new Core.Classes.Orbit(x)).ToList();

            //< Get the minimal orbital transfers from source to target
            var sourceBody = "YOU";
            var targetBody = "SAN";
            var xfers = Core.Helpers.OrbitHelper.GetOrbitalTransfers(sourceBody, targetBody, orbits);

            Assert.IsTrue(xfers == 436);
        }
    }
}
