using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using CGC.Advent.Core.Helpers;

namespace CGC.Advent.Core.Classes
{
    public class Body
    {
        public string Name { get; private set; } = null;

        //< This is the body we're orbiting around, is left null if not orbiting
        public Body OrbitSource { get; private set; } = null;
        //< These are all the other bodies that directly orbit this body
        public List<Body> Orbiters { get; private set; } = null;

        public int Depth => GetAllBodies().Count;

        public Body(string name)
        {
            this.Name = name;
            this.Orbiters = new List<Body>();
        }

        public void SetSource(Body source)
        {
            this.OrbitSource = source;
        }

        public void AddConnection(Body orbiter)
        {
            this.Orbiters.Add(orbiter);
        }

        public int CountDirectOrbits()
        {
            return this.OrbitSource != null ? 1 : 0;
        }

        public int CountIndirectOrbits()
        {
            var bodies = GetAllBodies();
            return bodies.Count > 0 ? bodies.Count - 1 : 0; //< Subtracting one to account for our 'OrbitSource' not being in-direct
        }

        public List<Body> GetAllBodies()
        {
            //< These are tracing downwards from the OrbitSource - go until null
            if (this.OrbitSource != null)
            {
                var bodies = new List<Body>();
                Body currBody = this.OrbitSource;
                while (currBody != null)
                {
                    bodies.Add(currBody.OrbitSource);
                    currBody = currBody.OrbitSource;
                }
                return bodies;
            }
            else
                return new List<Body>();
        }

        public override bool Equals(object obj)
        {
            var other = (Body)obj;
            if (other != null)
            {
                return this.Name == other.Name;
            }
            else
                return false;
        }
    }

    public class Orbit
    {
        public string Source { get; private set; } = null;
        public string Orbiter { get; private set; } = null;

        public Orbit(string orbitStr)
        {
            var arr = orbitStr.Split(')');
            this.Source = arr[0];
            this.Orbiter = arr[1];
        }
    }
}
