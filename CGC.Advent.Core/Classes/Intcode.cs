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
        public int[] Result { get; private set; } = null;

        public int Length => Source.Length;

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

        public void Process()
        {
            //< How process? Start at pos 0 and go up in fours boiii
            for (int i = 0; i < this.Source.Length; i += Opcode.Length)
            {
                //< Check that we can parse an Opcode out of this
                if (i + Opcode.Length >= this.Source.Length)
                {
                    break;
                }

                //< Get the current Opcode
                var opcode = new Opcode(this.Source, i);
                
                //< Handle
                switch (opcode.Type)
                {
                    case OpcodeType.Add:
                    case OpcodeType.Multiply:
                        HandleOpcode(opcode);
                        break;
                    case OpcodeType.Halt:
                        return;
                    default:
                        throw new NotImplementedException(); //< Shouldn't happen?
                }
            }
        }

        private void HandleOpcode(Opcode op)
        {
            //< Get the source values
            int A = this.GetValue(op.FromA);
            int B = this.GetValue(op.FromB);

            //< Get the final values
            int val = -1;
            if (op.Type == OpcodeType.Add)
            {
                val = A + B;
            }
            else if (op.Type == OpcodeType.Multiply)
            {
                val = A * B;
            }

            //< Set the value in the 'Source' array
            this.SetValue(op.To, val);
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
    }
}
