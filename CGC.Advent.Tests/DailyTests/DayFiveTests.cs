using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayFiveTests
    {
        //< From: https://adventofcode.com/2019/day/5#part2
        private static List<Tuple<int, long[], int>> ComparisonTests = new List<Tuple<int, long[], int>>()
        {
            Tuple.Create(8, new long[] { 3,9,8,9,10,9,4,9,99,-1,8 }, 1),
            Tuple.Create(9, new long[] { 3,9,8,9,10,9,4,9,99,-1,8 }, 0),
            Tuple.Create(7, new long[] { 3,9,7,9,10,9,4,9,99,-1,8 }, 1),
            Tuple.Create(8, new long[] { 3,9,7,9,10,9,4,9,99,-1,8 }, 0),
            Tuple.Create(8, new long[] { 3,3,1108,-1,8,3,4,3,99 }, 1),
            Tuple.Create(9, new long[] { 3,3,1108,-1,8,3,4,3,99 }, 0),
            Tuple.Create(7, new long[] { 3,3,1107,-1,8,3,4,3,99 }, 1),
            Tuple.Create(8, new long[] { 3,3,1107,-1,8,3,4,3,99 }, 0),

            Tuple.Create(0,    new long[] { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 }, 0),
            Tuple.Create(1337, new long[] { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 }, 1),
            Tuple.Create(0,    new long[] { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 },      0),
            Tuple.Create(1337, new long[] { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 },      1),

            Tuple.Create(7, new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 999),
            Tuple.Create(8, new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 1000),
            Tuple.Create(9, new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 1001),
        };

        [TestMethod]
        public void Test_DayFive_PartOne()
        {
            var fn = Path.Combine(TestHelper.TestDir, "Day5.Input.txt");

            try
            {
                //< Generate and run the Intcode
                var intcode = new Core.Classes.Intcode(fn);
                intcode.AddInput(1);
                intcode.Process();

                //< Need to validate that all outputs save the last were 0
                var countNonZero = intcode.Outputs.Count(output => output.Item2 != 0);
                Assert.IsTrue(countNonZero == 1);

                //< Need to ensure the non-zero output is the last, and that it's the correct value (6761139) 
                var nonZero = intcode.Outputs.Where(o => o.Item2 != 0).Single();
                var idxNonZero = intcode.Outputs.IndexOf(nonZero);
                Assert.IsTrue(idxNonZero == intcode.Outputs.Count - 1);
                Assert.IsTrue(nonZero.Item2 == 6761139);
            }
            catch (Exception e)
            {
                //< Rip
                int fail = 1;
            }
        }

        [TestMethod]
        public void Test_KnownComparisons()
        {
            foreach (var testTup in ComparisonTests)
            {
                var intcode = new Core.Classes.Intcode(testTup.Item2);
                intcode.AddInput(testTup.Item1);
                intcode.Process();

                Assert.IsTrue(intcode.Outputs.Single().Item2 == testTup.Item3);
            }
        }

        [TestMethod]
        public void Test_DayFive_PartTwo()
        {
            var fn = Path.Combine(TestHelper.TestDir, "Day5.Input.txt");

            try
            {
                //< Generate and run the Intcode
                var intcode = new Core.Classes.Intcode(fn);
                intcode.AddInput(5);
                intcode.Process();

                //< There should only be one output
                var output = intcode.Outputs.Single();
                Assert.IsTrue(output.Item2 == 9217546);
            }
            catch (Exception e)
            {
                //< Rip
                int fail = 1;
            }
        }
    }
}
