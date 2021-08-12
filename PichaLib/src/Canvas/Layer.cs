using System.Linq;
using System.Collections.Generic;

using OctavianLib;


namespace PichaLib
{

    public delegate void LayerChangeHandler(Layer layer, bool major);

    public class Layer
    {
        public event LayerChangeHandler LayerChanged;

        private string _Name;
        public string Name {
            get => this._Name;
            set {
                this._Name = value;
                this.LayerChanged?.Invoke(this, false);
            }
        }

        private float _AnimTime;
        public float AnimTime {
            get => this._AnimTime;
            set {
                this._AnimTime = value;
                this.LayerChanged?.Invoke(this, false);
            }
        }

        private bool _MirrorX, _MirrorY;
        public bool MirrorX {
            get => this._MirrorX;
            set {
                this._MirrorX = value;
                this.LayerChanged?.Invoke(this, true);
            }
        }
        public bool MirrorY {
            get => this._MirrorY;
            set {
                this._MirrorY = value;
                this.LayerChanged?.Invoke(this, true);
            }
        }

        private int _X, _Y;
        public int X {
            get => this._X;
            set {
                this._X = value;
                this.LayerChanged?.Invoke(this, false);
            }
        }
        public int Y {
            get => this._Y;
            set {
                this._Y = value;
                this.LayerChanged?.Invoke(this, false);
            }
        }

        public (int w, int h) Size {
            get {
                foreach(string[,] f in this._Frames)
                {
                    return (f.GetWidth(), f.GetHeight());
                }
                return (0, 0);
            }
        }
        
        public (int x, int y) Position {
            get => (this.X, this.Y);
            set {
                if(this.X != value.x || this.Y != value.y)
                {
                    this._X = value.x;
                    this._Y = value.y;
                    this.LayerChanged?.Invoke(this, false);
                }
            }
        }

        private List<string[,]> _Frames = new List<string[,]>();
        public List<string[,]> Frames {
            get => this._Frames;
            set {
                this._Frames = value;
                this.LayerChanged?.Invoke(this, true);
            }
        }

        private Dictionary<string, Pixel> _Pixels = new Dictionary<string, Pixel>();
        public Dictionary<string, Pixel> Pixels {
            get => this._Pixels;
            set {
                this._Pixels = value;
                this.LayerChanged?.Invoke(this, false);
            }
        }
        
        private List<Cycle> _Cycles = new List<Cycle>();
        public List<Cycle> Cycles {
            get => this._Cycles;
            set {
                this._Cycles = value;
                this.LayerChanged?.Invoke(this, true);
            }
        }
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