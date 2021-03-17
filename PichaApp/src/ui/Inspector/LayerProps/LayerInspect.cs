using Godot;

using PichaLib;

public class LayerInspect : ScrollContainer
{
    private PixelSection _Pixels;
    private CycleSection _Cycles;
    private GenSection _Gen;
    private VBoxContainer _Contents;

    public override void _Ready()
    {
        this.AddToGroup("layer_gui_props");

        this._Contents = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Pixels = new PixelSection();
        this._Cycles = new CycleSection();
        this._Gen = new GenSection();

        this._Contents.AddChild(this._Pixels);
        this._Contents.AddChild(this._Cycles);
        this._Contents.AddChild(this._Gen);

        this.AddChild(this._Contents);
    }

    public void LoadLayer(GenLayer l)
    {
        this._Pixels.LoadLayer(l.Data);
        this._Cycles.LoadLayer(l.Data);
    }
}
