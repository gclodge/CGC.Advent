using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGC.Advent.Core.Helpers
{
    public class FuelCalculator
    {
        public static int CalculateFuelFromMass(int mass)
        {
            var fuel = (int)Math.Floor(mass / 3.0) - 2;
            return fuel;
        }

        public static int CalculateFuelForMassAndFuel(int mass)
        {
            int totalFuel = 0;
            int value = mass;
            while (value > 0)
            {
                var fuel = CalculateFuelFromMass(value);
                if (fuel > 0)
                {
                    totalFuel += fuel;
                    value = fuel;
                }
                else
                    break;
            }

            return totalFuel;
        }
    }
}
