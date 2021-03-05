using System.Collections.Generic;

namespace PichaLib
{
    public class Canvas
    {
        public SortedDictionary<int, Layer> Layers = new SortedDictionary<int, Layer>();

        public int Width;
        public int Height;
        public int X;
        public int Y;
    }
}