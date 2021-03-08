using System;

namespace OctavianLib
{
    public static class RandomExtensions
    {
        public static double RandfRange(this Random r, float min, float max)
        {
            return r.NextDouble() * (max - min) + min; 
        }
    }
}