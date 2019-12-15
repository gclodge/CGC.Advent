using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayFiftenTests
    {
        [TestMethod]
        public void Test_DayFifteen_PartOne()
        {
            var source = Path.Combine(TestHelper.TestDir, "Day15.Input.txt");
            //< Instantiate droid (will fill out the Maze)
            var droid = new RepairDroid(source); //< Confirmed droid I'm looking for
            //< Get that puzzle result
            var steps = droid.GetMinStepsToOxygen();
            //< Ensure we ain't fuck up
            Assert.IsTrue(steps == 296);
        }

        [TestMethod]
        public void Test_DayFifteen_PartTwo()
        {
            var source = Path.Combine(TestHelper.TestDir, "Day15.Input.txt");
            //< Instantiate droid (will fill out the Maze)
            var droid = new RepairDroid(source);
            //< Get that puzzle result boi
            var time = droid.GetTimeToFill();
            Assert.IsTrue(time == 302);
        }
    }
}
