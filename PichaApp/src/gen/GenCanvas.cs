using System.Collections.Generic;

using Godot;

public class GenCanvas : Node2D
{
    private bool _AutoGen = false;
    public bool AutoGen {
        get => this._AutoGen;
        set {
            this._AutoGen = value;
            if(value) { this._Timer.Start(); }
            else { this._Timer.Stop(); }
        }
    }

    private float _TimeToGen = 3f;
    public float TimeToGen {
        get => this._TimeToGen;
        set {
            this._TimeToGen = value;
            this._Timer.WaitTime = value;
            if(this.AutoGen) { this._Timer.Start(); }
        }
    }


    private Timer _Timer;

    public Vector2 Size {
        get => this._BG.RectSize;
        set {
            this._BG.RectSize = value;
            this._FG.RectSize = value;
            this._FG.Texture = this._GetFG((int)value.x, (int)value.y);
        }
    }

    private SortedList<int, GenLayer> _Layers;
    public SortedList<int, GenLayer> Layers {
        get => this._Layers;
        set => this._Layers = value;
    }
    private ColorRect _BG;
    private TextureRect _FG;

    public Color BG {
        get => this._BG.Color;
        set => this._BG.Color = value;
    }

    public Color FG {
        get => this._FG.Modulate;
        set => this._FG.Modulate = value;
    }

    public override void _Ready()
    {
        this._Timer = new Timer() {
            WaitTime = this.TimeToGen
        };

        this.Layers = new SortedList<int, GenLayer>();

        this._BG = new ColorRect() {
            Color = new Color(1f, 1f, 1f, 1f),
            RectSize = new Vector2(16, 16),
        };

        this._FG = new TextureRect() {
            Texture = this._GetFG(16, 16),
            RectSize = new Vector2(16, 16),
        };

        this.AddChild(this._Timer);
        this.AddChild(this._BG);
        this.AddChild(this._FG);

        this.AddLayer(new GenLayer());

        this.FG = new Color(.1f, .1f, .1f, 1f);
        this.BG = new Color(.4f, .4f, .4f, 1f);

        this._Timer.Connect("timeout", this, "Generate");
        if(this.AutoGen) { this._Timer.Start(); }

        this.Generate();
    }

    public void AddLayer(Node n)
    {
        GenLayer l = n as GenLayer;

        this.Layers.Add(this.Layers.Count, l);
        this.AddChild(l);
    }

    public void Generate()
    {
        foreach(GenLayer _l in this.Layers.Values)
        {
            _l.Generate();
        }
    }
    
    private ImageTexture _GetFG(int w, int h)
    {
        var _t = new Color(1f, 1f, 1f, 0f);
        var _v = new Color(1f, 1f, 1f, 1f);

        var _im = new Image();
        var _imTex = new ImageTexture();

        _im.Create(w, h, false, Image.Format.Rgba8);
        _im.Lock();

        for(int y = 0; y < h; y++)
        {
            for(int x = 0; x < w; x++)
            {
                if(x % 2 == 0)
                {
                    if(y % 2 == 0)
                    {
                        _im.SetPixel(x, y, _v);
                    } 
                    else
                    {
                        _im.SetPixel(x, y, _t);
                    }
                } 
                else
                {
                    if(y % 2 != 0)
                    {
                        _im.SetPixel(x, y, _v);
                    }
                    else
                    {
                        _im.SetPixel(x, y, _t);
                    }
                }
            }
        }
        
        _imTex.CreateFromImage(_im, 0);

        return _imTex;
    }
}