using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using CGC.Advent.Core.Utility;

using MathNet.Numerics.LinearAlgebra;

namespace CGC.Advent.Core.Classes
{
    using V2 = Vector<double>;

    public class Asteroid
    {
        public int X { get; private set; } = -1;
        public int Y { get; private set; } = -1;

        public V2 Position => CreateVector.Dense(new double[] { this.X, this.Y });

        public bool IsVapourized = false;

        public Asteroid(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Asteroid;
            if (other != null)
            {
                return other.X == this.X && other.Y == this.Y;
            }
            else
                return false;
        }
    }

    class VisibleLine
    {
        public Asteroid A { get; }
        public Asteroid B { get; }

        public int dX => B.X - A.X;
        public int dY => B.Y - A.Y;

        public double Angle => (Math.PI / 2.0) + Math.Atan2(dY, dX);
        public double Length => (dX * dX + dY * dY);

        public VisibleLine(Asteroid a, Asteroid b)
        {
            this.A = a;
            this.B = b;
        }
    }

    public class AsteroidMap
    {
        public List<Asteroid> Asteroids { get; private set; } = null;

        public AsteroidMap(string fileName)
        {
            //< Have to parse the asteroids into memory
            this.Asteroids = ParseMap(fileName);
        }

        private static List<Asteroid> ParseMap(string fileName)
        {
            var asteroids = new List<Asteroid>();
            using (var sr = new StreamReader(fileName))
            {
                int currLine = 0;
                while (!sr.EndOfStream)
                {
                    //< Pull the entire line in
                    var line = sr.ReadLine();
                    //< Iterate through the line, find and create all asteroid
                    for (int i = 0; i < line.Length; i++)
                    {
                        //< If we've got an asteroid here, make one
                        if (line[i] == '#')
                        {
                            asteroids.Add(new Asteroid(i, currLine));
                        }
                    }
                    //< Move the counter to the next line/row
                    currLine++;
                }
            }
            return asteroids;
        }

        public Tuple<Asteroid, int> GetBestAsteroid()
        {
            int currCount = int.MinValue;
            Asteroid currBest = null;
            //< Iterate over each Asteroid
            foreach (var asteroid in this.Asteroids)
            {
                //< Generate a unique set of all rays tracing to other asteroids from this one
                var angles = new HashSet<double>();
                foreach (var other in this.Asteroids.Where(a => !a.Equals(asteroid)))
                {
                    angles.Add(new VisibleLine(asteroid, other).Angle);
                }
                //< If the total count of unique ray angles is more than curr max, use this one for now
                if (angles.Count > currCount)
                {
                    currCount = angles.Count;
                    currBest = asteroid;
                }
            }
            //< Return the best
            return Tuple.Create(currBest, currCount);
        }

        public List<Asteroid> GetVapourizationOrder(Asteroid laserPos)
        {
            //< Get all of the asteroids, ordered by distance from laser
            var assDict = GetLinesByAngle(laserPos, this.Asteroids);

            //< Get the angles in order (and output the starting index for the laser)
            int idx = 0;
            var angles = GetAngleOrder(assDict, out idx);

            //< Start the laser, find the next angle to be zapped, zap first point, move on.
            var vapourized = new List<Asteroid>();
            while (true)
            {
                //< Vapourize the first point at the first angle
                var angle = angles[idx];
                var nextPointIdx = assDict[angle].FindIndex(ass => !ass.IsVapourized);

                //< Add this Asteroid to the list of vapourization and mark it as vapourized
                vapourized.Add(assDict[angle][nextPointIdx]);
                assDict[angle][nextPointIdx].IsVapourized = true;

                //< Reset the angle index
                idx = (idx < angles.Length - 1) ? idx + 1 : 0;

                //< Check the count
                if (vapourized.Count == 200)
                {
                    break;
                }
            }
            return vapourized;
            throw new NotImplementedException();
        }

        private static Dictionary<double, List<Asteroid>> GetLinesByAngle(Asteroid laser, List<Asteroid> asteroids)
        {
            var assDict = new Dictionary<double, List<VisibleLine>>();
            foreach (var asteroid in asteroids)
            {
                if (asteroid.Equals(laser))
                    continue;

                var ls = new VisibleLine(laser, asteroid);
                if (!assDict.ContainsKey(ls.Angle))
                {
                    assDict.Add(ls.Angle, new List<VisibleLine>());
                }
                assDict[ls.Angle].Add(ls);
            }

            var orderedDict = new Dictionary<double, List<Asteroid>>();
            foreach (var kvp in assDict)
            {
                orderedDict.Add(kvp.Key, kvp.Value.OrderBy(ls => ls.Length).Select(ls => ls.B).ToList());
            }

            return orderedDict;
        }

        private static double[] GetAngleOrder(Dictionary<double, List<Asteroid>> assDict, out int startingIndex)
        {
            //< So, we need to know the order of angles the laser will hit
            //< Order the angles between [0, 2 * Math.Pi] where 0 is 'straight up'
            var angles = assDict.Keys.OrderBy(angle => angle).ToArray();

            //< Need to find the index that is nearest to zero
            startingIndex = GetIndexAtOrAboveZero(angles);

            //< Return
            return angles;
        }

        private static int GetIndexAtOrAboveZero(double[] angles)
        {
            int idx = -1;
            double currVal = double.MaxValue;
            for (int i = 0; i < angles.Length; i++)
            {
                if (angles[i] < 0.0)
                    continue;
                else if (angles[i] < currVal)
                {
                    currVal = angles[i];
                    idx = i;
                }
            }
            return idx;
        }
    }
}
