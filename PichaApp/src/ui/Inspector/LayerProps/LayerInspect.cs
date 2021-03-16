using Godot;

using PichaLib;

public class LayerInspect : VBoxContainer
{
    private PixelSection _Pixels;
    private RuleProps _Rules;
    private GenProps _Gen;

    public override void _Ready()
    {
        this.AddToGroup("layer_gui_props");

        this._Pixels = new PixelSection();
        this._Rules = new RuleProps();
        this._Gen = new GenProps();

        this.AddChild(this._Pixels);
        this.AddChild(this._Rules);
        this.AddChild(this._Gen);
    }

    public void LayerLoad(GenLayer l)
    {
        this._Pixels.LayerLoad(l);
    }
}
