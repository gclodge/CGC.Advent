using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGC.Advent.Core.Classes
{
    public class ElvenPassword
    {
        //< Store the actual numeric value of the password here
        public int Value { get; private set; } = int.MinValue;

        //< Return the string representation of the numerical password
        public string String => this.Value.ToString();

        public ElvenPassword(string pass)
        {
            this.Value = int.Parse(pass);
        }

        public ElvenPassword(int pass)
        {
            this.Value = pass;  
        }

        const int PasswordLength = 6;
        public bool IsGood(bool requireAdjacentPair = false)
        {
            //< What do?
            var valString = this.String;
            //< We must:
            //<  - Ensure we're the right length
            //<  - Ensure we have two adjacent digits
            //<  - Ensure we're only ever increasing (or staying the same)
            var bLength = valString.Length == PasswordLength;
            var bAdjacent = ContainsAdjacentDigits(valString);
            var bDirection = EnsureDirectionality(valString);

            if (requireAdjacentPair)
            {
                var bPair = ContainsAnAdjacentPair(valString);
                return (bLength && bAdjacent && bDirection && bPair);
            }
            else
                return (bLength && bAdjacent && bDirection);
        }

        private static bool ContainsAdjacentDigits(string val)
        {
            bool hasAdjacent = false;
            //< Loop over all but the last digit
            for (int i = 0; i < val.Length - 1; i++)
            {
                if (val[i] == val[i + 1])
                {
                    hasAdjacent = true;
                    break;
                }
            }
            return hasAdjacent;
        }

        private static bool EnsureDirectionality(string val)
        {
            //< Loop over all but the last digit
            for (int i = 0; i < val.Length - 1; i++)
            {
                var curr = int.Parse(val[i].ToString());
                var next = int.Parse(val[i + 1].ToString());
                if (curr > next)
                {
                    return false;
                }
            }
            //< If we passed through the loop, we never decrease, return true
            return true;
        }

        //< Part two specifies that it contains a 'matching adjacent number pair' ie. (112233 or 111122 works but 112333 does not).
        const int MaxAllowedAdjacentChars = 2;
        private static bool ContainsAnAdjacentPair(string val)
        {
            //< Quickly get all adjacent groups, verify none have a count more than 2
            var groupArray = new int[val.Length];

            int currGroup = 1;
            for (int i = 0; i < val.Length; i++)
            {
                groupArray[i] = currGroup;
                if (i < val.Length - 1)
                {
                    //< If we have a match with the following character, don't alter the 'currGroup'
                    if (val[i] != val[i + 1])
                    {
                        currGroup++;
                    }
                }
            }

            //< Now we want to count the total occurences of each group ID
            var groups = groupArray.GroupBy(g => g).ToList();
            return groups.Any(grp => grp.Count() == MaxAllowedAdjacentChars);
        }

        public static List<ElvenPassword> GetGoodPasswordsInRange(int minValue, int maxValue, bool requireAdjacentPair = false)
        {
            var passes = Enumerable.Range(minValue, maxValue - minValue + 1).Select(v => new ElvenPassword(v)).ToList();
            return passes.Where(pass => pass.IsGood(requireAdjacentPair)).ToList();
        }
    }
}
