using System.Collections.Generic;

using Godot;

using PichaLib;
using OctavianLib;

public delegate void LayerChangedHandler(GenLayer Layer);
public delegate void LayerNameChangedHandler(string name);

public class GenLayer : TextureRect
{
    public LayerChangedHandler LayerChanged;
    public LayerNameChangedHandler LayerNameChanged;

    private Timer _Timer;
    private SortedList<int, Texture> _Textures = new SortedList<int, Texture>();

    private bool _Hover;
    public bool Hover {
        get => this._Hover;
        set {
            this._Hover = value;

            if(value) 
            { 
                this.Modulate = new Color(.3f, .3f, .3f, 1f); 
                this.GetTree().CallGroup("LayerButtons", "OnLayerHover", true, this);
            }
            else 
            { 
                this.Modulate = new Color(1f, 1f, 1f, 1f); 
                this.GetTree().CallGroup("LayerButtons", "OnLayerHover", false, this);
            }
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
            this.LayerChanged?.Invoke(this);
        }
    }

    public SortedList<int, string[,]> Frames {
        get => this.Data.Frames;
        set {
            this.Data.Frames = value;
            this.RectSize = (this.Data.Size).ToVector2();
            this.LayerChanged?.Invoke(this);
        }
    }

    public Dictionary<string, Pixel> Pixels {
        get => this.Data.Pixels;
        set {
            this.Data.Pixels = value;
            this.LayerChanged?.Invoke(this);
        }
    }

    public SortedList<int, Cycle> Cycles {
        get => this.Data.Cycles;
        set {
            this.Data.Cycles = value;
            this.LayerChanged?.Invoke(this);
        }
    }

    public float AnimTime {
        get => this.Data.AnimTime;
        set {
            this.Data.AnimTime = value;
            this.FrameTime = value / this._Textures.Count;
            this.LayerChanged?.Invoke(this);
        }
    }

    public string LayerName {
        get => this.Data.Name;
        set {
            this.Data.Name = value;
            this.LayerNameChanged?.Invoke(value);
        }
    }

    public bool MirrorX {
        get => this.Data.MirrorX;
        set {
            this.Data.MirrorX = value;
            this.LayerChanged?.Invoke(this);
        }
    }

    public bool MirrorY {
        get => this.Data.MirrorY;
        set {
            this.Data.MirrorY = value;
            this.LayerChanged?.Invoke(this);
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

    public string ChangePixelName(Pixel p, string n)
    {
        string _oldName = p.Name;
        string _newName = n;

        if(this.Pixels.ContainsKey(_newName))
        {
            int _num = 1;
            while(this.Pixels.ContainsKey($"{n}{_num}"))
            {
                _num = _num + 1;
            }

            _newName = $"{n}{_num}";
        }

        this.Pixels.Remove(_oldName);
        this.Pixels.Add(_newName, p);

        p.Name = _newName;        
        
        this.Data.Frames = this._RenamePixelFrames(_oldName, _newName);

        foreach(KeyValuePair<int, Cycle> cycle in this.Cycles)
        {
            foreach(Policy policy in cycle.Value.Policies)
            {
                policy.RenamePixel(_oldName, _newName);
            }
        }

        return _newName;
    }

    public Pixel NewPixel()
    {
        var _num = this.Pixels.Count;

        while(this.Pixels.ContainsKey($"Pixel_{_num}"))
        {
            _num = _num + 1;
        }

        var _newPixelName = $"Pixel_{_num}";
        var _newPixel = PDefaults.Pixel;
        _newPixel.Name = _newPixelName;

        this.Data.Pixels.Add(_newPixelName, _newPixel);

        return _newPixel;
    }

    /// <summary>
    /// Deletes pixels from the layer, accounting for affected policies and cycles.
    /// </summary>
    /// <param name="p">Pixel being deleted from the layer.</param>
    public void DeletePixel(Pixel p)
    {
        this.Pixels.Remove(p.Name);
        this._RemovePixelCycles(p);
        this.Frames = this._RenamePixelFrames(p.Name, Pixel.NULL);
    }

    private void _RemovePixelCycles(Pixel p)
    {
        var _badCycles = new List<int>();
        foreach(KeyValuePair<int, Cycle> _c in this.Cycles)
        {
            var _badPolicies = new List<Policy>();
            foreach(Policy _pol in _c.Value.Policies)
            {
                if(_pol.HasPixel(p))
                {
                    _badPolicies.Add(_pol);
                }
            }

            foreach(Policy _del in _badPolicies)
            {   
                _c.Value.Policies.Remove(_del);
            }

            if(_c.Value.Policies.Count <= 0)
            {
                _badCycles.Add(_c.Key);
            }
        }

        foreach(int i in _badCycles)
        {
            this.Cycles.Remove(i);
        }
    }

    private SortedList<int, string[,]> _RenamePixelFrames(string oldName, string newName)
    {
        var _newFrames = new SortedList<int, string[,]>();

        foreach(KeyValuePair<int, string[,]> frame in this.Frames)
        {
            _newFrames.Add(frame.Key, frame.Value.ReplaceVal(oldName, newName));
        }

        return _newFrames;
    }
}
