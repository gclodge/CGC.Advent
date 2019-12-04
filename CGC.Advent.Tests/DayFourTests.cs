using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using CGC.Advent.Core.Classes;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayFourTests
    {
        private static readonly List<Tuple<string, bool>> TestTups = new List<Tuple<string, bool>>()
        {
            Tuple.Create("111111", true),
            Tuple.Create("223450", false),
            Tuple.Create("123789", false)
        };

        private static readonly List<Tuple<string, bool>> ContstrainedTestTups = new List<Tuple<string, bool>>()
        {
            Tuple.Create("112233", true),
            Tuple.Create("123444", false),
            Tuple.Create("111122", true),
        };

        [TestMethod]
        public void Test_KnownPasswords()
        {
            //< Test that the part-one known passwords pass
            foreach (var testTup in TestTups)
            {
                var pass = new ElvenPassword(testTup.Item1);
                var isGood = pass.IsGood();
                Assert.IsTrue(isGood == testTup.Item2);
            }

            //< Test that the part-two known passwords pass
            foreach (var testTup in ContstrainedTestTups)
            {
                var pass = new ElvenPassword(testTup.Item1);
                var isGood = pass.IsGood(requireAdjacentPair: true);
                Assert.IsTrue(isGood == testTup.Item2);
            }
        }

        [TestMethod]
        public void Test_DayFour_PartOne()
        {
            int rangeMin = 353096;
            int rangeMax = 843212;

            var goodPasses = ElvenPassword.GetGoodPasswordsInRange(rangeMin, rangeMax);

            Assert.IsTrue(goodPasses.Count == 579);
        }

        [TestMethod]
        public void Test_DayFour_PartTwo()
        {
            int rangeMin = 353096;
            int rangeMax = 843212;

            var goodPasses = ElvenPassword.GetGoodPasswordsInRange(rangeMin, rangeMax, requireAdjacentPair: true);

            Assert.IsTrue(goodPasses.Count == 358);
        }
    }
}
