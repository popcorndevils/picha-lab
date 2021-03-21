

using Godot;

using PichaLib;

public class PatternDesigner : AcceptDialog
{
    private GenLayer Layer;
    private bool _Editing = false;
    private TabContainer FramesView;
    public PaintBtn PaintBtn;

    public override void _Ready()
    {
        this.AddToGroup("pattern_designer");
        this.Connect("confirmed", this, "OnConfirmedLayers");

        this.FramesView = this.GetNode<TabContainer>("Contents/HBox/FramesView");
        this.PaintBtn = this.GetNode<PaintBtn>("Contents/HBox/ToolBar/PaintBtn");
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
        this.FramesView.ClearChildren();

        this._Editing = false;
        this.Layer = PDefaults.Layer;

        this.FramesView.AddChild(new FrameControl() {
            Frame = this.Layer.Data.Frames[0],
            Pixels = this.Layer.Data.Pixels,
        });

        this.PaintBtn.LoadLayer(this.Layer);
        this.PopupCentered();
    }

    public void OpenLayer(GenLayer c)
    {
        this._Editing = true;
        this.Layer = c;
        this.PopupCentered();
    }
}
