using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class AdventTests
    {
        //< I'm lazy right now, get at me
        public static readonly string TestDir = @"D:\_dev_test\CGC.Advent";

        //< From: https://adventofcode.com/2019/day/2
        private static List<Tuple<int[], int[]>> _TestTups = new List<Tuple<int[], int[]>>()
        {
            Tuple.Create(new int[] {1, 0, 0, 0, 99}, new int[] {2, 0, 0, 0, 99}),
            Tuple.Create(new int[] {2, 3, 0, 3, 99}, new int[] {2, 3, 0, 6, 99}),
            Tuple.Create(new int[] {2, 4, 4, 5, 99, 0}, new int[] {2, 4, 4, 5, 99, 9801}),
            Tuple.Create(new int[] {1, 1, 1, 4, 99, 5, 6, 0, 99}, new int[] {30, 1, 1, 4, 2, 5, 6, 0, 99}),
        };


        [TestMethod]
        public void Test_Opcodes()
        {
            foreach (var testTup in _TestTups)
            {
                var intcode = new Core.Classes.Intcode(testTup.Item1);
                intcode.Process();

                Assert.IsTrue(intcode.Source.SequenceEqual(testTup.Item2));
            }
        }

        [TestMethod]
        public void Test_DayTwo_PartOne()
        {
            var fn = Path.Combine(TestDir, "Day2.Input.txt");

            try
            {
                //< Generate and run the Intcode
                var intcode = new Core.Classes.Intcode(fn);
                intcode.Process();

                //< Ensure we've got the right first result
                Assert.IsTrue(intcode.Source[0] == 3562672);
            }
            catch (Exception e)
            {
                //< Rip
                int fail = 1;
            }
        }

        [TestMethod]
        public void Test_DayTwo_PartTwo()
        {
            //< This one has the un-altered input
            var fn = Path.Combine(TestDir, "Day2.Input - Backup.txt");

            const int nounPos = 1;
            const int verbPos = 2;
            const int finalVal = 19690720;

            int final_verb = -1;
            int final_noun = -1;

            try
            {
                //< We need to batch run every possibility of inputs as pos 1 / 2 between [0, 99] and find what pair results in '19690720'
                for (int noun = 0; noun < 100; noun++)
                {
                    if (final_noun != -1 && final_noun != -1)
                        break;

                    for (int verb = 0; verb < 100; verb++)
                    {
                        //< Ensure we ain't done
                        if (final_noun != -1 && final_noun != -1)
                            break;

                        //< Instantiate the Intcode
                        var intcode = new Core.Classes.Intcode(fn);
                        //< Alter the verb and noun and run
                        intcode.SetValue(nounPos, noun);
                        intcode.SetValue(verbPos, verb);
                        intcode.Process();
                        //< Check the value
                        if (intcode.GetValue(0) == finalVal)
                        {
                            final_verb = verb;
                            final_noun = noun;
                        }
                    }
                }
                //< Final noun is 82 and final verb is 50
                Assert.IsTrue(final_verb == 50 && final_noun == 82);
            }
            catch (Exception e)
            {
                //< Rip
                int fail = 1;
            }
        }
    }
}
