using Godot;

public class LayersView : TabContainer
{
    private VBoxContainer _LayersViewList = new VBoxContainer() {
        Name = "Layers",
        RectMinSize = new Vector2(260, 200)
    };

    private ScrollContainer _Contents = new ScrollContainer() {
        SizeFlagsVertical = (int)SizeFlags.ExpandFill,
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
    };

    private HBoxContainer _HeaderContainer = new HBoxContainer() {
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
    };

    private VBoxContainer _Buttons = new VBoxContainer() {
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)SizeFlags.ExpandFill,
    };

    private Button _NewLayer = new Button() {
        Text = "+",
        SizeFlagsHorizontal = (int)SizeFlags.ShrinkCenter,
        FocusMode = FocusModeEnum.None,
    };

    public GenCanvas Canvas;

    public override void _Ready()
    {
        this.TabAlign = TabAlignEnum.Left;
        this.DragToRearrangeEnabled = false;

        this.AddToGroup("gp_canvas_gui");
        this.AddToGroup("layers_list");

        this.AddChild(this._LayersViewList);

        this._HeaderContainer.AddChildren(this._NewLayer);
        this._Contents.AddChild(_Buttons);

        this._LayersViewList.AddChildren(
            this._HeaderContainer,
            this._Contents);

        this._NewLayer.Connect("pressed", this, "OnNewLayerPressed");
    }

    public void LoadCanvas(GenCanvas c)
    {
        this.Canvas = c;

        foreach(Node n in this._Buttons.GetChildren())
        {
            this._Buttons.RemoveChild(n);
        }
        foreach(GenLayer _l in c.Layers.Values)
        {
            this._Buttons.AddChild(new LayerButtonControl() { Layer = _l });
        }
    }

    public void AddNewLayer(GenLayer l)
    {
        this._Buttons.AddChild(new LayerButtonControl() { Layer = l });
    }

    public void OnNewLayerPressed()
    {
        this.GetTree().CallGroup("pattern_designer", "NewLayer");
    }
}