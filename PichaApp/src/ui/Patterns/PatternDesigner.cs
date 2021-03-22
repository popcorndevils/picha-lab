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

    public void EditLayer(GenLayer c)
    {
        this._Editing = true;
        this.Layer = c;
        this.PopupCentered();
    }

    public void NewLayer()
    {
        this._Editing = false;
        
        this._CurrentFrame = 0;
        this._FrameIndex.Text = "Frame 1/1";

        this.Layer = PDefaults.Layer;
        this.PaintBtn.LoadLayer(this.Layer);

        var _w = this.Layer.Data.Frames[0].GetWidth();
        var _h = this.Layer.Data.Frames[0].GetHeight();

        this._PopulateView(this.Layer.Data.Frames, this.Layer.Data.Pixels);
        this.FramesView.RectMinSize = new Vector2((float)_w, (float)_h) * new Vector2(20f, 20f);

        this.WEdit.Value = _w;
        this.HEdit.Value = _h;

        this.PopupCenteredMinsize();
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
        if(this._Editing)
        {
            GD.Print("EDIT DONE");
        }
        else
        {
            var _newLayers = new SortedList<int, string[,]>();

            foreach(FrameControl _f in this.FramesView.GetChildren())
            {
                _newLayers.Add(_newLayers.Count, _f.FinalizedFrame);
            }

            this.Layer.Data.Frames = _newLayers;

            this.GetTree().CallGroup("gp_canvas_handler", "AddLayer", this.Layer);
            this.GetTree().CallGroup("layers_list", "AddNewLayer", this.Layer);
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
        GD.Print("PREV");
        if(this.FrameCount > 1)
        {
            this.CurrentFrame = this.CurrentFrame <= 0 ? this.FrameCount - 1 : this.CurrentFrame - 1;
        }
    }

    public void OnNavNext()
    {
        GD.Print("NEXT");
        if(this.FrameCount > 1)
        {
            this.CurrentFrame = this.CurrentFrame >= this.FrameCount - 1 ? 0 : this.CurrentFrame + 1;
        }
    }

    public void OnAddFrame()
    {
        var _sample = this.FramesView.GetChild<FrameControl>(this.FramesView.CurrentTab).FinalizedFrame;

        this.FramesView.AddChild(new FrameControl() {
            Frame = _sample,
            Pixels = this.Layer.Data.Pixels,
        });

        this.CurrentFrame = this.FrameCount - 1;
    }
}
