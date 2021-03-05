using System.Collections.Generic;

namespace PichaLib
{
    public class Layer
    {
        public SortedList<int, int[,]> Frames;
        public Dictionary<int, Pixel> Pixels;
        public SortedList<int, Dictionary<int, Policy>> Cycles;

        public int W;
        public int H;
        public int X;
        public int Y;
    }
}