using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayFourteenTests
    {
        private static readonly List<Tuple<string, int>> KnownReactions = new List<Tuple<string, int>>()
        {
            Tuple.Create(Path.Combine(TestHelper.TestDir, "Day14.Test0.txt"), 165),
            Tuple.Create(Path.Combine(TestHelper.TestDir, "Day14.Test1.txt"), 13312),
            Tuple.Create(Path.Combine(TestHelper.TestDir, "Day14.Test2.txt"), 180697),
            Tuple.Create(Path.Combine(TestHelper.TestDir, "Day14.Test3.txt"), 2210736),
        };

        private static readonly List<Tuple<string, int>> KnownMaxFuels = new List<Tuple<string, int>>()
        {
            Tuple.Create(Path.Combine(TestHelper.TestDir, "Day14.Test1.txt"), 82892753),
            Tuple.Create(Path.Combine(TestHelper.TestDir, "Day14.Test2.txt"), 5586022),
            Tuple.Create(Path.Combine(TestHelper.TestDir, "Day14.Test3.txt"), 460664),
        };

        [TestMethod]
        public void Test_KnownReactions()
        {
            var fuel = new Chemical("FUEL", 1);
            foreach (var testTup in KnownReactions)
            {
                var fact = new NanoFactory(testTup.Item1);
                fact.ProduceChemical(fuel);
                Assert.IsTrue(fact.OreRequired == testTup.Item2);
            }
        }

        [TestMethod]
        public void Test_KnownMaxFuels()
        {
            foreach (var testTup in KnownMaxFuels)
            {
                long oreAvailable = 1000000000000;
                var fuel = NanoFactoryHelper.FindMaximumFuelWithOre(testTup.Item1, oreAvailable);
                Assert.IsTrue(fuel == testTup.Item2);
            }
        }

        [TestMethod]
        public void Test_DayFourteen_PartOne()
        {
            var fact = new NanoFactory(Path.Combine(TestHelper.TestDir, "Day14.Input.txt"));
            var fuel = new Chemical("FUEL", 1);
            fact.ProduceChemical(fuel);

            Assert.IsTrue(fact.OreRequired == 899155);
        }

        [TestMethod]
        public void Test_DayFourteen_PartTwo()
        {
            var src = Path.Combine(TestHelper.TestDir, "Day14.Input.txt");
            long oreAvailable = 1000000000000;
            var fuel = NanoFactoryHelper.FindMaximumFuelWithOre(src, oreAvailable);

            Assert.IsTrue(fuel == 2390226);
        }
    }
}
