using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CGC.Advent.Core.Classes
{
    public enum OpcodeType
    {
        Add,
        Multiply,
        Halt,
        Unknown
    }

    public class Opcode
    {
        public int[] Source { get; private set; } = null;

        public int FromA => Source[1];
        public int FromB => Source[2];
        public int To => Source[3];

        public OpcodeType Type { get; private set; } = OpcodeType.Unknown;

        public const int Length = 4;
        public Opcode(int[] intCode, int pos)
        {
            //< Pull the Opcode from the current position of the Intcode stream
            this.Source = new int[Length];
            //< TODO :: Do '99' Opcodes still have values after? Must we check?
            Array.Copy(intCode, pos, this.Source, 0, Length);

            //< Get the functional type of this Opcode
            this.Type = GetType(this.Source.First());
        }

        private static OpcodeType GetType(int firstValue)
        {
            switch (firstValue)
            {
                case 1:
                    return OpcodeType.Add;
                case 2:
                    return OpcodeType.Multiply;
                case 99:
                    return OpcodeType.Halt;
                default:
                    return OpcodeType.Unknown;
            }
        }
    }
}
