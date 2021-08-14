using System.Collections.Generic;

using Godot;

using PichaLib;
using OctavianLib;

public class PatternDesigner : AcceptDialog
{
    private GenLayer _Layer;
    public GenLayer Layer {
        get => this._Layer;
        set => this._Layer = value;
    }

    private bool _Editing = false;
    private bool _IgnoreSizeSignal = true;
    private CenterContainer FramesView;
    public PaintBtn PaintBtn;
    public SpinBox WEdit;
    public SpinBox HEdit;

    private Label _FrameIndex;
    private Button _NavPrev;
    private Button _NavNext;
    private Button _AddFrame;
    private Button _DelFrame;
    private Button _Accept;

    public int FrameCount => this.FramesView.GetChildren().Count;

    private int _CurrentFrame = 0;
    public int CurrentFrame {
        get => this._CurrentFrame;
        set {
            if(this.FramesView.GetChildCount() >= this._CurrentFrame + 1)
            {
                var _prevFrame = this.FramesView.GetChild<FrameControl>(this._CurrentFrame);
                if(_prevFrame != null)
                {
                    _prevFrame.Visible = false;
                }
            }
            var _frame = this.FramesView.GetChild<FrameControl>(value);
            _frame.Visible = true;
            this._CurrentFrame = value;
            this._FrameIndex.Text = $"Frame {value + 1}/{this.FrameCount}";
        }
    }

    public override void _Ready()
    {
        this.GetOk().Visible = false;

        this.AddToGroup("pattern_designer");
        this.Connect("about_to_show", this, "OnShow");
        this.Connect("popup_hide", this, "OnHide");

        this.FramesView = this.GetNode<CenterContainer>("Center/Contents/HBox/FramesView");
        this.PaintBtn = this.GetNode<PaintBtn>("Center/Contents/HBox/ToolBar/PaintBtn");
        this.WEdit = this.GetNode<SpinBox>("Center/Contents/HBox/ToolBar/WBox/WEdit");
        this.HEdit = this.GetNode<SpinBox>("Center/Contents/HBox/ToolBar/HBox/HEdit");
        this._NavPrev = this.GetNode<Button>("Center/Contents/HBox/ToolBar/FrameNav/NavPrev");
        this._NavNext = this.GetNode<Button>("Center/Contents/HBox/ToolBar/FrameNav/NavNext");
        this._AddFrame = this.GetNode<Button>("Center/Contents/HBox/ToolBar/FrameNav/AddFrame");
        this._DelFrame = this.GetNode<Button>("Center/Contents/HBox/ToolBar/FrameNav/DelFrame");
        this._Accept = this.GetNode<Button>("Center/Contents/Accept");
        this._FrameIndex = this.GetNode<Label>("Center/Contents/HBox/ToolBar/FrameIndex");


        this.WEdit.Connect("value_changed", this, "OnSizeEdit");
        this.HEdit.Connect("value_changed", this, "OnSizeEdit");
        this._NavPrev.Connect("pressed", this, "OnNavPrev");
        this._NavNext.Connect("pressed", this, "OnNavNext");
        this._AddFrame.Connect("pressed", this, "OnAddFrame");
        this._DelFrame.Connect("pressed", this, "OnDelFrame");
        this._Accept.Connect("pressed", this, "OnConfirmedLayers");
    }

    private void _OpenLayer(GenLayer l)
    {
        this.Layer = l;
        this.PaintBtn.LoadLayer(this.Layer);

        var _w = this.Layer.Frames[0].GetWidth();
        var _h = this.Layer.Frames[0].GetHeight();

        this._PopulateView(this.Layer.Frames, this.Layer.Pixels);

        this.FramesView.RectMinSize = new Vector2((float)_w, (float)_h) * new Vector2(20f, 20f);

        this.WEdit.Value = _w;
        this.HEdit.Value = _h;
        
        this.PopupCenteredMinsize();
    }

    public void NewLayer()
    {
        this._Editing = false;
        this._OpenLayer(PDefaults.Layer);
    }

    public void EditLayer(GenLayer l)
    {
        this._Editing = true;
        this._OpenLayer(l);
    }

    private void _PopulateView(List<string[,]> frames, Dictionary<string, Pixel> pixels)
    {

        foreach(string[,] _pair in frames)
        {
            this.FramesView.AddChild(new FrameControl() {
                Frame = _pair,
                Pixels = pixels,
                Visible = false,
            });
        }

        this.FramesView.GetChild<FrameControl>(0).Visible = true;
        this._FrameIndex.Text = $"Frame 1/{this.Layer.Data.Frames.Count}";
    }

    public void OnShow()
    {
        this._IgnoreSizeSignal = false;
        this._CurrentFrame = 0;
    }

    public void OnHide()
    {
        this._IgnoreSizeSignal = true;
        
        foreach(Node n in this.FramesView.GetChildren())
            { n.QueueFree(); }
    }

    public void OnConfirmedLayers()
    {
        var _newFrames = new List<string[,]>();

        foreach(FrameControl _f in this.FramesView.GetChildren())
        {
            _newFrames.Add(_f.FinalizedFrame);
        }

        this.Layer.Frames = _newFrames;

        if(!this._Editing)
            { this.GetTree().CallGroup("gp_canvas_handler", "AddLayer", this.Layer); }

        this.Layer = null;
        this.Hide();
    }

    public void OnSizeEdit(float value)
    {
        if(!this._IgnoreSizeSignal)
        {
            var _w = this.WEdit.Value;
            var _h = this.HEdit.Value;

            this.FramesView.RectMinSize = new Vector2((float)_w, (float)_h) * new Vector2(20f, 20f);

            foreach(FrameControl _f in this.FramesView.GetChildren())
            {
                _f.SetSize((int)_w, (int)_h);
            }

            this.RectSize = this.FramesView.RectMinSize;
        }
    }

    public void OnNavPrev()
    {
        if(this.FrameCount > 1)
            { this.CurrentFrame = this.CurrentFrame <= 0 ? this.FrameCount - 1 : this.CurrentFrame - 1; }
    }

    public void OnNavNext()
    {
        if(this.FrameCount > 1)
            { this.CurrentFrame = this.CurrentFrame >= this.FrameCount - 1 ? 0 : this.CurrentFrame + 1; }
    }

    public void OnAddFrame()
    {
        var _sample = this.FramesView.GetChild<FrameControl>(CurrentFrame).FinalizedFrame;

        this.FramesView.AddChild(new FrameControl() {
            Frame = _sample,
            Pixels = this.Layer.Data.Pixels,
        });

        this.CurrentFrame = this.FrameCount - 1;
    }

    public void OnDelFrame()
    {
        if(this.FramesView.GetChildCount() > 1)
        {
            this.FramesView.RemoveChild(this.FramesView.GetChild(this.CurrentFrame));
            if(this.CurrentFrame == 0)
            {
                this.CurrentFrame = 0;
            }
            else if(this.CurrentFrame >= this.FrameCount)
            {
                this.CurrentFrame = this.FrameCount - 1;
            }
            else
            {
                this.CurrentFrame = this.CurrentFrame;
            }
        }
    }
}