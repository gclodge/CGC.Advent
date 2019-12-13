using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CGC.Advent.Core.Classes
{
    public class Moon
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; set; } = 0;

        public int Vx { get; set; } = 0;
        public int Vy { get; set; } = 0;
        public int Vz { get; set; } = 0;

        public string Name { get; set; } = null;

        public Moon()
        { }

        public Moon(int[] pos)
        {
            this.X = pos[0];
            this.Y = pos[1];
            this.Z = pos[2];
        }

        public void CalculateGravity(IEnumerable<Moon> moons)
        {
            //< Loop over all other moons, alter our gravity
            foreach (var moon in moons)
            {
                this.Vx += GetVelocity(this.X, moon.X);
                this.Vy += GetVelocity(this.Y, moon.Y);
                this.Vz += GetVelocity(this.Z, moon.Z);
            }
        }

        private static int GetVelocity(int ourPos, int theirPos)
        {
            if (ourPos == theirPos)
                return 0;
            else
            {
                return (theirPos > ourPos) ? 1 : -1;
            }
        }

        public void ApplyVelocity()
        {
            this.X += this.Vx;
            this.Y += this.Vy;
            this.Z += this.Vz;
        }

        public int CalculateTotalEnergy()
        {
            return CalculateKineticEnergy() * CalculatePotentialEnergy();
        }

        public int CalculateKineticEnergy()
        {
            return Math.Abs(this.Vx) + Math.Abs(this.Vy) + Math.Abs(this.Vz);
        }

        public int CalculatePotentialEnergy()
        {
            return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);
        }
    }
}
