

using Godot;

using PichaLib;

public class PatternDesigner : AcceptDialog
{
    private GenLayer Layer;
    private bool _Editing = false;
    private FrameControl _Pattern = new FrameControl();
    private VBoxContainer _Contents = new VBoxContainer() {
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)SizeFlags.ExpandFill,
        Alignment = BoxContainer.AlignMode.Center,
    };

    private TabContainer _FramesView = new TabContainer() {
        TabsVisible = false,
    };

    public override void _Ready()
    {
        this.AddToGroup("pattern_designer");
        this.Connect("confirmed", this, "OnConfirmedLayers");

        this._FramesView.AddChild(this._Pattern);
        this._Contents.AddChild(_FramesView);
        this.AddChild(this._Contents);

        this._FramesView.AddConstantOverride("top_margin", 0);
        this._FramesView.AddConstantOverride("side_margin", 0);
    }

    public void OnConfirmedLayers()
    {
        if(this._Editing)
        {
            GD.Print("EDIT DONE");
        }
        else
        {
            this.GetTree().CallGroup("gp_canvas_handler", "AddLayer", this.Layer);
            this.GetTree().CallGroup("layers_list", "AddNewLayer", this.Layer);
        }
    }

    public void NewLayer()
    {
        this._Editing = false;
        this.Layer = PDefaults.Layer;

        this._FramesView.RectMinSize = this._Pattern.Size;
        GD.Print(this._Pattern.Size);

        this.PopupCentered();
    }

    public void OpenLayer(GenLayer c)
    {
        this._Editing = true;
        this.Layer = c;
        this.PopupCentered();
    }
}
