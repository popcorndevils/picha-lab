using System.Collections.Generic;

using Godot;

using PichaLib;

public class GenLayer : TextureRect
{
    private Timer _Timer;
    private SortedList<int, Texture> _Textures = new SortedList<int, Texture>();

    private bool _Hover;
    public bool Hover {
        get => this._Hover;
        set {
            this._Hover = value;

            if(value) { this.Modulate = new Color(.3f, .3f, .3f, 1f); }
            else { this.Modulate = new Color(1f, 1f, 1f, 1f); }
        }
    }

    private bool _Dragging;
    private bool Dragging {
        get => this._Dragging;
        set {
            this._Dragging = value;
            if(!value)
                { this.MouseDefaultCursorShape = CursorShape.PointingHand; }
            else
                { this.MouseDefaultCursorShape = CursorShape.Move; }
        }
    }

    private float _FrameTime = .5f;
    public float FrameTime {
        get => this._FrameTime;
        set {
            this._FrameTime = value;
            this._Timer.WaitTime = value;
        }
    }

    private int _Frame;
    public int Frame {
        get => this._Frame;
        set {
            this._Frame = value;
            this.Texture = this._Textures[value];
            this.RectSize = this._Textures[value].GetSize();
        }
    }

    //*************************\\ 
    // ** ACCESS DATA LAYER ** \\
    //*************************\\ 

    public Layer _Data = new Layer();
    public Layer Data {
        get {
            this._Data.X = (int)this.RectPosition.x;
            this._Data.Y = (int)this.RectPosition.y;
            return this._Data;
        }
        set {
            this._Data = value;
            this.RectPosition = value.Position.ToVector2();
            this.RectSize = value.Size.ToVector2();
        }
    }

    public SortedList<int, string[,]> Frames {
        get => this.Data.Frames;
        set {
            this.Data.Frames = value;
            this.RectSize = (this.Data.Size).ToVector2();
            this.Generate();
        }
    }

    public Dictionary<string, Pixel> Pixels {
        get => this.Data.Pixels;
        set => this.Data.Pixels = value;
    }

    public SortedList<int, Cycle> Cycles {
        get => this.Data.Cycles;
        set => this.Data.Cycles = value;
    }

    public float AnimTime {
        get => this.Data.AnimTime;
        set {
            this.Data.AnimTime = value;
            this.FrameTime = value / this._Textures.Count;
        }
    }
    
    //******************\\ 
    // ** OPERATIONS ** \\
    //******************\\ 

    public override void _Ready()
    { 
        this._Timer = new Timer() {
            Autostart = true,
            WaitTime = this.FrameTime,
        }; 

        this.AddChild(this._Timer);

        this.Connect("mouse_entered", this, "OnMouseIn");
        this.Connect("mouse_exited", this, "OnMouseOut");
        this._Timer.Connect("timeout", this, "OnChange");
        this._Timer.Start();

        this.MouseDefaultCursorShape = CursorShape.PointingHand;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton btn)
        {
            if(btn.ButtonIndex == (int)ButtonList.Left)
            {
                if(btn.Pressed && this.Hover)
                {
                    this.Dragging = true;
                    this.GetTree().CallGroup("gp_layer_gui", "LoadLayer", this);
                }
                else
                {
                    this.Dragging = false;
                }
            } 
        }
        else if(@event is InputEventMouseMotion mtn)
        {
            if(this.Dragging)
            {
                this.RectPosition += mtn.Relative;
            }
        }
    }

    public void OnMouseIn() { this.Hover = true; }
    public void OnMouseOut() { this.Hover = false; }

    public void OnChange()
    { 
        if(this._Textures.Count > 0)
        {
            this.Frame = this.Frame >= this._Textures.Count - 1 ? 0 : this.Frame + 1;
        }
    }

    public void Generate()
    {
        if(this.Data != null)
        {
            this._Textures.Clear();

            foreach(KeyValuePair<int, Chroma[,]> _p in PFactory.ProcessLayer(this.Data))
                { this._Textures.Add(_p.Key, _p.Value.ToGodotTex()); }

            if(this._Textures.Count > 0)
            {
                this.Texture = this._Textures[0];
                this._Frame = 0;
                if(this.IsInsideTree()) { this._Timer.Start(); }
            }
        }
    }

    public void DeletePixel(Pixel p)
    {
        foreach(Cycle _c in this.Cycles.Values)
        {
            foreach(Policy _p in _c.Policies)
            {
                _p.VoidValue(p);
            }
        }
    }
}
