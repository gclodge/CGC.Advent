using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace CGC.Advent.Core.Utility
{
    using V2 = Vector<double>;

    public class LineSegment
    {
        public V2 Start { get; private set; } = null;
        public V2 End { get; private set; } = null;

        public V2 Unit { get; private set; } = null;
        public double Length { get; private set; } = double.NaN;

        public LineSegment(V2 a, V2 b)
        {
            this.Start = a.Clone();
            this.End = b.Clone();

            var diff = this.End - this.Start;
            this.Length = diff.L2Norm();
            this.Unit = diff / this.Length;
        }

        public double GetT(V2 p)
        {
            return (p - Start).DotProduct(Unit);
        }

        public V2 ExtractPoint(double T)
        {
            return Start + T * Unit;
        }

        public V2 GetClosestPoint(V2 p)
        {
            var T = GetT(p);
            if (T <= 0)
            {
                return Start;
            }
            else if (T >= Length)
            {
                return End;
            }
            else
            {
                return ExtractPoint(T);
            }
        }

        public double DistanceToPoint(V2 p)
        {
            return (GetClosestPoint(p) - p).L2Norm();
        }

        public V2 GetIntersection(LineSegment that)
        {
            return Helpers.LinearAlgebraHelper.GetIntersection(this, that);
        }

        public bool ContainsPoint(V2 p)
        {
            var AB = this.Length;
            var AP = Math.Sqrt(Math.Pow(p[0] - this.Start[0], 2) + Math.Pow(p[1] - this.Start[1], 2));
            var PB = Math.Sqrt(Math.Pow(this.End[0] - p[0], 2) + Math.Pow(this.End[1] - p[1], 2));

            return (AB == AP + PB);
        }

        //< Note this shit is signed, negative refers to 'to the left' within the coordinate system
        public double GetSigned2DOffsetFromInfiniteLine(Vector<double> p)
        {
            double res = Unit[1] * (p[0] - Start[0]) - Unit[0] * (p[1] - Start[1]);
            return res;
        }
    }
}
