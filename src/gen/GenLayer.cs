using SysDraw = System.Drawing;
using System.Linq;
using System.Collections.Generic;

using Godot;

using PichaLib;
using OctavianLib;

public delegate void FrameChangeHandler(int current, int total);

public class GenLayer : TextureRect
{
    public LayerChangeHandler LayerChanged;
    public FrameChangeHandler FrameChange;

    public GenCanvas Parent;

    private Timer _Timer;
    private List<Texture> _Textures = new List<Texture>();
    public Dictionary<string, Pixel> Pixels => this.Parent.Pixels;

    private bool _Hover;
    public bool Hover {
        get => this._Hover;
        set {
            this._Hover = value;

            if(value) 
            { 
                this.Modulate = new Color(.75f, .75f, .75f, 1f); 
                this.GetTree().CallGroup("layerstack", "OnLayerHover", true, this);
            }
            else 
            { 
                this.Modulate = new Color(1f, 1f, 1f, 1f); 
                this.GetTree().CallGroup("layerstack", "OnLayerHover", false, this);
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
            if(this._Timer != null)
            {
                this._Timer.WaitTime = value;
            }
        }
    }

    private int _Frame;
    public int Frame {
        get => this._Frame;
        set {
            this._Frame = value;
            this.Texture = this._Textures[value];
            this.RectSize = this._Textures[value].GetSize();
            this.FrameChange?.Invoke(value, this.FrameCount);
        }
    }

    public Vector2 _Position;
    public Vector2 Position {
        get => this._Position;
        set {
            this._Position = value;
            this.RectPosition = new Vector2((int)value.x, (int)value.y);
        }
    }

    //*************************\\ 
    // ** ACCESS DATA LAYER ** \\
    //*************************\\ 

    public Layer _Data;
    public Layer Data {
        get => this._Data;
        set {
            if(this._Data != null)
            {
                this._Data.LayerChanged -= this.OnLayerChanged;
            }
            this._Data = value;
            this.Position = value.Position.ToVector2();
            this.RectSize = value.Size.ToVector2();
            this._Data.LayerChanged += this.OnLayerChanged;
        }
    }

    public int FrameCount => this.Data.FramesCount;

    public List<Frame> Frames {
        get => this.Data.Frames;
        set {
            this.Data.Frames = value;
            this.RectSize = (this.Data.Size).ToVector2();
            this.FrameChange?.Invoke(0, this.FrameCount);
        }
    }

    public List<Cycle> Cycles {
        get => this.Data.Cycles;
        set {
            this.Data.Cycles = value;
        }
    }

    public string LayerName {
        get => this.Data.Name;
        set {
            this.Data.Name = value;
        }
    }

    public bool MirrorX {
        get => this.Data.MirrorX;
        set {
            this.Data.MirrorX = value;
        }
    }

    public bool MirrorY {
        get => this.Data.MirrorY;
        set {
            this.Data.MirrorY = value;
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
                    this.GetTree().CallGroup("inspector", "LoadLayer", this);
                }
                else if(!btn.Pressed && this.Dragging)
                {
                    this.Dragging = false;
                    this.Data.Position = this.RectPosition.ToIntPair();
                }
            } 
        }
        else if(@event is InputEventMouseMotion mtn)
        {
            if(this.Dragging)
            {
                this.Position += mtn.Relative;
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

    public void Generate(Dictionary<string, PixelColors> colors)
    {
        if(this.Data != null)
        {
            this._Textures.Clear();

            foreach(SysDraw.Bitmap _bit in this.Data.Generate(colors))
            { 
                this._Textures.Add(_bit.ToGodotTex());
            }

            if(this._Textures.Count > 0)
            {
                this.Texture = this._Textures[0];
                this.Frame = 0;
                if(this.IsInsideTree()) { this._Timer.Start(); }
            }
        }
    }

    public string ChangePixelName(string oldName, string newName)
    {
        foreach(Cycle cycle in this.Cycles)
        {
            foreach(Policy policy in cycle.Policies)
            {
                policy.RenamePixel(oldName, newName);
            }
        }

        this.Frames = this._RenamePixelFrames(oldName, newName);

        return newName;
    }

    public void SetAnimTime(float time)
    {
        this.Frame = 0;
        this.FrameTime = time / this._Textures.Count;
        if(this._Timer != null)
            { this._Timer.Start(); } 
    }

    /// <summary>
    /// Deletes pixels from the layer, accounting for affected policies and cycles.
    /// </summary>
    /// <param name="p">Pixel being deleted from the layer.</param>
    public void DeletePixel(Pixel p)
    {
        this._RemovePixelCycles(p);
        this.Frames = this._RenamePixelFrames(p.Name, Pixel.NULL);
    }

    public void DeleteCycle(Cycle c)
    {
        this.Cycles.Remove(c);
        // this.LayerChanged?.Invoke(this.Data, true);
    }

    public void DeletePolicy(Cycle c, Policy p)
    {
        c.Policies.Remove(p);
        // this.LayerChanged?.Invoke(this.Data, true);
    }

    public Cycle NewCycle()
    {
        var _num = this.Cycles.Count;

        var _output = new Cycle() {
            Name = $"Cycle_{_num}",
            Policies = new List<Policy>(),
        };

        this.Cycles.Add(_output);

        return _output;
    }

    public void MoveCycle(Cycle cycle, int location)
    {
        this.Cycles.Remove(cycle);
        this.Cycles.Insert(location, cycle);
        // this.LayerChanged?.Invoke(this.Data, true);
    }

    public Policy NewPolicy(Cycle c)
    {
        var _pix = this.Pixels[this.Pixels.Keys.First()].Name;

        var _output = new Policy(){
            Input = _pix,
            Output = _pix,
            Rate = 0f,
            ConditionA = ConditionTarget.NONE,
            ConditionLogic = ConditionExpression.NONE,
            ConditionB = _pix,
        };
        
        c.Policies.Add(_output);

        return _output;
    }

    public void _RemovePixelCycles(Pixel p)
    {
        var _badCycles = new List<Cycle>();

        foreach(Cycle _c in this.Cycles)
        {
            var _badPolicies = new List<Policy>();
            foreach(Policy _pol in _c.Policies)
            {
                if(_pol.HasPixel(p))
                {
                    _badPolicies.Add(_pol);
                }
            }

            foreach(Policy _del in _badPolicies)
            {   
                _c.Policies.Remove(_del);
            }

            if(_c.Policies.Count <= 0)
            {
                _badCycles.Add(_c);
            }
        }

        foreach(Cycle c in _badCycles)
        {
            this.Cycles.Remove(c);
        }
    }

    private List<Frame> _RenamePixelFrames(string oldName, string newName)
    {
        var _newFrames = new List<Frame>();

        foreach(Frame frame in this.Frames)
        {
            _newFrames.Add(new Frame() { Data = frame.Data.ReplaceVal(oldName, newName) });
        }

        return _newFrames;
    }

    public void OnLayerChanged(LayerChangeEvent _change)
    {
        this.LayerChanged?.Invoke(_change);
    }
}
