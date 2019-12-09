using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGC.Advent.Core.Classes
{
    public class Amplifier
    {
        public long? Output => GetOutput();

        public bool WaitingForInput => this.Intcode.WaitingForInput;
        public bool IsHalted => this.Intcode.IsFinished;

        public Intcode Intcode { get; private set; } = null;

        public Amplifier OutputAmp { get; set; } = null;

        public Amplifier(string intcodeSource, bool feedbackLoop = false)
        {
            this.Intcode = new Intcode(intcodeSource, feedbackLoop);
        }

        public void Process()
        {
            this.Intcode.Process();
            //< Check if we're halted or waiting
            //if (!this.IsHalted)
            //{
            //    //< We're waiting, so send the signal to the next one and set it running
            //    OutputAmp.AddInput(this.Output);
            //    OutputAmp.Process();
            //}
        }

        public void AddInput(int input)
        {
            this.Intcode.AddInput(input);
        }

        private long? GetOutput()
        {
            if (Intcode.Outputs.Count == 0)
            {
                return null;
            }
            else
            {
                return Intcode.Outputs.Last().Item2;
            }
        }
    }
}
