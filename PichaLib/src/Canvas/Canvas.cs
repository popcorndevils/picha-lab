using System.Collections.Generic;

namespace PichaLib
{
    public class Canvas
    {
        public SortedDictionary<int, Layer> Layers = new SortedDictionary<int, Layer>();
        public (int W, int H) Size;
    }
}