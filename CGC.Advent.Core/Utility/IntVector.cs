using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGC.Advent.Core.Utility
{
    public class IntVector
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public IntVector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static IntVector operator +(IntVector a, IntVector b)
        {
            return new IntVector(a.X + b.X, a.Y + b.Y);
        }

        public IntVector Clone()
        {
            return new IntVector(this.X, this.Y);
        }

        public int[] ToArray()
        {
            return new int[] { this.X, this.Y };
        }

        public string HashString()
        {
            return $"{this.X}-{this.Y}";
        }

        public override bool Equals(object obj)
        {
            var vec = obj as IntVector;
            if (vec != null)
            {
                return vec.X == this.X && vec.Y == this.Y;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
