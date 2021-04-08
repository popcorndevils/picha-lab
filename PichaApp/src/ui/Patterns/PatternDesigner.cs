using System.Collections.Generic;

using Godot;

using PichaLib;
using OctavianLib;

public class PatternDesigner : WindowDialog
{
    private GenLayer Layer;
    private bool _Editing = false;
    private bool _IgnoreSizeSignal = true;
    private TabContainer FramesView;
    public PaintBtn PaintBtn;
    public SpinBox WEdit;
    public SpinBox HEdit;

    private Label _FrameIndex;
    private Button _NavPrev;
    private Button _NavNext;
    private Button _AddFrame;

    public int FrameCount => this.FramesView.GetChildren().Count;

    private int _CurrentFrame = 0;
    public int CurrentFrame {
        get => this._CurrentFrame;
        set {
            var _prevFrame = this.FramesView.GetChild<FrameControl>(this._CurrentFrame);
            var _frame = this.FramesView.GetChild<FrameControl>(value);
            _prevFrame.Visible = false;
            _frame.Visible = true;
            this._CurrentFrame = value;
            this._FrameIndex.Text = $"Frame {value + 1}/{this.FrameCount}";
        }
    }

    public override void _Ready()
    {
        this.AddToGroup("pattern_designer");

        this.Connect("confirmed", this, "OnConfirmedLayers");
        this.Connect("about_to_show", this, "OnShow");
        this.Connect("popup_hide", this, "OnHide");

        this.FramesView = this.GetNode<TabContainer>("Contents/HBox/FramesView");
        this.PaintBtn = this.GetNode<PaintBtn>("Contents/HBox/ToolBar/PaintBtn");
        this.WEdit = this.GetNode<SpinBox>("Contents/HBox/ToolBar/WBox/WEdit");
        this.HEdit = this.GetNode<SpinBox>("Contents/HBox/ToolBar/HBox/HEdit");
        this._NavPrev = this.GetNode<Button>("Contents/HBox/ToolBar/FrameNav/NavPrev");
        this._NavNext = this.GetNode<Button>("Contents/HBox/ToolBar/FrameNav/NavNext");
        this._AddFrame = this.GetNode<Button>("Contents/HBox/ToolBar/FrameNav/AddFrame");
        this._FrameIndex = this.GetNode<Label>("Contents/HBox/ToolBar/FrameIndex");


        this.WEdit.Connect("value_changed", this, "OnSizeEdit");
        this.HEdit.Connect("value_changed", this, "OnSizeEdit");
        this._NavPrev.Connect("pressed", this, "OnNavPrev");
        this._NavNext.Connect("pressed", this, "OnNavNext");
        this._AddFrame.Connect("pressed", this, "OnAddFrame");
    }

    private void _OpenLayer(GenLayer l)
    {
        this.Layer = l;
        this._CurrentFrame = 0;
        this._FrameIndex.Text = "Frame 1/1";
        this.PaintBtn.LoadLayer(this.Layer);

        var _w = this.Layer.Data.Frames[0].GetWidth();
        var _h = this.Layer.Data.Frames[0].GetHeight();

        this._PopulateView(this.Layer.Data.Frames, this.Layer.Data.Pixels);

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

    private void _PopulateView(SortedList<int, string[,]> frames, Dictionary<string, Pixel> Pixels)
    {
        foreach(Node n in this.FramesView.GetChildren())
            { n.QueueFree(); }

        this.FramesView.AddChild(new FrameControl() {
            Frame = this.Layer.Data.Frames[0],
            Pixels = this.Layer.Data.Pixels,
        });
    }

    public void OnShow()
    {
        this._IgnoreSizeSignal = false;
    }

    public void OnHide()
    {
        this._IgnoreSizeSignal = true;
    }

    public void OnConfirmedLayers()
    {
        var _output = this.Layer;

        if(this._Editing)
        {
            GD.Print("EDIT DONE");
        }
        else
        {
            var _newFrames = new SortedList<int, string[,]>();

            foreach(FrameControl _f in this.FramesView.GetChildren())
            {
                _newFrames.Add(_newFrames.Count, _f.FinalizedFrame);
            }

            _output.Data.Frames = _newFrames;

            this.Layer = null;

            this.GetTree().CallGroup("gp_canvas_handler", "AddLayer", _output);
        }
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
        {
            this.CurrentFrame = this.CurrentFrame <= 0 ? this.FrameCount - 1 : this.CurrentFrame - 1;
        }
    }

    public void OnNavNext()
    {
        if(this.FrameCount > 1)
        {
            this.CurrentFrame = this.CurrentFrame >= this.FrameCount - 1 ? 0 : this.CurrentFrame + 1;
        }
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
}
