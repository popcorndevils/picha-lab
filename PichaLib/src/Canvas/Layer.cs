using System.Collections.Generic;

namespace PichaLib
{
    public class Layer
    {
        public string Name;
        public int X;
        public int Y;
        public SortedList<int, int[,]> Frames;
        public Dictionary<int, Pixel> Pixels;
        public SortedList<int, Cycle> Cycles;
        
        public bool MirrorX;
        public bool MirrorY;

        // for app only
        public float AnimTime;
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