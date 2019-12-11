using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using CGC.Advent.Core.Helpers;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DaySevenTests
    {
        //< From: https://adventofcode.com/2019/day/7
        private static List<Tuple<int, int[], int[]>> TestTups = new List<Tuple<int, int[], int[]>>()
        {
            Tuple.Create(43210, new int[] {4,3,2,1,0}, new int[] {3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0}),
            Tuple.Create(54321, new int[] {0,1,2,3,4}, new int[] {3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0}),
            Tuple.Create(65210, new int[] {1,0,4,3,2}, new int[] {3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0}),
        };

        //< From: https://adventofcode.com/2019/day/7#part2
        private static List<Tuple<int, int[], string>> FeedbackTups = new List<Tuple<int, int[], string>>()
        {
            Tuple.Create(139629729, new int[] {9,8,7,6,5}, Path.Combine(TestHelper.TestDir, @"Day7.KnownFeedback.1.txt")),
            Tuple.Create(18216, new int[] {9,7,8,5,6}, Path.Combine(TestHelper.TestDir, @"Day7.KnownFeedback.2.txt")),
        };

        [TestMethod]
        public void Test_KnownPhaseSettings()
        {
            foreach (var testTup in TestTups)
            {
                var res = AmplifierHelper.GetThrustValue(testTup.Item2.ToList(), string.Join(",", testTup.Item3));
                Assert.IsTrue(res == testTup.Item1);
            }
        }

        [TestMethod]
        public void Test_KnownFeedbackLoops()
        {
            foreach (var testTup in FeedbackTups)
            {
                var res = AmplifierHelper.GetFeedbackThrustValue(testTup.Item2.ToList(), testTup.Item3);
                Assert.IsTrue(res == testTup.Item1);
            }
        }

        [TestMethod]
        public void Test_DaySeven_PartOne()
        {
            var testFile = Path.Combine(TestHelper.TestDir, "Day7.Input.txt");
            var maxResult = AmplifierHelper.GetMaximumThrustValue(testFile);

            Assert.IsTrue(maxResult == 366376);
        }

        [TestMethod]
        public void Test_DaySeven_PartTwo()
        {
            var testFile = Path.Combine(TestHelper.TestDir, "Day7.Input.txt");
            var maxResult = AmplifierHelper.GetMaximumFeedbackThrustValue(testFile);

            Assert.IsTrue(maxResult == 21596786);
        }
    }
}
