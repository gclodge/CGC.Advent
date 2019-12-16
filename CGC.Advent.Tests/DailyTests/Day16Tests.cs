using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DaySixteenTests
    {
        private static readonly List<Tuple<string, int, string>> KnownSignals = new List<Tuple<string, int, string>>()
        {
            Tuple.Create("12345678", 4, "01029498"),
            Tuple.Create("80871224585914546619083218645595", 100, "24176176"),
            Tuple.Create("19617804207202209144916044189917", 100, "73745418"),
            Tuple.Create("69317163492948606335995924319873", 100, "52432133"),
        };

        private static readonly List<Tuple<string, int, string>> KnownOffsetSignals = new List<Tuple<string, int, string>>()
        {
            Tuple.Create("03036732577212944063491565474664", 100, "84462026"),
            Tuple.Create("02935109699940807407585447034323", 100, "78725270"),
            Tuple.Create("03081770884921959731165446850517", 100, "53553731"),
        };

        [TestMethod]
        public void Test_KnownSignals()
        {
            foreach (var testTup in KnownSignals)
            {
                var sig = FlawedFrequency.CalculatePartOne(testTup.Item1, testTup.Item2);
                Assert.IsTrue(sig.StartsWith(testTup.Item3));
            }
        }

        [TestMethod]
        public void Test_DaySixteen_PartOne()
        {
            //< Get the input signal
            var source = Path.Combine(TestHelper.TestDir, "Day16.Input.txt");
            var signal = File.ReadAllText(source);
            //< Parse/calculate the output
            var sig = FlawedFrequency.CalculatePartOne(signal, 100);
            Assert.IsTrue(sig == "25131128");
        }

        [TestMethod]
        public void Test_KnownOffsetSignals()
        {
            foreach (var testTup in KnownOffsetSignals)
            {
                var sig = FlawedFrequency.CalculatePartTwo(testTup.Item1, 100);
                Assert.IsTrue(sig == testTup.Item3);
            }
        }

        [TestMethod]
        public void Test_DaySixteen_PartTwo()
        {
            //< Get the input signal
            var source = Path.Combine(TestHelper.TestDir, "Day16.Input.txt");
            var signal = File.ReadAllText(source);
            //< Parse/calculate the output
            var sig = FlawedFrequency.CalculatePartTwo(signal, 100);

            Assert.IsTrue(sig == "53201602");
        }
    }
}
