using Godot;

public class LayerStack : TabContainer
{
    private VBoxContainer _LayersViewList = new VBoxContainer() {
        Name = "Layer Stack",
        RectMinSize = new Vector2(260, 200)
    };

    private ScrollContainer _Contents = new ScrollContainer() {
        SizeFlagsVertical = (int)SizeFlags.ExpandFill,
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
    };

    private HBoxContainer _FooterContainer = new HBoxContainer() {
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
    };

    private VBoxContainer _Buttons = new VBoxContainer() {
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)SizeFlags.ExpandFill,
    };

    private Button _NewLayer = new Button() {
        SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        Icon = GD.Load<Texture>("res://res/icons/queue_white.svg"),
        FocusMode = FocusModeEnum.None,
        HintTooltip = PDefaults.ToolHints.Layer.NewLayer,
    };

    public GenCanvas Canvas;

    public override void _Ready()
    {
        this.TabAlign = TabAlignEnum.Left;
        this.DragToRearrangeEnabled = false;

        this.AddToGroup("gp_canvas_gui");
        this.AddToGroup("layers_list");

        this.AddChild(this._LayersViewList);

        this._FooterContainer.AddChildren(this._NewLayer);
        this._Contents.AddChild(_Buttons);

        this._LayersViewList.AddChildren(
            this._Contents,
            this._FooterContainer
        );

        this._NewLayer.Connect("pressed", this, "OnNewLayerPressed");
    }

    public void LoadCanvas(GenCanvas c)
    {
        GD.Print("HERE");
        this.Canvas = c;

        foreach(Node n in this._Buttons.GetChildren())
        {
            if(n is LayerButtonControl b)
            {
                GD.Print(b);
                b.Layer = null;
                b.QueueFree();
                this._Buttons.RemoveChild(b);
            }
        }
        foreach(GenLayer _l in c.Layers)
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

    public void OnLayerHover(bool hover, GenLayer layer)
    {

        foreach(Node n in this._Buttons.GetChildren())
        {
            if(n is LayerButtonControl b)
            {
                if(hover & layer == b.Layer)
                {
                    b.Modulate = new Color(.75f, .75f, .75f, 1f);
                }
                else
                {
                    b.Modulate = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
    }
}