using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CGC.Advent.Tests
{
    public class TestHelper
    {
        static Assembly Self = Assembly.GetExecutingAssembly();

        private const string _TestDir = "TestData";
        public static string TestDir => GetTestDirectoryRoot(_TestDir);

        public static string GetTestDirectoryRoot(string relativePath = null)
        {
            string[] hypotheticals = new[]
            {
                Path.Combine(Path.GetDirectoryName(Self.Location), @"..\..\.."),
                Path.Combine(Path.GetDirectoryName(Self.Location), @"..\..\..\..")
            };

            if (relativePath != null)
            {
                hypotheticals = hypotheticals.Select(x => Path.Combine(x, relativePath)).ToArray();
            }

            var exists = hypotheticals.Where(x => File.Exists(x) || Directory.Exists(x)).FirstOrDefault();
            return exists ?? null;
        }
    }
}
