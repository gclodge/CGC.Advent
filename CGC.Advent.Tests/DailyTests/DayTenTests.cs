using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    class DayTenTest
    {
        public string File { get; set; } = null;
        public Asteroid Asteroid { get; set; } = null;
        public int CountVisible { get; set; } = 0;

        public DayTenTest()
        { }
    }

    [TestClass]
    public class DayTenTests
    {
        private static readonly List<DayTenTest> KnownMaps = new List<DayTenTest>()
        {
            new DayTenTest()
            {
                File = Path.Combine(TestHelper.TestDir, "Day10.Test0.txt"),
                Asteroid = new Asteroid(3, 4),
                CountVisible = 8
            },
            new DayTenTest()
            {
                File = Path.Combine(TestHelper.TestDir, "Day10.Test1.txt"),
                Asteroid = new Asteroid(5, 8),
                CountVisible = 33
            },
            new DayTenTest()
            {
                File = Path.Combine(TestHelper.TestDir, "Day10.Test2.txt"),
                Asteroid = new Asteroid(1, 2),
                CountVisible = 35
            },
            new DayTenTest()
            {
                File = Path.Combine(TestHelper.TestDir, "Day10.Test3.txt"),
                Asteroid = new Asteroid(6, 3),
                CountVisible = 41
            },
            new DayTenTest()
            {
                File = Path.Combine(TestHelper.TestDir, "Day10.Test4.txt"),
                Asteroid = new Asteroid(11, 13),
                CountVisible = 210
            },
        };

        [TestMethod]
        public void Test_KnownMaps()
        {
            foreach (var test in KnownMaps)
            {
                //< Make the map
                var map = new AsteroidMap(test.File);
                //< Get the best asteroid
                var best = map.GetBestAsteroid();
                //< Ensure we ain't fuck up
                Assert.IsTrue(best.Item1.Equals(test.Asteroid));
                Assert.IsTrue(best.Item2 == test.CountVisible);
            }
        }

        [TestMethod]
        public void Test_KnownVapourization()
        {
            var map = new AsteroidMap(Path.Combine(TestHelper.TestDir, "Day10.Test4.txt"));
            var laser = new Asteroid(11, 13);
            var order = map.GetVapourizationOrder(laser);

            var known = new Asteroid(8, 2);
            Assert.IsTrue(order.Last().Equals(known));
        }

        [TestMethod]
        public void Test_DayTen_PartOne()
        {
            var testFile = Path.Combine(TestHelper.TestDir, "Day10.Input.txt");
            var map = new AsteroidMap(testFile);
            var best = map.GetBestAsteroid();

            Assert.IsTrue(best.Item2 == 319);
        }

        [TestMethod]
        public void Test_DayTen_PartTwo()
        {
            var testFile = Path.Combine(TestHelper.TestDir, "Day10.Input.txt");
            var map = new AsteroidMap(testFile);
            var best = map.GetBestAsteroid();

            //< Set the laser at the 'best' point and get the vapourization order
            var order = map.GetVapourizationOrder(best.Item1);

            //< Get the 'test' value
            var last = order.Last();
            var testValue = last.X * 100.0 + last.Y;
            Assert.IsTrue(testValue == 517);
        }
    }
}
