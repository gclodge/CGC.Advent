using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

using MathNet.Numerics.LinearAlgebra;

namespace CGC.Advent.Core.Helpers
{
    using V2 = Vector<double>;

    public class ManhattanHelper
    {
        public static double GetMinManhattanDistance(string input)
        {
            List<ManhattanWire> wires = ParseWires(input);

            //< Get the points, distances, and return the min
            var pnts = GetIntersectionPoints(wires); //< NB :: Hosed this up, is returning duplicate points (just need to filter)
            var dists = GetManhattanDistances(pnts);
            var minDist = dists.Min();

            return minDist;
        }

        public static double GetMinStepsToIntersect(string input)
        {
            List<ManhattanWire> wires = ParseWires(input);

            //< Get the points, distances, and return the min steps
            var pnts = GetIntersectionPoints(wires); //< NB :: Hosed this up, is returning duplicate points (just need to filter)
            var minSteps = GetMinimumStepsToIntersect(pnts, wires);

            return minSteps;
        }

        public static List<ManhattanWire> ParseWires(string input)
        {
            List<ManhattanWire> wires = null;
            if (input.EndsWith(".txt"))
            {
                wires = ParseWiresFromFile(input);
            }
            else
            {
                wires = input.Split(new string[] { "\r\n" }, StringSplitOptions.None)
                             .Select(d => new ManhattanWire(d)).ToList();
            }
            return wires;
        }

        private static List<ManhattanWire> ParseWiresFromFile(string wireFile)
        {
            //< Parse all the input data into the Array
            var data = File.ReadAllLines(wireFile);

            //< Ensure that we have more than one wire
            if (data.Length > 1)
            {
                var wires = data.Select(d => new ManhattanWire(d)).ToList();
                return wires;
            }
            else
            {
                throw new Exception($"Only one wire encountered in file: {wireFile}");
            }
        }

        public static List<V2> GetIntersectionPoints(List<ManhattanWire> wires)
        {
            var sects = new List<V2>();
            for (int i = 0; i < wires.Count; i++)
            {
                for (int j = 0; j < wires.Count; j++)
                {
                    if (j != i)
                    {
                        var pnts = wires[i].GetIntersectionPoints(wires[j]);
                        if (pnts.Count > 0)
                        {
                            sects.AddRange(pnts);
                        }
                    }
                }
            }
            return sects;
        }

        public static List<double> GetManhattanDistances(IEnumerable<V2> intersectionPoints)
        {
            //< Normally we'd need to do |p1 - q1| + |p2 - q2|
            //< But here, the origin is just (0, 0) so its just |X| + |Y|
            return intersectionPoints.Select(pnt => Math.Abs(pnt[0]) + Math.Abs(pnt[1])).ToList();
        }

        private static double GetMinimumStepsToIntersect(IEnumerable<V2> intersectionPoints, IEnumerable<ManhattanWire> wires)
        {
            var steps = new List<double>();
            foreach (var pnt in intersectionPoints)
            {
                var dists = wires.Select(wire => wire.GetStepsToIntersect(pnt)).ToList();
                var sum = dists.Sum();
                steps.Add(sum);
            }
            return steps.Min();
        }
    }
}
