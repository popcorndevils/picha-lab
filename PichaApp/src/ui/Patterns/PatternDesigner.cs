using System.Collections.Generic;

using Godot;

using PichaLib;
using OctavianLib;

public class PatternDesigner : WindowDialog
{
    private GenLayer Layer;
    private bool _Editing = false;
    private TabContainer FramesView;
    public PaintBtn PaintBtn;
    public SpinBox WEdit;
    public SpinBox HEdit;

    public override void _Ready()
    {
        this.AddToGroup("pattern_designer");

        this.Connect("confirmed", this, "OnConfirmedLayers");

        this.FramesView = this.GetNode<TabContainer>("Contents/HBox/FramesView");
        this.PaintBtn = this.GetNode<PaintBtn>("Contents/HBox/ToolBar/PaintBtn");
        this.WEdit = this.GetNode<SpinBox>("Contents/HBox/ToolBar/WBox/WEdit");
        this.HEdit = this.GetNode<SpinBox>("Contents/HBox/ToolBar/HBox/HEdit");
        this.WEdit.Connect("value_changed", this, "OnSizeEdit");
        this.HEdit.Connect("value_changed", this, "OnSizeEdit");
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

    public void EditLayer(GenLayer c)
    {
        this._Editing = true;
        this.Layer = c;
        this.PopupCentered();
    }

    public void NewLayer()
    {
        this._Editing = false;
        
        this.Layer = PDefaults.Layer;
        this.PaintBtn.LoadLayer(this.Layer);

        var _w = this.Layer.Data.Frames[0].GetWidth();
        var _h = this.Layer.Data.Frames[0].GetHeight();

        this._PopulateView(this.Layer.Data.Frames, this.Layer.Data.Pixels);
        this.FramesView.RectMinSize = new Vector2((float)_w, (float)_h) * new Vector2(20f, 20f);

        this.WEdit.Disconnect("value_changed", this, "OnSizeEdit");
        this.HEdit.Disconnect("value_changed", this, "OnSizeEdit");
        this.WEdit.Value = _w;
        this.HEdit.Value = _h;
        this.WEdit.Connect("value_changed", this, "OnSizeEdit");
        this.HEdit.Connect("value_changed", this, "OnSizeEdit");

        this.PopupCenteredMinsize();
    }

    private void _PopulateView(SortedList<int, string[,]> frames, Dictionary<string, Pixel> Pixels)
    {
        foreach(Node n in this.FramesView.GetChildren())
            { this.FramesView.RemoveChild(n); }

        this.FramesView.AddChild(new FrameControl() {
            Frame = this.Layer.Data.Frames[0],
            Pixels = this.Layer.Data.Pixels,
        });
    }

    public void OnSizeEdit(float value)
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
