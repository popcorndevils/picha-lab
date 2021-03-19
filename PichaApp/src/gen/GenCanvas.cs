using System.Collections.Generic;

using Godot;

using Newtonsoft.Json;
using PichaLib;

public class GenCanvas : Node2D
{
    public bool FileExists = false;
    public string FilePath = "";

    private bool _AutoGen = false;
    public bool AutoGen {
        get => this._AutoGen;
        set {
            this._AutoGen = value;
            if(_Timer != null)
            {
                if(value) { this._Timer.Start(); }
                else { this._Timer.Stop(); }
            }
        }
    }

    private float _TimeToGen = 3f;
    public float TimeToGen {
        get => this._TimeToGen;
        set {
            this._TimeToGen = value;
            if(_Timer != null)
            {
                this._Timer.WaitTime = value;
                if(this.AutoGen){ this._Timer.Start(); }
            }
        }
    }

    private Timer _Timer;

    private Vector2 _Size = new Vector2(16, 16);
    public Vector2 Size {
        get => this._Size;
        set {
            this._Size = value;
            if(this._BG != null)
            {
                this._BG.RectSize = value;
                this._FG.RectSize = value;
                this._FG.Texture = this._GetFG((int)value.x, (int)value.y);
            }
        }
    }

    private SortedList<int, GenLayer> _Layers = new SortedList<int, GenLayer>();
    public SortedList<int, GenLayer> Layers {
        get => this._Layers;
        set => this._Layers = value;
    }
    private ColorRect _BG;
    private TextureRect _FG;

    private Color _BGCol = new Color(.4f, .4f, .4f, 1f);
    public Color BG {
        get => this._BGCol;
        set {
            this._BGCol = value;
            if(this._BG != null)
                { this._BG.Modulate = value; }
        }
    }

    private Color _FGCol = new Color(.1f, .1f, .1f, 1f);
    public Color FG {
        get => this._FGCol;
        set {
            this._FGCol = value;
            if(this._FG != null)
                { this._FG.Modulate = value; }
        }
    }

    public override void _Ready()
    {
        this._Timer = new Timer() {
            WaitTime = this.TimeToGen,
        };

        this._BG = new ColorRect() {
            Color = new Color(1f, 1f, 1f, 1f),
            RectSize = this.Size,
            Modulate = this.BG,
        };

        this._FG = new TextureRect() {
            Texture = this._GetFG(this.Size),
            RectSize = this.Size,
            Modulate = this.FG,
        };

        this.AddChild(this._Timer);
        this.AddChild(this._BG);
        this.AddChild(this._FG);

        this.AddLayer(new GenLayer());

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
    
    private ImageTexture _GetFG(Vector2 s) { return this._GetFG((int)s.x, (int)s.y); }
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

    public void Save() 
    { 
        this._WriteFile(this.FilePath); 
    }

    public void SaveAsFile(string path)
    {
        this.FileExists = true;
        this.FilePath = path;
        this._WriteFile(path);
    }

    public void _WriteFile(string path)
        { System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(this.SaveData())); }

    public Canvas SaveData()
    {
        var _output = new Canvas() {
            Size = ((int)this.Size.x, (int)this.Size.y),
            AutoGen = this.AutoGen,
            TimeToGen = this.TimeToGen,
            TransparencyFG = this.FG.ToChroma(),
            TransparencyBG = this.BG.ToChroma(),
        };

        foreach(KeyValuePair<int, GenLayer> _pair in this.Layers)
        {
            _output.Layers.Add(_pair.Key, _pair.Value.Data);
        }

        return _output;
    }

    public void LoadData(Canvas c) 
    {
        this.Size = new Vector2(c.Size.W, c.Size.H);
        this.AutoGen = c.AutoGen;
        this.TimeToGen = c.TimeToGen;
        this.FG = c.TransparencyFG.ToGodotColor();
        this.BG = c.TransparencyBG.ToGodotColor();

        foreach(KeyValuePair<int, Layer> _pair in c.Layers)
        {
            var _l = new GenLayer();
            _l.Data = _pair.Value;
            this.AddLayer(_l);
        }
    }
}