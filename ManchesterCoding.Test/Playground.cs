using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManchesterCoding.Test
{
    [TestClass]
    public class Playground
    {
        [TestMethod]
        public void TestMethod1()
        {
            uint banned = 0;
            uint allowed = 0;
            checked
            {
                for (uint i = 0; i <= ushort.MaxValue; i++)
                {
                    if (IsBanned(i))
                        banned++;
                    else
                        allowed++;
                }
            }

            Console.WriteLine(banned + " " + allowed);
        }

        private static bool IsBanned(uint value)
        {
            string binary = Convert.ToString(value, 2);

            return binary.Contains("0000") || binary.StartsWith("0");
        }
    }
}
