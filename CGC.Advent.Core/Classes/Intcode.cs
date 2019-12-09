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
        public long[] Source { get; private set; } = null;
        public int Length => Source.Length;

        private int _Index = 0;
        private int _CurrInput = 0;
        public List<long> Inputs { get; private set; } = new List<long>();

        public List<Opcode> Opcodes { get; private set; } = new List<Opcode>();
        public List<Tuple<Opcode, long>> Outputs { get; private set; } = new List<Tuple<Opcode, long>>();

        private bool WaitOnOutputs = false;
        public bool WaitingForInput { get; private set; } = false;
        public bool IsFinished { get; private set; } = false;

        public int RelativeBase { get; private set; } = 0;

        public Intcode(long[] source, bool waitOnOuputs = false)
        {
            this.Source = source;
            this.WaitOnOutputs = waitOnOuputs;
        }

        public Intcode(string source, bool waitOnOuputs = false)
        {
            string data = null;
            if (source.EndsWith(".txt"))
            {
                //< Parse all the input data into the Array
                data = File.ReadAllText(source);
            }
            else
            {
                //< Input data is already a string, just parse it
                data = source;
            }
            this.Source = GetSource(data.Split(',').Select(d => long.Parse(d)).ToArray());
            this.WaitOnOutputs = waitOnOuputs;
        }

        const int Mult = 1024;
        //< For Day9, we're told to include 'alot more memory than required'
        private static long[] GetSource(long[] sourceArr)
        {
            var data = new long[Mult * sourceArr.Length];
            Array.Copy(sourceArr, 0, data, 0, sourceArr.Length);
            return data;
        }

        public void AddInput(int inp)
        {
            this.Inputs.Add(inp);
            if (this.WaitingForInput)
            {
                this.WaitingForInput = false;
            }
        }

        public void SetRelativeBase(int relBase)
        {
            this.RelativeBase = relBase;
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
                {
                    this.IsFinished = true;
                    _Index = 0;
                    return;
                }
                else
                {
                    forward = HandleCode(opcode);
                }

                //< Iterate forward based on the Opcode's function
                _Index += forward;

                //< If we're marked as WaitingForInput, halt the process loop where it is (after adjusting the index)
                if (this.WaitingForInput)
                {
                    break;
                }

                //< Hold onto the Opcode for debug
                this.Opcodes.Add(opcode);
            }
        }

        public long? GetValue(OpcodeParameter param)
        {
            if (param.Value == null)
                throw new Exception("Tried to get value of 'null' OpcodeParameter!");

            switch (param.Mode)
            {
                case ParameterMode.Immediate:
                    return param.Value;
                case ParameterMode.Position:
                    return GetValue((int)param.Value);
                case ParameterMode.Relative:
                    return GetRelativeValue(param);
                default:
                    throw new Exception("Unknown Opcode ParameterMode encountered!");
            }
        }

        private long? GetRelativeValue(OpcodeParameter param)
        {
            var idx = (int)param.Value + this.RelativeBase;
            return GetValue(idx);
            //return long.MaxValue;
        }

        public long GetValue(int idx)
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

        public void SetValue(int idx, long value)
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
                case OpcodeType.AdjustBase:
                    return HandleRelativeBase(opcode);
                default:
                    throw new NotImplementedException(); //< Shouldn't happen?
            }
        }

        private int HandleArithmeticCode(Opcode op)
        {
            //< Get the source values
            var A = this.GetValue(op.First);
            var B = this.GetValue(op.Second);
            var C = op.Third.Value + (op.Third.Mode == ParameterMode.Relative ? this.RelativeBase : 0);

            //< Get the final values
            long? val = -1;
            if (op.Type == OpcodeType.Add)
            {
                val = A + B;
            }
            else if (op.Type == OpcodeType.Multiply)
            {
                val = A * B;
            }

            //< Set the value in the 'Source' array
            this.SetValue((int)C, (long)val);
            return op.Length;
        }

        private int HandleInputCode(Opcode op)
        {
            //< Querying for input from VB/Dialog/Console wasn't working, so being more lazy
            if (_CurrInput < this.Inputs.Count)
            {
                //< Get the current input
                var input = this.Inputs[_CurrInput];
                _CurrInput += 1;

                //< Need to handle things being relative
                int idx = (int)op.First.Value + (op.First.Mode == ParameterMode.Relative ? this.RelativeBase : 0);

                //< Set that input value to the specified position
                this.SetValue(idx, input);
                return op.Length;
            }
            else
            {
                //< We need to wait another Input, so we're gonna set 'WaitingForInput' and break the current process loop
                this.WaitingForInput = true;
                //< Returning zero since we want to re-use this input command at next Process()
                return 0;
            }
                
        }

        private int HandleOutputCode(Opcode op)
        {
            //< Get the source value (it's a position)
            var A = this.GetValue(op.First);

            //< Get the actual output value, add to the 'outputs' listing
            this.Outputs.Add(Tuple.Create(op, (long)A));

            //< Check if we're part of Amplifier in a feedback loop and thus gotta wait on Outputs
            if (this.WaitOnOutputs)
            {
                this.WaitingForInput = true;
            }

            //< Return the length to offset - still want to start on the next command at next Process()
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
                //SetValue(_Index, (long)B);
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
            var C = op.Third.Value + (op.Third.Mode == ParameterMode.Relative ? this.RelativeBase : 0);

            bool comparison = (op.Type == OpcodeType.LessThan) ? A < B : A == B;
            long result = comparison ? 1 : 0;
            SetValue((int)C, result);

            return op.Length;
        }

        private int HandleRelativeBase(Opcode op)
        {
            //< Get the source value
            var A = this.GetValue(op.First);
            //< Set the relative base to the value of the first parameter
            this.RelativeBase += (int)A;
            return op.Length;
        }
    }
}
