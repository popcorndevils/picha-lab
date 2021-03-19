using System.Collections.Generic;

namespace PichaLib
{
    public class Canvas
    {
        public SortedDictionary<int, Layer> Layers = new SortedDictionary<int, Layer>();
        public (int W, int H) Size;

        // useful for the app only.
        public bool AutoGen;
        public float TimeToGen;
        public Chroma TransparencyFG;
        public Chroma TransparencyBG;
    }
}