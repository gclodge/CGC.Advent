using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using CGC.Advent.Core.Helpers;
using CGC.Advent.Core.Utility;

using MathNet.Numerics.LinearAlgebra;

namespace CGC.Advent.Core.Classes
{
    using V2 = Vector<double>;

    public class ManhattanWire
    {
        const int OriginX = 0;
        const int OriginY = 0;

        private static readonly V2 U = CreateVector.Dense(new double[] { 0, 1 });
        private static readonly V2 D = CreateVector.Dense(new double[] { 0, -1 });
        private static readonly V2 L = CreateVector.Dense(new double[] { -1, 0 });
        private static readonly V2 R = CreateVector.Dense(new double[] { 1, 0 });

        public V2 Origin { get; private set; } = CreateVector.Dense(new double[] { OriginX, OriginY });

        public List<V2> Directions { get; private set; } = null;
        public List<V2> Coordinates { get; private set; } = null;

        public List<LineSegment> Segments { get; private set; } = null;

        public ManhattanWire(string dirString)
        {
            //< Parse all the invididual Directions from the input string
            this.Directions = dirString.Split(',').Select(d => ParseDirection(d)).ToList();
            //< Generate all the coordinates
            this.Coordinates = GenerateCoordinates(this.Directions, this.Origin);
            //< Generate all the segments
            this.Segments = GenerateSegments(this.Directions, this.Origin);
        }

        public List<V2> GetIntersectionPoints(ManhattanWire that)
        {
            var pnts = new List<V2>();
            foreach (var thisSeg in this.Segments)
            {
                foreach (var thatSeg in that.Segments)
                {
                    var pnt = thisSeg.GetIntersection(thatSeg);
                    if (pnt != null)
                    {
                        pnts.Add(pnt);
                    }
                }
            }
            return pnts;
        }

        public double GetStepsToIntersect(V2 p)
        {
            double totalDist = 0.0;
            for (int i = 0; i < this.Segments.Count; i++)
            {
                var segment = this.Segments[i];
                if (segment.ContainsPoint(p))
                {
                    var dist = segment.GetT(p);
                    totalDist += dist;
                    break;
                }
                else
                {
                    totalDist += segment.Length;
                }
            }

            return totalDist;
        }

        private static List<V2> GenerateCoordinates(IEnumerable<V2> directions, V2 origin)
        {
            var coords = new List<V2>();
            var currOrigin = origin.Clone();
            foreach (var dir in directions)
            {
                currOrigin += dir;
                coords.Add(currOrigin.Clone());
            }
            return coords;
        }

        private static List<LineSegment> GenerateSegments(IEnumerable<V2> directions, V2 origin)
        {
            var segs = new List<LineSegment>();
            V2 currOrigin = origin.Clone();
            foreach (var dir in directions)
            {
                //< Generate the segment
                var A = currOrigin.Clone();
                var B = currOrigin + dir;
                segs.Add(new LineSegment(A, B));
                //< Move the origin to the final point
                currOrigin = B.Clone();
            }
            return segs;
        }

        private static V2 ParseDirection(string dir)
        {
            //< Parse the direction (D) and magnitude (MM) from the direction in format (DMM)
            var direction = dir.Substring(0, 1);
            var magnitude = int.Parse(dir.Substring(1, dir.Length - 1));

            //< Get the base vector - multiply by the magnitude to get the final vector
            var baseVec = GetBaseVectorForDirection(direction);
            var finalVec = baseVec * magnitude;

            return finalVec;
        }

        private static V2 GetBaseVectorForDirection(string direction)
        {
            switch (direction)
            {
                case "U":
                    return U;
                case "D":
                    return D;
                case "L":
                    return L;
                case "R":
                    return R;
                default:
                    throw new InvalidDataException($"Unknown direction encountered: {direction}");
            }
        }
    }
}
