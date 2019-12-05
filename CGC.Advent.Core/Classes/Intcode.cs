using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CGC.Advent.Core.Classes
{
    public class Intcode
    {
        public int[] Source { get; private set; } = null;
        public int Length => Source.Length;

        private int _Index = 0;
        public int Input { get; private set; } = 1;

        public List<Opcode> Opcodes { get; private set; } = new List<Opcode>();
        public List<Tuple<Opcode, int>> Outputs { get; private set; } = new List<Tuple<Opcode, int>>();

        public Intcode(int[] source)
        {
            this.Source = source;
        }

        public Intcode(string fileName)
        {
            //< Parse all the input data into the Array
            var data = File.ReadAllText(fileName);
            this.Source = data.Split(',').Select(d => int.Parse(d)).ToArray();
        }

        public void SetInput(int inp)
        {
            this.Input = inp;
        }

        public void Process()
        {
            //< So, we can't pre-parse all the Opcodes, have to go one-by-one as they alter the Source as we go
            while (_Index < this.Source.Length)
            {
                //< Get Opcode, find length, add to listing
                var opcode = new Opcode(this.Source, _Index);
                int forward = 0;

                if (opcode.Type == OpcodeType.Halt)
                    return;
                else
                {
                    forward = HandleCode(opcode);
                }

                //< Hold onto the Opcode for debug
                this.Opcodes.Add(opcode);

                //< Iterate forward based on the Opcode's function
                _Index += forward;
            }
        }

        public int? GetValue(OpcodeParameter param)
        {
            if (param.Value == null)
                throw new Exception("Tried to get value of 'null' OpcodeParameter!");

            switch (param.Mode)
            {
                case ParameterMode.Immediate:
                    return param.Value;
                case ParameterMode.Position:
                    return GetValue((int)param.Value);
                default:
                    throw new Exception("Unknown Opcode ParameterMode encountered!");
            }
        }

        public int GetValue(int idx)
        {
            //< Ensure it's within the Intcode bounds
            if (idx >= 0 && idx < this.Length)
            {
                return this.Source[idx];
            }
            else
            {
                throw new Exception($"Invalid index supplied, must be between [0,{this.Length - 1}], index: {idx}");
            }
        }

        public void SetValue(int idx, int value)
        {
            //< Ensure it's within the Intcode bounds
            if (idx >= 0 && idx < this.Length)
            {
                this.Source[idx] = value;
            }
            else
            {
                throw new Exception($"Invalid index supplied, must be between [0,{this.Length - 1}], index: {idx}");
            }
        }

        private int HandleCode(Opcode opcode)
        {
            //< Really shoulda used inheritance, but gotta stick with it now
            switch (opcode.Type)
            {
                case OpcodeType.Add:
                case OpcodeType.Multiply:
                    return HandleArithmeticCode(opcode);
                case OpcodeType.Input:
                    return HandleInputCode(opcode);
                case OpcodeType.Output:
                    return HandleOutputCode(opcode);
                case OpcodeType.JumpIfTrue:
                case OpcodeType.JumpIfFalse:
                    return HandleJumpCode(opcode);
                case OpcodeType.LessThan:
                case OpcodeType.Equals:
                    return HandleComparison(opcode);
                default:
                    throw new NotImplementedException(); //< Shouldn't happen?
            }
        }

        private int HandleArithmeticCode(Opcode op)
        {
            //< Get the source values
            var A = this.GetValue(op.First);
            var B = this.GetValue(op.Second);
            var C = op.Third.Value;

            //< Get the final values
            int? val = -1;
            if (op.Type == OpcodeType.Add)
            {
                val = A + B;
            }
            else if (op.Type == OpcodeType.Multiply)
            {
                val = A * B;
            }

            //< Set the value in the 'Source' array
            this.SetValue((int)C, (int)val);
            return op.Length;
        }

        private int HandleInputCode(Opcode op)
        {
            //< Querying for input from VB/Dialog/Console wasn't working, so being more lazy

            //< Set that input value to the specified position
            this.SetValue((int)op.First.Value, this.Input);
            return op.Length;
        }

        private int HandleOutputCode(Opcode op)
        {
            //< Get the source value (it's a position)
            var A = this.GetValue(op.First);

            //< Get the actual output value, add to the 'outputs' listing
            this.Outputs.Add(Tuple.Create(op, (int)A));
            return op.Length;
        }

        private int HandleJumpCode(Opcode op)
        {
            //< Get the source values
            var A = this.GetValue(op.First);
            var B = this.GetValue(op.Second);

            bool zeroCondition = (op.Type == OpcodeType.JumpIfTrue) ? A != 0 : A == 0;

            if (zeroCondition)
            {
                SetValue(_Index, (int)B);
                _Index = (int)B;
                return 0;
            }
            else
                return op.Length;
        }

        private int HandleComparison(Opcode op)
        {
            //< Get the source values
            var A = this.GetValue(op.First);
            var B = this.GetValue(op.Second);
            var C = op.Third.Value;

            bool comparison = (op.Type == OpcodeType.LessThan) ? A < B : A == B;
            int result = comparison ? 1 : 0;
            SetValue((int)C, result);

            return op.Length;
        }
    }
}
