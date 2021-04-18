using Godot;

public class WorkArea : MarginContainer
{
    private HSplitContainer _WorkSpace = new HSplitContainer();
    private MarginContainer _PropertyView = new MarginContainer();
    private VSplitContainer _PropertyPanel = new VSplitContainer();
    private PanelContainer _PropertyBackGround = new PanelContainer();

    public CanvasView CanvasView = new CanvasView();

    public override void _Ready()
    {
        this._PropertyBackGround.AddChild(new LayersList());

        this._PropertyPanel.AddChild(new Inspector());
        this._PropertyPanel.AddChild(this._PropertyBackGround);
        
        this._PropertyView.AddChild(this._PropertyPanel);
        this._WorkSpace.AddChildren(this.CanvasView, this._PropertyView);

        this._PropertyPanel.RectMinSize = new Vector2(260, 0);

        this.AddChild(this._WorkSpace);

        this.AddConstantOverride("margin_left", 10);
        this.AddConstantOverride("margin_right", 10);
        this.AddConstantOverride("margin_bottom", 10);
    }
}