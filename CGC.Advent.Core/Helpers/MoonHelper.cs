using System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

namespace CGC.Advent.Core.Helpers
{
    public class MoonHelper
    {
        public List<Moon> Moons { get; private set; } = null;

        public MoonHelper(IEnumerable<Moon> moons)
        {
            this.Moons = moons.ToList();
        }

        public void Simulate(int steps)
        {
            int currStep = 0;
            while (currStep < steps)
            {
                //< First, calculate the velocities
                this.Moons.ForEach(moon => moon.CalculateGravity(this.Moons));
                //< Apply the velocities
                this.Moons.ForEach(moon => moon.ApplyVelocity());
                //< Move to the next step
                currStep += 1;
            }
        }

        public long GetStepsToRepeat()
        {
            //< We need to find the number of cycles it takes for each velocity to become zero again
            long currStep = 0;

            long CycleX = -1;
            long CycleY = -1;
            long CycleZ = -1;
            while (true)
            {
                //< First, calculate the velocities
                this.Moons.ForEach(moon => moon.CalculateGravity(this.Moons));
                //< Apply the velocities
                this.Moons.ForEach(moon => moon.ApplyVelocity());
                //< Move to the next step
                currStep += 1;

                //< Once we know that (CycleX, CycleY, CycleZ) we need to computer the LCM of the triplet, then multiply by two.
                //< NB :: This is because we know the system will first arrive at Velocity (0, 0, 0) halfway through the cycle.

                //< Check each Moon's velocity components looking for their return to zero
                if (CycleX == -1 && this.Moons.All(moon => moon.Vx == 0))
                    CycleX = currStep;

                if (CycleY == -1 && this.Moons.All(moon => moon.Vy == 0))
                    CycleY = currStep;

                if (CycleZ == -1 && this.Moons.All(moon => moon.Vz == 0))
                    CycleZ = currStep;

                //< If they're all set, break out this bitch
                if (CycleX != -1 && CycleY != -1 && CycleZ != -1)
                {
                    var lcm = MathNet.Numerics.Euclid.LeastCommonMultiple(new[] { CycleX, CycleY, CycleZ });
                    return lcm * 2; //< Again, gotta double this bitch
                }
            }
        }
    }
}
