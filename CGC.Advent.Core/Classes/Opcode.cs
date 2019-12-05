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
        Input,
        Output,
        JumpIfTrue,
        JumpIfFalse,
        LessThan,
        Equals,
        Unknown
    }

    public enum ParameterMode
    {
        Position,
        Immediate,
        Unknown
    }

    public class OpcodeParameter
    {
        public int? Value { get; private set; } = null;
        public ParameterMode Mode { get; private set; } = ParameterMode.Unknown;

        public OpcodeParameter(int mode, int? val = null)
        {
            this.Mode = ParseMode(mode);
            this.Value = val;
        }

        private static ParameterMode ParseMode(int modeCode)
        {
            switch (modeCode)
            {
                case 0:
                    return ParameterMode.Position;
                case 1:
                    return ParameterMode.Immediate;
                default:
                    return ParameterMode.Unknown;
            }
        }
    }

    public class Opcode
    {
        public int[] Source { get; private set; } = null;

        public List<OpcodeParameter> Parameters { get; private set; } = null;

        public OpcodeParameter First { get; private set; } = null;
        public OpcodeParameter Second { get; private set; } = null;
        public OpcodeParameter Third { get; private set; } = null;

        public OpcodeType Type { get; private set; } = OpcodeType.Unknown;
        public int Length { get; private set; } = 1;
        public int NumParams => this.Length - 1;

        public int[] Instruction { get; private set; } = null;

        public Opcode(int[] intCode, int pos)
        {
            //< Get the 'instruction' array
            this.Instruction = GetInstructionArray(intCode[pos]);

            //< Get the functional type of this Opcode (and thus its length)
            this.Type = GetType(Instruction);
            this.Length = GetLength(Type);

            //< Pull the Opcode from the current position of the Intcode stream
            this.Source = new int[Length];
            Array.Copy(intCode, pos, this.Source, 0, Length);

            //< Generate the Parameters for this Opcode
            GetParameters();
        }

        private void GetParameters()
        {
            this.First  = new OpcodeParameter(this.Instruction[2], 1 > NumParams ? null : (int?)this.Source[1]);
            this.Second = new OpcodeParameter(this.Instruction[1], 2 > NumParams ? null : (int?)this.Source[2]);
            this.Third  = new OpcodeParameter(this.Instruction[0], 3 > NumParams ? null : (int?)this.Source[3]);
        }

        private static OpcodeType GetType(int[] instruction)
        {
            //< Glue the last two (right-most) values together to get the type code
            var typeValue = $"{instruction[instruction.Length - 2]}{instruction[instruction.Length - 1]}";
            var typeCode = int.Parse(typeValue);
            //< Return the OpcodeType
            switch (typeCode)
            {
                case 1:
                    return OpcodeType.Add;
                case 2:
                    return OpcodeType.Multiply;
                case 3:
                    return OpcodeType.Input;
                case 4:
                    return OpcodeType.Output;
                case 5:
                    return OpcodeType.JumpIfTrue;
                case 6:
                    return OpcodeType.JumpIfFalse;
                case 7:
                    return OpcodeType.LessThan;
                case 8:
                    return OpcodeType.Equals;
                case 99:
                    return OpcodeType.Halt;
                default:
                    return OpcodeType.Unknown;
            }
        }

        private const int MaxCheckVal = 5;
        private static int[] GetInstructionArray(int value)
        {
            var valStr = value.ToString().PadLeft(MaxCheckVal, '0');
            var arr = new int[MaxCheckVal];

            for (int i = 0; i < valStr.Length; i++)
            {
                arr[i] = int.Parse(valStr[i].ToString());
            }

            return arr;
        }

        private static int GetLength(OpcodeType type)
        {
            switch(type)
            {
                case OpcodeType.Add:
                case OpcodeType.Multiply:
                case OpcodeType.LessThan:
                case OpcodeType.Equals:
                    return 4;
                case OpcodeType.JumpIfTrue:
                case OpcodeType.JumpIfFalse:
                    return 3;
                case OpcodeType.Input:
                case OpcodeType.Output:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
