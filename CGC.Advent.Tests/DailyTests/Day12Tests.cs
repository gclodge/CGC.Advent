using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;
using CGC.Advent.Core.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayTwelveTests
    {
        private static List<int[]> TestMoonsOne = new List<int[]>()
        {
            new int[] { -1, 0, 2 },
            new int[] {  2, -10, -7 },
            new int[] {  4, -8, 8 },
            new int[] {  3, 5, -1 },
        };

        private static List<int[]> TestResultsOne = new List<int[]>()
        {
            new int[] {2, 1, -3, -3, -2, 1 },
            new int[] {1, -8, 0, -1, 1, 3 },
            new int[] {3, -6, 1, 3, 2, -3 },
            new int[] {2, 0, 4, 1, -1, -1 }
        };

        private static List<int[]> TestMoonsTwo = new List<int[]>()
        {
            new int[] { -8, -10, 0 },
            new int[] {  5, 5, 10 },
            new int[] {  2, -7, 3 },
            new int[] {  9, -8, -3 },
        };

        private static List<int[]> TestResultsTwo = new List<int[]>()
        {
            new int[] {8, -12, -9, -7, 3, 0 },
            new int[] {13, 16, -3, 3, -11, -5 },
            new int[] {-29, -11, -1, -3, 7, 4 },
            new int[] {16, -13, 23, 7, 1, 1 }
        };

        private static List<int[]> PartOneMoons = new List<int[]>()
        {
            new int[] { -9, 10, -1 },
            new int[] {  -14, -8, 14 },
            new int[] {  1, 5, 6 },
            new int[] {  -19, 7, 8 },
        };

        [TestMethod]
        public void Test_MoonsOne()
        {
            int numSteps = 10;
            //< Get them moons
            var moons = GetMoons(TestMoonsOne);
            //< Get the helper
            var helper = new MoonHelper(moons);
            helper.Simulate(numSteps);
            //< Check the results
            for (int i = 0; i < helper.Moons.Count; i++)
            {
                var moon = helper.Moons[i];
                var res = TestResultsOne[i];
                Assert.IsTrue(moon.X == res[0] && moon.Y == res[1] && moon.Z == res[2]);
                Assert.IsTrue(moon.Vx == res[3] && moon.Vy == res[4] && moon.Vz == res[5]);
            }
            //< Get the total energy and check it's correct
            var energies = helper.Moons.Select(moon => moon.CalculateTotalEnergy()).ToList();
            var total = energies.Sum();
            Assert.IsTrue(total == 179);
        }

        [TestMethod]
        public void Test_MoonsTwo()
        {
            int numSteps = 100;
            //< Get them moons
            var moons = GetMoons(TestMoonsTwo);
            //< Get the helper
            var helper = new MoonHelper(moons);
            helper.Simulate(numSteps);
            //< Check the results
            for (int i = 0; i < helper.Moons.Count; i++)
            {
                var moon = helper.Moons[i];
                var res = TestResultsTwo[i];
                Assert.IsTrue(moon.X == res[0] && moon.Y == res[1] && moon.Z == res[2]);
                Assert.IsTrue(moon.Vx == res[3] && moon.Vy == res[4] && moon.Vz == res[5]);
            }
            //< Get the total energy and check it's correct
            var energies = helper.Moons.Select(moon => moon.CalculateTotalEnergy()).ToList();
            var total = energies.Sum();
            Assert.IsTrue(total == 1940);
        }

        [TestMethod]
        public void Test_PartOne()
        {
            int numSteps = 1000;
            //< Get them moons
            var moons = GetMoons(PartOneMoons);
            //< Get the helper
            var helper = new MoonHelper(moons);
            helper.Simulate(numSteps);
            //< Get the total energy and check it's correct
            var energies = helper.Moons.Select(moon => moon.CalculateTotalEnergy()).ToList();
            var total = energies.Sum();
            Assert.IsTrue(total == 8538);
        }

        [TestMethod]
        public void Test_PartTwo()
        {
            //< Get them moons
            var moons = GetMoons(PartOneMoons);
            //< Get the helper
            var helper = new MoonHelper(moons);
            //< Omae wa mou shindeiru
            var numSteps = helper.GetStepsToRepeat();
            //< NANI???
            Assert.IsTrue(numSteps == 506359021038056);

            //< Some quik mafs - it takes ~12ms for 1000 steps in PartOne
            var secPerStep = (12.0 / 10E3) / 100.0;
            var simTimeSec = numSteps * secPerStep;
            var simTimeDays = simTimeSec / 86400.0;
            var simTimeYears = simTimeDays / 365.0;

            Assert.IsTrue(IsToFuckingMuch(simTimeSec));
        }

        private static Moon[] GetMoons(IEnumerable<int[]> positions)
        {
            return positions.Select(pos => new Moon(pos)).ToArray();
        }

        private static bool IsToFuckingMuch(double val)
        {
            //< It really is man
            return true;
        }
    }
}
