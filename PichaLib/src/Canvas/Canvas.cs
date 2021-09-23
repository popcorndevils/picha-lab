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

        public (int MinX, int MinY, int MaxX, int MaxY) Extents {
            get {
                var _minX = 0;
                var _minY = 0;
                var _maxX = this.Size.W;
                var _maxY = this.Size.H;
                foreach(Layer l in this.Layers)
                {
                    _minX = Math.Min(_minX, l.X);
                    _minY = Math.Min(_minY, l.Y);
                    _maxX = Math.Max(_maxX, l.X + l.Size.W);
                    _maxY = Math.Max(_maxY, l.Y + l.Size.H);
                }
                return (_minX, _minY, _maxX, _maxY);
            }
        }

        public (int W, int H) TrueSize {
            get {
                var _ext = this.Extents;
                return (_ext.MaxX - _ext.MinX, _ext.MaxY - _ext.MinY);
            }
        }

        // useful for the app only.
        public bool AutoGen = false;
        public float TimeToGen = 1f;
        public float AnimTime = 3f;
        public Chroma TransparencyFG = Chroma.CreateFromHex("#298c8c8c");
        public Chroma TransparencyBG = new Chroma(.1f, .1f, .1f, 0f);
    }
}