using Godot;

public class LayersList : VBoxContainer
{
    private ScrollContainer _Contents = new ScrollContainer() {
        SizeFlagsVertical = (int)SizeFlags.ExpandFill,
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
    };

    private HBoxContainer _HeaderContainer = new HBoxContainer() {
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
    };

    private Button _NewLayer = new Button() {
        Text = "+",
        SizeFlagsHorizontal = (int)SizeFlags.ShrinkCenter,
    };

    public GenCanvas Canvas;

    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_gui");

        this._HeaderContainer.AddChildren(
            new Label() { Text = "Layers" },
            this._NewLayer );

        this.AddChildren(
            this._HeaderContainer,
            this._Contents );

        this._NewLayer.Connect("pressed", this, "OnNewLayerPressed");
    }

    public void LoadCanvas(GenCanvas c)
    {
        // TODO
        this.Canvas = c;
    }

    public void OnNewLayerPressed()
    {
        this.GetTree().CallGroup("pattern_designer", "NewLayer");
    }
}