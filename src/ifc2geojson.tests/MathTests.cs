using ifc2geojson.core;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ifc2geojson.tests
{
    internal class MathTests
    {
        [Test]
        public void RoundupTest()
        {
            double width = 349;

            if (width > 3)
            {
                width = width / 1000;
                width = RoundUp(width, 2);
            }
            Console.WriteLine(width);
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
