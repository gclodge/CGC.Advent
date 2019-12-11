using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Drawing;

using CGC.Advent.Core.Utility;

namespace CGC.Advent.Core.Classes
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        WAT
    }

    public class HullPainter
    {
        public int Width { get; } = 0;
        public int Height { get; } = 0;
        public int[,] Hull { get; private set; } = null;

        public Intcode Brain { get; private set; } = null;

        public IntVector Position { get; private set; } = null;
        public Direction Direction { get; private set; } = Direction.DOWN;

        private int CurrOutput = 0;
        public HashSet<string> Painted { get; private set; } = new HashSet<string>();
        public int CountIters = 0;

        public HullPainter(string intCodeSource, int hullWidth, int hullHeight)
        {
            //< Parse the Intcode 'Brain'
            this.Brain = new Intcode(intCodeSource);
            //< Start the panel array - defaults to 0 which is 'black'
            this.Width = hullWidth;
            this.Height = hullHeight;
            this.Hull = new int[hullWidth, hullHeight];
        }

        public void Paint(int startingColour = -1)
        {
            //< Start 'em in the middle, facing up
            int startX = this.Width / 2;
            int startY = this.Height / 2;
            this.Position = new IntVector(startX, startY);

            if (startingColour != -1)
            {
                this.Hull[this.Position.X, this.Position.Y] = startingColour;
            }

            //< Reset the output index and position hashset
            this.CurrOutput = 0;
            this.Painted = new HashSet<string>();

            //< Let 'er rip
            Process();
        }

        private void Process()
        {
            this.CountIters = 0;
            while (!this.Brain.IsFinished)
            {
                //< Get the input from the current position, mainline that shit (to the.. brain?)
                var inp = GetHullValue(this.Position);
                this.Brain.AddInput(inp);

                //< Process until either halt or wait
                this.Brain.Process();

                //< Extract the two outputs (colour painted and direction turned)
                var colour = this.Brain.Outputs[CurrOutput].Item2;
                var dirFlag = this.Brain.Outputs[CurrOutput + 1].Item2;
                //< Move the output index forward
                CurrOutput += 2;

                //< Update the hull array and track which panel's been painted
                this.Hull[Position.X, Position.Y] = (int)colour;
                this.Painted.Add(this.Position.HashString());

                //< Get the direction offset and update the position and direction
                var dirOffset = GetDirectionOffset(this.Position, this.Direction, (int)dirFlag);
                this.Position += dirOffset;
                this.Direction = GetNewDirection(dirOffset);
                this.CountIters++;
            }
        }

        private int GetHullValue(IntVector pos)
        {
            var bX = pos.X >= 0 && pos.X < this.Width;
            var bY = pos.Y >= 0 && pos.Y < this.Height;

            if (bX && bY)
            {
                return this.Hull[pos.X, pos.Y];
            }
            else
                throw new Exception($"Outside bounds of Hull! Pos: {pos.ToArray()}");
        }

        private static IntVector GetDirectionOffset(IntVector pos, Direction currDir, int dirFlag)
        {
            var currHeading = GetCurrentHeading(currDir);
            switch (dirFlag)
            {
                case 0:
                    return new IntVector(-1 * currHeading.Y, currHeading.X);
                case 1:
                    return new IntVector(currHeading.Y, -1 * currHeading.X);
                default:
                    throw new Exception("You can only turn left or right Mr. Zoolander");
            }

            //< If we're facing U (0, 1) and turn R -> (1, 0) or L -> (-1, 0)
            //< If we're facing R (1, 0) and turn R -> (0, -1) or L -> (0, 1)
            //< If we're facing D (0, -1) and turn R -> (-1, 0) or L -> (1, 0)
            //< If we're facing L (-1, 0) and turn R -> (0, 1) or L -> (0, -1)
        }

        private static readonly Dictionary<Direction, IntVector> Headings = new Dictionary<Direction, IntVector>()
        {
            { Direction.UP, new IntVector(0, 1) },
            { Direction.DOWN, new IntVector(0, -1) },
            { Direction.LEFT, new IntVector(-1, 0) },
            { Direction.RIGHT, new IntVector(1, 0) },
        };

        private static IntVector GetCurrentHeading(Direction currDir)
        {
            if (Headings.ContainsKey(currDir))
            {
                return Headings[currDir];
            }
            else
                throw new Exception("But like, how, man?");
        }

        private static Direction GetNewDirection(IntVector dirOffset)
        {
            foreach (var dirKvp in Headings)
            {
                if (dirKvp.Value.Equals(dirOffset))
                {
                    return dirKvp.Key;
                }
            }
            throw new Exception("No known direction for given directional offset");
        }

        public void GenerateImage(string image)
        {
            var bmp = GenerateImage(this.Hull, this.Width, this.Height);
            bmp.Save(image, System.Drawing.Imaging.ImageFormat.Png);
        }

        private static Bitmap GenerateImage(int[,] image, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bmp.SetPixel(x, y, SpaceImage.GetColor(image[x, y]));
                }
            }
            return bmp;
        }
    }
}
