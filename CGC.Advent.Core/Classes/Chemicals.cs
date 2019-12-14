using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CGC.Advent.Core.Classes
{
    public class Chemical
    {
        public long Amount { get; set; } = 0;
        public string Name { get; private set; } = null;

        public Chemical(string input)
        {
            var arr = input.Split(' ');
            this.Amount = long.Parse(arr[0]);
            this.Name = arr[1];
        }

        public Chemical(string name, long amount)
        {
            this.Name = name;
            this.Amount = amount;
        }

        public static Chemical operator *(Chemical a, long mult)
        {
            return new Chemical(a.Name, mult * a.Amount);
        }

        public override string ToString()
        {
            return $"{Amount} {Name}";
        }
    }

    public class ChemicalReaction
    {
        public Chemical Output { get; private set; } = null;
        public List<Chemical> Inputs { get; private set; } = null;
        
        public ChemicalReaction(string line)
        {
            //< Split the reaction into input/output segments
            var arr = line.Split(new string[] { " => " }, StringSplitOptions.None);

            //< Parse the inputs
            var inputs = arr[0].Split(new string[] { ", " }, StringSplitOptions.None);
            this.Inputs = inputs.Select(inp => new Chemical(inp)).ToList();

            //< Parse the output
            this.Output = new Chemical(arr[1]);
        }

        public bool Produces(Chemical chem)
        {
            return this.Output.Name == chem.Name;
        }

        public override string ToString()
        {
            return $"{string.Join(", ", this.Inputs)} => {this.Output}";
        }
    }

    public class NanoFactory
    {
        private const string OreName = "ORE";
        private const string FuelName = "FUEL";
        
        public List<ChemicalReaction> Reactions { get; } = null;

        public long OreRequired = 0;
        public Dictionary<string, long> Surplus { get; set; } = null;

        public NanoFactory(string source)
        {
            this.Surplus = new Dictionary<string, long>();
            this.Reactions = ParseReactions(source);
        }

        private static List<ChemicalReaction> ParseReactions(string source)
        {
            string[] lines = null;
            if (source.EndsWith(".txt"))
            {
                //< Parse file
                lines = File.ReadAllLines(source);
            }
            else
            {
                //<< Parse string
                lines = source.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }

            //< TODO :: inline this
            var reactions = lines.Select(ln => new ChemicalReaction(ln)).ToList();
            return reactions;
        }

        public void ProduceChemical(Chemical c)
        {
            if (c.Name == OreName)
            {
                this.OreRequired += c.Amount;
                return;
            }

            //< See if there's anything laying about in the Surplus we can use
            if (Surplus.ContainsKey(c.Name))
            {
                //< If the surplus has enough to satisfy this request, use it and return
                if (Surplus[c.Name] >= c.Amount)
                {
                    Surplus[c.Name] -= c.Amount;
                    return;
                }
                else
                {
                    //< Not enough, so use what we have and make the rest
                    c.Amount -= Surplus[c.Name];
                    Surplus[c.Name] = 0;
                }
            }

            //< Get the ChemicalReaction that produces this shit
            var reaction = GetReactionToMake(c);
            //< Get the number of times to do this reaction
            int mult = (int)Math.Ceiling((double)c.Amount / (double)reaction.Output.Amount);

            //< Create each required input (scaled by the multiplier)
            foreach (var input in reaction.Inputs)
            {
                ProduceChemical(new Chemical(input.Name, input.Amount * mult));
            }

            //< Check if we can add anything to the surplus
            AddToSurplus(reaction.Output.Amount * mult, c);
        }

        private void AddToSurplus(long amountMade, Chemical requested)
        {
            if (amountMade > requested.Amount)
            {
                var surp = amountMade - requested.Amount;
                if (Surplus.ContainsKey(requested.Name))
                {
                    Surplus[requested.Name] += surp;
                }
                else
                {
                    Surplus.Add(requested.Name, surp);
                }
            }
        }

        private ChemicalReaction GetReactionToMake(Chemical c)
        {
            var reaction = this.Reactions.SingleOrDefault(react => react.Produces(c));
            if (reaction != null)
                return reaction;
            else
                throw new Exception($"Boi, you done found a chemical that ain't got a reaction: {c}");
        }
    }

    public class NanoFactoryHelper
    {
        public static long FindMaximumFuelWithOre(string source, long oreCount)
        {
            var fact = new NanoFactory(source);
            long fuelCount = 0;
            var prodFact = 1000;

            Dictionary<string, long> oldSurp = null;
            long oldOreReq = 0;

            while (prodFact >= 1)
            {
                while (fact.OreRequired < oreCount)
                {
                    oldSurp = new Dictionary<string, long>(fact.Surplus);
                    oldOreReq = fact.OreRequired;
                    fact.ProduceChemical(new Chemical("FUEL", prodFact));
                    fuelCount += prodFact;
                }

                if (prodFact >= 1)
                {
                    /*reset old state*/
                    fact.Surplus = new Dictionary<string, long>(oldSurp);
                    fact.OreRequired = oldOreReq;
                    fuelCount -= prodFact;
                    prodFact /= 10;
                }
            }

            return fuelCount;
        }
    }
}
