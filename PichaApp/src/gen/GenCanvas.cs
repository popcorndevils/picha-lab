using System.Collections.Generic;

using Godot;

using Newtonsoft.Json;
using PichaLib;

public delegate void CanvasChangedHandler(Canvas canvas);

public class GenCanvas : Node2D
{
    public CanvasChangedHandler GenCanvasChanged;

    private Canvas _Data;
    private Timer _Timer;
    private bool _FileSaved = false;
    private ColorRect _BG = new ColorRect();
    private TextureRect _FG = new TextureRect();
    private Color _BGCol = new Color(.1f, .1f, .1f, 0f);
    private Color _FGCol = Chroma.CreateFromHex("#298c8c8c").ToGodotColor();

    public string PathName = "";
    public bool FileExists = false;

    public Canvas Data {
        get => this._Data;
        set {
            this._Data = value;
            this._Timer.WaitTime = 1f;
            this.BG = value.TransparencyBG.ToGodotColor();
            this.FG = value.TransparencyFG.ToGodotColor();
            if(this.AutoGen) { this._Timer.Start(); }
            this._FG.Texture = this._GetFG(value.Size.W, value.Size.H);
        }
    }

    public bool FileSaved {
        get => this._FileSaved;
        set {
            if(value != this._FileSaved)
            {
                this._FileSaved = value;
                this.GetTree().CallGroup("gp_canvas_handler", "NameCurrentTab", this.CanvasName);
            }
        }
    }

    public string CanvasName {
        get { 
            var _output = "";

            if(this.FileExists)
                { _output += System.IO.Path.GetFileNameWithoutExtension(this.PathName); }
            else
                { _output += "[new canvas]"; }

            if(!this.FileSaved) 
                { _output += "*"; }

            return _output;
        }
    }

    public bool AutoGen {
        get {
            if(this.Data != null) 
                { return this.Data.AutoGen; }
            else 
                { return false; }
        }
        set {
            if(this.Data != null) 
            {
                this.Data.AutoGen = value;
                if(_Timer != null)
                {
                    if(value) 
                        { this._Timer.Start(); }
                    else 
                        { this._Timer.Stop(); }
                }
            }
        }
    }

    public float TimeToGen {
        get {
            if(this.Data != null) 
                { return this.Data.TimeToGen; }
            else 
                { return .1f; }
        }
        set {
            if(this.Data != null) 
            {
                this.Data.TimeToGen = value;
                if(_Timer != null)
                {
                    this._Timer.WaitTime = value;
                    if(this.AutoGen)
                        { this._Timer.Start(); }
                }
            }
        }
    }

    public Vector2 Size {
        get {
            if(this.Data != null) 
                { return this.Data.Size.ToVector2(); }
            else 
                { return new Vector2(16f, 16f); }
        }
        set {
            if(this.Data != null) 
                { this.Data.Size = value.ToIntPair(); }

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

    public Color BG {
        get => this._BGCol;
        set {
            this._BGCol = value;
            if(this._BG != null)
                { this._BG.Modulate = value; }
        }
    }

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

        this.AddChild(this._Timer);
        this.AddChild(this._BG);
        this.AddChild(this._FG);

        this._Timer.Connect("timeout", this, "Generate");
        if(this.AutoGen) { this._Timer.Start(); }

        this.Generate();
    }

    public void AddLayer(GenLayer l)
    {
        this.Layers.Add(this.Layers.Count, l);
        this.AddChild(l);
        l.LayerChanged += this.OnLayerChange;
        l.Generate();
        if(this.GetTree() != null)
            { this.GetTree().CallGroup("gp_layer_gui", "LoadLayer", l); }
        this.FileSaved = false;
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
        
        _im.Unlock();
        _imTex.CreateFromImage(_im, 0);

        return _imTex;
    }

    public void Save() 
    { 
        this._WriteFile(this.PathName); 
    }

    public void SaveAsFile(string path)
    {
        this.FileExists = true;
        this.PathName = path;
        this._WriteFile(path);
    }

    public void _WriteFile(string path)
    { 
        System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(this.SaveData()));
        this.FileSaved = true;
    }

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
        this.Data = c;

        foreach(KeyValuePair<int, Layer> _pair in c.Layers)
        {
            var _l = new GenLayer();
            _l.Data = _pair.Value;
            this.AddLayer(_l);
        }

        this.FileSaved = true;
    }

    public void OnLayerChange(Layer layer, bool major)
    {
        if(major)
        {
            this.Generate();
        }
        this.FileSaved = false;
    }
}