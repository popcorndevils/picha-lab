using Godot;

using PichaLib;

public class LayerInspect : ScrollContainer
{
    private PixelSection _Pixels;
    private RuleProps _Rules;
    private GenProps _Gen;
    private VBoxContainer _Contents;

    public override void _Ready()
    {
        this.AddToGroup("layer_gui_props");

        this._Contents = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Pixels = new PixelSection();
        this._Rules = new RuleProps();
        this._Gen = new GenProps();

        this._Contents.AddChild(this._Pixels);
        this._Contents.AddChild(this._Rules);
        this._Contents.AddChild(this._Gen);

        this.AddChild(this._Contents);
    }

    public void LayerLoad(GenLayer l)
    {
        this._Pixels.LayerLoad(l);
    }
}
