using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayNineTests
    {
        //< I'm lazy right now, get at me
        public static readonly string TestDir = @"D:\_dev_test\CGC.Advent";

        [TestMethod]
        public void Test_KnownResults()
        {
            var prog0 = "109,19,204,-34,99";
            var prog1 = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";
            var prog2 = "1102,34915192,34915192,7,4,7,99,0";
            var prog3 = "104,1125899906842624,99";

            var int0 = new Core.Classes.Intcode(prog0);
            int0.SetRelativeBase(2000);
            int0.SetValue(1985, 1985);
            int0.Process();
            Assert.IsTrue(int0.Outputs.Single().Item2 == 1985);

            var int1 = new Core.Classes.Intcode(prog1);
            int1.Process();
            var resArr = string.Join(",", int1.Outputs.Select(o => o.Item2));
            Assert.IsTrue(resArr.Equals(prog1));

            var int2 = new Core.Classes.Intcode(prog2);
            int2.Process();
            Assert.IsTrue(int2.Outputs.Single().Item2 == 1219070632396864);

            var int3 = new Core.Classes.Intcode(prog3);
            int3.Process();
            Assert.IsTrue(int3.Outputs.Single().Item2 == 1125899906842624);
        }

        [TestMethod]
        public void Test_DayNine_PartOne()
        {
            var input = Path.Combine(TestDir, "Day9.Input.txt");
            var intcode = new Core.Classes.Intcode(input);
            //< The input here is'1'
            intcode.AddInput(1);
            intcode.Process();

            //< Should only be one output as no Opcodes are failing
            Assert.IsTrue(intcode.Outputs.Count == 1);
            //< The resulting BOOST keycode should be '3546494377'
            Assert.IsTrue(intcode.Outputs.Single().Item2 == 3546494377);
        }

        [TestMethod]
        public void Test_DayNine_PartTwo()
        {
            var input = Path.Combine(TestDir, "Day9.Input.txt");
            var intcode = new Core.Classes.Intcode(input);
            //< The input is now '2'
            intcode.AddInput(2);
            intcode.Process();

            //< Should only be one output as no Opcodes are failing
            Assert.IsTrue(intcode.Outputs.Count == 1);
            //< The resulting BOOST keycode should be '47253'
            Assert.IsTrue(intcode.Outputs.Single().Item2 == 47253);
        }
    }
}
