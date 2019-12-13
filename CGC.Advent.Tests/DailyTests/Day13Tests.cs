using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGC.Advent.Tests
{
    [TestClass]
    public class DayThirteenTests
    {
        [TestMethod]
        public void Test_DayThirteen_PartOne()
        {
            //< Git 'er done
            var source = Path.Combine(TestHelper.TestDir, "Day13.Input.txt");
            //< Instantiate the game and run it
            var game = new ArcadeGame(source);
            game.TestGame();
            //< Get the count of 'Block' tiles (t = 2)
            var numBlocks = game.Grid.CountTiles(2);
            //< Make sure we ain't fuck it up
            Assert.IsTrue(numBlocks == 452);
        }

        [TestMethod]
        public void Test_DayThirteen_PartTwo()
        {
            //< Git 'er done
            var source = Path.Combine(TestHelper.TestDir, "Day13.Input.txt");
            //< Run.. the game
            var game = new ArcadeGame(source);
            game.RunGame();
            //< Check the final score is correct
            Assert.IsTrue(game.Score == 21415);
        }
    }
}
