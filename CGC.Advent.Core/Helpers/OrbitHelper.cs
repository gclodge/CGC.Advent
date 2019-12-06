using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using CGC.Advent.Core.Classes;

namespace CGC.Advent.Core.Helpers
{
    public class OrbitHelper
    {
        public static int CountTotalOrbits(IEnumerable<Orbit> orbits)
        {
            //< Get the map of all Celestial Bodies in this orbit sequence (and their connections/orbits)
            var bodies = GetBodyDict(orbits);

            //< For each unique celestial body, count all orbits (direct and indirect)
            var results = new List<Tuple<string, int>>();
            foreach (var body in bodies.Values)
            {
                int direct = body.CountDirectOrbits();
                int indirect = body.CountIndirectOrbits();
                results.Add(Tuple.Create(body.Name, direct + indirect));
            }

            return results.Sum(res => res.Item2);
        }

        private static Dictionary<string, Body> GetBodyDict(IEnumerable<Orbit> orbits)
        {
            var dict = new Dictionary<string, Body>();
            //< Generate the original map of Celestial Bodies
            foreach (var orbit in orbits)
            {
                if (!dict.ContainsKey(orbit.Source))
                {
                    dict.Add(orbit.Source, new Body(orbit.Source));
                }

                if (!dict.ContainsKey(orbit.Orbiter))
                {
                    dict.Add(orbit.Orbiter, new Body(orbit.Orbiter));
                }
            }

            //< Then, we need to go through all the orbits and populate the orbits/connections
            foreach (var orbit in orbits)
            {
                dict[orbit.Source].AddConnection(dict[orbit.Orbiter]);
                dict[orbit.Orbiter].SetSource(dict[orbit.Source]);
            }

            //< Return the final dict
            return dict;
        }

        public static int GetOrbitalTransfers(string sourceBodyName, string targetBodyName, IEnumerable<Orbit> orbits)
        {
            //< Get the map (string->Body) of all Celestial Bodies in this orbit sequence
            var bodies = GetBodyDict(orbits);

            //< Get the source/target bodies
            var sourceBody = bodies[sourceBodyName];
            var targetBody = bodies[targetBodyName];

            //< Get all of the celestial bodies that these bitches be orbitin'
            var sourceBodies = sourceBody.GetAllBodies();
            var targetBodies = targetBody.GetAllBodies();

            //< Find the maximum-depth-shared-body, get total transfers from each body to that branch point
            var maxShared = GetMaximumDepthSharedBody(sourceBodies, targetBodies);
            var sourceDiff = GetTotalTransfers(sourceBody, maxShared);
            var targetDiff = GetTotalTransfers(targetBody, maxShared);
            
            //< Combine total transfers, return
            return sourceDiff + targetDiff;
        }

        private static Body GetMaximumDepthSharedBody(IEnumerable<Body> sourceBodies, IEnumerable<Body> targetBodies)
        {
            Body currBody = null;
            int currDepth = int.MinValue;

            foreach (var body in sourceBodies.Where(b => b != null))
            {
                if (targetBodies.Contains(body))
                {
                    if (body.Depth > currDepth)
                    {
                        currBody = body;
                        currDepth = body.Depth;
                    }
                }
            }

            return currBody;
        }

        private static int GetTotalTransfers(Body source, Body target)
        {
            return (source.Depth - 1) - target.Depth;
        }
    }
}
