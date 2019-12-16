using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CGC.Advent.Core.Classes
{
    public class FlawedFrequency
    {
        private static readonly int[] _BasePattern = new int[] { 0, 1, 0, -1 };

        public static string CalculatePartOne(string source, int phases)
        {
            var signal = ParseSignal(source);
            for (int p = 0; p < phases; p++)
            {
                for (int j = 0; j < signal.Count; j++)
                {
                    int digit = 0;
                    for (int k = 0; k < signal.Count; k++)
                    {
                        digit += signal[k] * GetMultiplier(j + 1, k);
                    }

                    signal[j] = Math.Abs(digit) % 10;
                }
            }

            return string.Join("", signal.Take(8));
        }

        public static string CalculatePartTwo(string source, int phases)
        {
            var signal = ParseSignal(source);
            int offset = GetOffset(source);
            int times = (int)Math.Ceiling((double)(signal.Count * 10000 - offset) / (double)signal.Count);

            var realSignal = Repeat(source, times); //< Need to slice this down
            signal = ParseSignal(realSignal).Skip(offset % source.Length).ToList();
            for (int p = 0; p < phases; p++)
            {
                for (int j = signal.Count - 2; j >= 0; j--)
                {
                    var digit = signal[j] + signal[j + 1];
                    signal[j] = Math.Abs(digit) % 10;
                }
            }

            return string.Join("", signal.Take(8));
        }

        private static int GetMultiplier(int num, int idx)
        {
            return _BasePattern[(int)Math.Floor(((double)idx + 1) % (4 * (double)num) / num)];
        }

        private static string Repeat(string source, int times)
        {
            var str = "";
            for (int i = 0; i < times; i++)
            {
                str += source;
            }
            return str;
        }

        private static List<int> ParseSignal(string signal)
        {
            return signal.Select(c => int.Parse(c.ToString())).ToList();
        }

        public static int GetOffset(string signal)
        {
            return int.Parse(string.Join("", signal.Take(7)));
        }
    }
}
