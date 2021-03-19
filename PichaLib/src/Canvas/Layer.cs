using System.Collections.Generic;

namespace PichaLib
{
    public class Layer
    {
        public string Name;
        public int X;
        public int Y;
        public SortedList<int, string[,]> Frames = new SortedList<int, string[,]>();
        public Dictionary<string, Pixel> Pixels = new Dictionary<string, Pixel>();
        public SortedList<int, Cycle> Cycles = new SortedList<int, Cycle>();
        
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