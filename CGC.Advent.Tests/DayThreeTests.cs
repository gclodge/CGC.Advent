using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using CGC.Advent.Core.Helpers;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayThreeTests
    {
        //< I'm lazy right now, get at me
        public static readonly string TestDir = @"D:\_dev_test\CGC.Advent";

        //< From: https://adventofcode.com/2019/day/3
        private static List<Tuple<string, int, int>> TestTups = new List<Tuple<string, int, int>>()
        {
            Tuple.Create("R8,U5,L5,D3\r\nU7,R6,D4,L4", 6, 30),
            Tuple.Create("R75,D30,R83,U83,L12,D49,R71,U7,L72\r\nU62,R66,U55,R34,D71,R55,D58,R83", 159, 610),
            Tuple.Create("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\r\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135, 410),
        };

        [TestMethod]
        public void Test_KnownDistances()
        {
            //< Want to test that we get the correct distance for the above two example sets
            foreach (var testTup in TestTups)
            {
                var minDist = ManhattanHelper.GetMinManhattanDistance(testTup.Item1);
                Assert.IsTrue(minDist == testTup.Item2);
            }
        }

        [TestMethod]
        public void Test_KnownSteps()
        {
            //< Want to test that we get the correct distance for the above two example sets
            foreach (var testTup in TestTups)
            {
                var minDist = ManhattanHelper.GetMinStepsToIntersect(testTup.Item1);
                Assert.IsTrue(minDist == testTup.Item3);
            }
        }

        [TestMethod]
        public void Test_DayThree_PartOne()
        {
            var testFile = Path.Combine(TestDir, @"Day3.Input.txt");

            var minDist = ManhattanHelper.GetMinManhattanDistance(testFile);

            Assert.IsTrue(minDist == 3229);
        }

        [TestMethod]
        public void Test_DayThree_PartTwo()
        {
            var testFile = Path.Combine(TestDir, @"Day3.Input.txt");

            var minSteps = ManhattanHelper.GetMinStepsToIntersect(testFile);

            Assert.IsTrue(minSteps == 32132);
        }
    }
}
