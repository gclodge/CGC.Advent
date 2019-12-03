using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace CGC.Advent.Core.Helpers
{
    public class LinearAlgebraHelper
    {
        public static Vector<double> GetIntersection(Utility.LineSegment A, Utility.LineSegment B)
        {
            double A1 = A.End[1] - A.Start[1];
            double B1 = A.Start[0] - A.End[0];
            double C1 = A1 * A.Start[0] + B1 * A.Start[1];

            double A2 = B.End[1] - B.Start[1];
            double B2 = B.Start[0] - B.End[0];
            double C2 = A2 * B.Start[0] + B2 * B.Start[1];

            double delta = A1 * B2 - A2 * B1;

            if (delta == 0)
                return null;
            else
            {
                double x = (B2 * C1 - B1 * C2) / delta;
                double y = (A1 * C2 - A2 * C1) / delta;
                var v = CreateVector.Dense(new double[] { x, y });

                if (v[0] == 0.0 && v[1] == 0.0)
                    return null;

                var T_A = A.GetT(v);
                var T_B = B.GetT(v);
                if (IsWithin(T_A, 0, A.Length) && IsWithin(T_B, 0, B.Length))
                {
                    return v;
                }
                else
                    return null;
            }
        }

        private static bool IsWithin(double val, double min, double max)
        {
            return (val < max && val > min);
        }

        private static bool Is2DEqual(Vector<double> P, Vector<double> Q)
        {
            return (P[0] == Q[0] && P[1] == Q[1]);
        }

        public static bool AreNearParallel(Vector<double> P, Vector<double> Q)
        {
            const double EPS = 0.001 * Math.PI / 180.0;
            double angle = Math.Acos(Math.Min(1, Math.Max(-1, P.Normalize(2).DotProduct(Q.Normalize(2))))); //< Ew

            bool nearParallel = angle < EPS || angle > Math.PI - EPS;
            return nearParallel;
        }
    }
}
