using Godot;

public class WorkArea : MarginContainer
{
    [Export] public int Margins = 5;

    public CanvasView CanvasView = new CanvasView();

    private PanelContainer _WorkBG = new PanelContainer();
    private HSplitContainer _WorkSpace = new HSplitContainer();
    private MarginContainer _PropertyView = new MarginContainer();
    private VSplitContainer _PropertyPanel = new VSplitContainer();
    private PanelContainer _PropertyBackGround = new PanelContainer();

    public override void _Ready()
    {
        this._PropertyBackGround.AddChild(new LayersList());

        this._PropertyPanel.AddChild(new Inspector());
        this._PropertyPanel.AddChild(this._PropertyBackGround);
        
        this._PropertyView.AddChild(this._PropertyPanel);
        this._WorkSpace.AddChildren(this.CanvasView, this._PropertyView);

        this._PropertyPanel.RectMinSize = new Vector2(260, 0);

        this._WorkBG.AddChild(this._WorkSpace);

        this.AddChild(this._WorkBG);

        this.AddConstantOverride("margin_left", Margins);
        this.AddConstantOverride("margin_right", Margins);
        this.AddConstantOverride("margin_bottom", Margins);
    }
}