using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayEightTests
    {
        //< From: https://adventofcode.com/2019/day/5#part2
        private static List<Tuple<string, int, int>> KnownImages = new List<Tuple<string, int, int>>()
        {
            Tuple.Create("123456789012", 3, 2),
        };

        [TestMethod]
        public void Test_KnownImage()
        {
            var imageData = "123456789012";
            int width = 3;
            int height = 2;

            var img = new Core.Classes.SpaceImage(imageData, width, height);

            Assert.IsTrue(img.Layers.Count == 2);
            Assert.IsTrue(img.Layers.All(layer => layer.Length == 6));
        }

        const int Width = 25;
        const int Height = 6;

        [TestMethod]
        public void Test_DayEight_PartOne()
        {
            var imageData = Path.Combine(TestHelper.TestDir, "Day8.Input.txt");
            //< Parse the actual image data
            var img = new Core.Classes.SpaceImage(imageData, Width, Height);
            //< Get the layer with the fewest '0' digits
            var minLayer = img.GetLayerWithMinimumNumValues(0);
            //< Get the number of 1 digits and number of 2 digits, multiply 'em together
            var countOnes = Core.Classes.SpaceImage.GetNumDigits(minLayer, 1);
            var countTwos = Core.Classes.SpaceImage.GetNumDigits(minLayer, 2);
            var res = countOnes * countTwos;

            Assert.IsTrue(res == 1320);
        }

        [TestMethod]
        public void Test_DayEight_PartTwo()
        {
            var imageData = Path.Combine(TestHelper.TestDir, "Day8.Input.txt");
            //< Parse the actual image data
            var img = new Core.Classes.SpaceImage(imageData, Width, Height);
            //< Generate the final image
            var imageName = Path.ChangeExtension(imageData, ".png");
            img.GenerateFinalImage(imageName);

            //< Can't really auto-parse and read the image.. it says 'RCYKR'
            Assert.IsTrue(File.Exists(imageName));
        }
    }
}
