using System;
using System.Collections.Generic;

namespace PichaLib
{
    public class Canvas
    {
        public List<Layer> Layers = new List<Layer>();
        public (int W, int H) Size = (16, 16);
        public int[] FrameCounts {
            get {
                var _val = new int[this.Layers.Count];
                for(int i = 0; i < this.Layers.Count; i++)
                {
                    _val[i] = this.Layers[i].Frames.Count;
                }
                return _val;
            }
        }

        // useful for the app only.
        public bool AutoGen = false;
        public float TimeToGen = 1f;
        public Chroma TransparencyFG = Chroma.CreateFromHex("#298c8c8c");
        public Chroma TransparencyBG = new Chroma(.1f, .1f, .1f, 0f);
    }
}