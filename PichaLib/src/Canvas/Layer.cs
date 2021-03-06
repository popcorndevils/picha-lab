using System.Collections.Generic;

namespace PichaLib
{
    public class Layer
    {
        public string Name;
        public (int W, int H, int X, int Y) Locus;
        public SortedList<int, int[,]> Frames;
        public Dictionary<int, Pixel> Pixels;
        public SortedList<int, Dictionary<int, Policy>> Cycles;

        public FadeDirection FadeDirection;
        public bool MirrorX;
        public bool MirrorY;
        public float BrightNoise;
        public float MinSaturation;
    }

    public enum FadeDirection
    {
        NONE,
        NORTH,
        SOUTH,
        EAST,
        WEST,
        RANDOM
    }
}