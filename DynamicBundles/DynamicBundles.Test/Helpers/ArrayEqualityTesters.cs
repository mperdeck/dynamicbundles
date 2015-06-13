using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicBundles.Test
{
    public class ArrayEqualityTesters
    {
        public static void AssertOneDimStringArraysEqual(string[] saa1, string[] saa2)
        {
            int saaLength = saa1.Length;
            Assert.AreEqual(saaLength, saa2.Length);

            for (int i = 0; i < saaLength; i++)
            {
                Assert.AreEqual(saa1[i], saa2[i]);
            }
        }

        public static void AssertTwoDimStringArraysEqual(string[][] saa1, string[][] saa2)
        {
            int saaLength = saa1.Length;
            Assert.AreEqual(saaLength, saa2.Length);

            for (int i = 0; i < saaLength; i++)
            {
                int saaLength2 = saa1[i].Length;
                Assert.AreEqual(saaLength2, saa2[i].Length);

                for (int i2 = 0; i2 < saaLength2; i2++)
                {
                    Assert.AreEqual(saa1[i][i2], saa2[i][i2]);
                }
            }
        }
    }
}
