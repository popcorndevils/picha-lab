using Godot;

public class LayerButtonControl : HBoxContainer
{
    public LayerButton Button = new LayerButton();
    public Button Grid = new Button() {
        SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        Icon = GD.Load<Texture>("res://res/icons/grid-white.svg"),
        FocusMode = FocusModeEnum.None,
        HintTooltip = PDefaults.ToolHints.Layer.OpenTemplate,
    };

    private GenLayer _Layer;
    public GenLayer Layer {
        get => this.Button.Layer;
        set => this.Button.Layer = value;
    } 

    public override void _Ready()
    {
        this.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        this.AddChildren(this.Button, this.Grid);
        this.Grid.Connect("pressed", this, "OnGridOpen");
    }

    public void OnGridOpen()
    {
        this.GetTree().CallGroup("pattern_designer", "EditLayer", this.Layer);
    }
}