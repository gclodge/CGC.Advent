using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGC.Advent.Core.Helpers
{
    public class AmpChain
    {

    }

    public class AmplifierHelper
    {
        const int NumAmplifiers = 5;
        public static int GetMaximumThrustValue(string intcodeSource)
        {
            //< Get all the possible permutations of the phase settings
            var phaseSettings = GetAllPossiblePhaseSettings(0);

            //< Return the maximum value returned by any of these phase settings
            return GetMaximumThrustValue(phaseSettings, intcodeSource);
        }

        public static int GetThrustValue(List<int> phaseSetting, string intcodeSource)
        {
            return GetMaximumThrustValue(new List<List<int>> { phaseSetting }, intcodeSource);
        }

        public static int GetMaximumFeedbackThrustValue(string intcodeSource)
        {
            //< Get all the possible permutations of the phase settings
            var phaseSettings = GetAllPossiblePhaseSettings(5);

            //< Return the maximum value returned by any of these phase settings
            return GetMaximumFeedbackThrust(phaseSettings, intcodeSource);
        }

        public static int GetFeedbackThrustValue(List<int> phaseSetting, string intcodeSource)
        {
            return GetMaximumFeedbackThrust(new List<List<int>> { phaseSetting }, intcodeSource);
        }

        private static int GetMaximumThrustValue(List<List<int>> phaseSettings, string intcodeSource)
        {
            //< Process each setting and get the result, retain all results
            var resultMap = new Dictionary<List<int>, int>();
            foreach (var phaseSetting in phaseSettings)
            {
                int input = 0;
                //< Iterate through each of the amplifiers and process the resulting code with the current phase setting
                for (int i = 0; i < NumAmplifiers; i++)
                {
                    var phase = phaseSetting[i];
                    var amp = new Classes.Amplifier(intcodeSource);
                    amp.AddInput(phase);
                    amp.AddInput(input);
                    amp.Process();

                    input = (int)amp.Output;
                }

                //< Then the 'final' value of 'input' is the final 'output'
                resultMap.Add(phaseSetting, input);
            }

            return resultMap.Max(kvp => kvp.Value);
        }

        private static int GetMaximumFeedbackThrust(List<List<int>> phaseSettings, string intcodeSource)
        {
            //< Process each setting and get the result, retain all results
            var resultMap = new Dictionary<List<int>, int>();
            foreach (var phaseSetting in phaseSettings)
            {
                //< First, make an array to house the Amplifiers
                var amps = new Classes.Amplifier[NumAmplifiers];
                for (int i = 0; i < NumAmplifiers; i++)
                {
                    amps[i] = new Classes.Amplifier(intcodeSource, feedbackLoop: true);
                    //< Add the phase as the first input
                    amps[i].AddInput(phaseSetting[i]);
                    if (i == 0)
                    {
                        amps[i].AddInput(0);
                    }
                }

                bool isFinished = false;
                while (!isFinished)
                {
                    for (int i = 0; i < NumAmplifiers; i++)
                    {
                        //< Get the index of the Amplifier this one is connected to
                        int connectedIdx = i < phaseSetting.Count - 1 ? i + 1 : 0;

                        //< Process until Halt or next WaitingForInput
                        amps[i].Process();

                        //< If we've got some new output, send it along
                        if (amps[i].Output != null)
                        {
                            //< Update the current input to the last Output
                            amps[connectedIdx].AddInput((int)amps[i].Output);
                        }
                    }

                    isFinished = amps.Last().IsHalted;
                }

                var output = amps.Last().Output;
                //< Then the 'final' value of 'input' is the final 'output'
                resultMap.Add(phaseSetting, (int)output);
            }

            return resultMap.Max(kvp => kvp.Value);
        }

        private static List<List<int>> GetAllPossiblePhaseSettings(int startVal)
        {
            //< Can only use values [0,1,2,3,4] - can never repeat
            //< Get all permutations of the array [0,1,2,3,4]
            var perms = GetPermutations(Enumerable.Range(startVal, NumAmplifiers), NumAmplifiers);
            //< Return them shits
            return perms.Select(p => p.ToList()).ToList();
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
