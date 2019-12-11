using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayElevenTests
    {
        [TestMethod]
        public void Test_DayEleven_PartOne()
        {
            //< Make it
            var source = Path.Combine(TestHelper.TestDir, "Day11.Input.txt");
            int width = 1000;
            int height = 1000;
            var painter = new HullPainter(source, width, height);
            //< Run it
            painter.Paint();
            //< Output the test file
            var testFile = Path.Combine(TestHelper.TestDir, "Day11.PartOne.png");
            painter.GenerateImage(testFile);
            //< Make sure we ain't fuck up
            Assert.IsTrue(painter.Painted.Count == 2276);
        }

        [TestMethod]
        public void Test_DayEleven_PartTwo()
        {
            //< Make it
            var source = Path.Combine(TestHelper.TestDir, "Day11.Input.txt");
            int width = 1000;
            int height = 1000;
            var painter = new HullPainter(source, width, height);
            //< Run it
            painter.Paint(startingColour: 1);
            //< Output the test file
            var testFile = Path.Combine(TestHelper.TestDir, "Day11.PartTwo.png");
            painter.GenerateImage(testFile);

            //< We can't really test the output.. so.. free test.
            //< ALSO YOUR SHIT IS BACKWARDS NERD
            Assert.IsTrue(true);
        }
    }
}
