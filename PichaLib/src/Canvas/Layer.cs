using System;
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

        public (int W, int H) Size {
            get {
                foreach(string[,] f in this._Frames)
                {
                    return (f.GetWidth() * (this.MirrorX ? 2 : 1) ,
                            f.GetHeight() * (this.MirrorY ? 2 : 1));
                }
                return (0, 0);
            }
        }
        
        public (int X, int Y) Position {
            get => (this.X, this.Y);
            set {
                if(this.X != value.X || this.Y != value.Y)
                {
                    this._X = value.X;
                    this._Y = value.Y;
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