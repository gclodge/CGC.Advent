using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace CGC.Advent.Core.Classes
{
    public class CardinalDirections
    {
        private const long NorthValue = 1;
        private const long SouthValue = 2;
        private const long WestValue = 3;
        private const long EastValue = 4;

        public static readonly Point North = new Point(0, 1);
        public static readonly Point South = new Point(0, -1);
        public static readonly Point West  = new Point(-1, 0);
        public static readonly Point East  = new Point(1, 0);

        public static List<Tuple<Point, long>> GetCardinalPointsAround(Point pos)
        {
            return new List<Tuple<Point, long>>()
            {
                Tuple.Create(pos.Add(North), NorthValue),
                Tuple.Create(pos.Add(South), SouthValue),
                Tuple.Create(pos.Add(West), WestValue),
                Tuple.Create(pos.Add(East), EastValue)
            };
        }

        public static long GetOppositeDirection(long dir)
        {
            switch (dir)
            {
                case NorthValue:
                    return SouthValue;
                case SouthValue:
                    return NorthValue;
                case WestValue:
                    return EastValue;
                case EastValue:
                    return WestValue;
                default:
                    throw new Exception("Non-cardinal direction supplied");
            }
        }
    }

    static class Ext
    {
        public static Point Add(this Point p, Point q)
        {
            return new Point(p.X + q.X, p.Y + q.Y);
        }

        public static Point Subtract(this Point p, Point q)
        {
            return new Point(p.X - q.X, p.Y - q.Y);
        }
    }

    public class RepairDroid
    {
        private const int Unexplored = 0;
        private const int Path = 1;
        private const int Wall = 2;
        private const int Tank = 3;

        Dictionary<Point, long> PointMap = null;
        long[,] Maze = null;
        long[,] FloodLevels = null;
        bool[,] Explored = null;

        public Intcode Brain { get; private set; } = null;

        public Point Start { get; private set; } 
        public Point Target { get; private set; }

        public RepairDroid(string source)
        {
            this.Brain = new Intcode(source);
            this.PointMap = new Dictionary<Point, long>();
            BuildMaze();
        }

        public long GetMinStepsToOxygen()
        {
            FloodFill(this.Start);
            return FloodLevels[Target.Y, Target.X];
        }

        public long GetTimeToFill()
        {
            //< Want to fill out from the where the Oxygen tank is
            FloodFill(this.Target);
            //< Each movement to an adjacent cell takes 1 minute
            //< Just need to find whichever cell took the longest to fill, and return the FillLevel there
            var maxLevel = FloodLevels.Cast<long>().Max();
            return maxLevel;
        }

        private void BuildMaze()
        {
            //< TODO :: Verify if we need to run the program once at the start or not

            //var origin = new Point(0, 0);
            this.Start = new Point(0, 0);
            Walk(this.Start);

            //< Calculate the total size of the maze
            long offsetX = 0;
            long offsetY = 0;
            var size = GetMazeSize(out offsetX, out offsetY);

            //< Need to offset the Start/Target points so they fit in the arrays
            this.Start = new Point(Start.X + (int)offsetX, Start.Y + (int)offsetY);
            this.Target = new Point(Target.X + (int)offsetX, Target.Y + (int)offsetY);

            //< Instantiate the array containers
            this.Maze = new long[size.Height + 1, size.Width + 1];
            this.FloodLevels = new long[size.Height + 1, size.Width + 1];
            this.Explored = new bool[size.Height + 1, size.Width + 1];

            //< Fill in the maze with what we've found
            foreach (var kvp in PointMap)
            {
                this.Maze[kvp.Key.Y + offsetY, kvp.Key.X + offsetX] = kvp.Value;
            }
        }

        private void FloodFill(Point start)
        {
            var toProc = new Stack<Point>();
            toProc.Push(start);

            while (toProc.Count > 0)
            {
                var p = toProc.Pop();

                var cardinals = CardinalDirections.GetCardinalPointsAround(p).Select(c => c.Item1);
                foreach (var point in cardinals)
                {
                    if (Maze[point.Y, point.X] != 0)
                    {
                        //< If un-explored, fill it in
                        if (Explored[point.Y, point.X] == false)
                        {
                            //< Set the flood value to 1 more than the the previous point
                            FloodLevels[point.Y, point.X] = FloodLevels[p.Y, p.X] + 1;
                            //< Add this point to be filled
                            toProc.Push(point);
                        }
                    }
                }

                //< Set the current point as seen
                Explored[p.Y, p.X] = true;
            }
        }

        private Size GetMazeSize(out long offsetX, out long offsetY)
        {
            var minX = PointMap.Keys.Min(p => p.X);
            var minY = PointMap.Keys.Min(p => p.Y);
            var maxX = PointMap.Keys.Max(p => p.X);
            var maxY = PointMap.Keys.Max(p => p.Y);

            offsetX = Math.Abs(minX);
            offsetY = Math.Abs(minY);

            return new Size(maxX + (int)offsetX, maxY + (int)offsetY);
        }

        private void Walk(Point p)
        {
            //< Get all the cardinal directions that need checking
            var pointsToCheck = GetPointsToCheck(p);

            foreach (var point in pointsToCheck)
            {
                if (this.PointMap.ContainsKey(point.Item1))
                    continue;

                //< Get the movement result and add that point to the PointMap
                var moveRes = MoveInDirection(point.Item2);
                this.PointMap.Add(point.Item1, moveRes);

                //< If non-zero movement result, handle
                if (moveRes != 0)
                {
                    //< Walk new point, then take back the movement
                    Walk(point.Item1);
                    //< Get the opposite direction to our present movement, move that way
                    var oppositeDir = CardinalDirections.GetOppositeDirection(point.Item2);
                    MoveInDirection(oppositeDir);
                    if (moveRes == 2)
                    {
                        PointMap[point.Item1] = 1;
                        Target = new Point(point.Item1.X, point.Item1.Y);
                    }
                }
            }
        }

        private long MoveInDirection(long dir)
        {
            //< Add the direction and run the program
            this.Brain.AddInput(dir);
            this.Brain.Process();
            //< Return the last output
            return this.Brain.Outputs.Last().Item2;
        }

        private List<Tuple<Point, long>> GetPointsToCheck(Point p)
        {
            //< Get all the cardinal positions around this point
            var cardinals = CardinalDirections.GetCardinalPointsAround(p);
            //< Return those that ain't been explored
            return cardinals.Where(c => IsUnexplored(c.Item1)).ToList();
        }

        private bool IsUnexplored(Point p)
        {
            return !PointMap.ContainsKey(p);
        }
    }
}
