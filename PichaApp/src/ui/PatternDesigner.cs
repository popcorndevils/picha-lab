

using Godot;

using PichaLib;

public class PatternDesigner : AcceptDialog
{
    private GenLayer Layer;
    private bool _Editing = false;

    public override void _Ready()
    {
        this.AddToGroup("pattern_designer");
        this.Connect("confirmed", this, "OnConfirmedLayers");
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

        this.PopupCentered();
    }

    public void OpenLayer(GenLayer c)
    {
        this._Editing = true;
        this.Layer = c;
        this.PopupCentered();
    }
}
