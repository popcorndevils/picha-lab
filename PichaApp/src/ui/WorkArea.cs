using Godot;

public class WorkArea : MarginContainer
{
    private MarginContainer _PropertyView;
    private VSplitContainer _PropertyPanel = new VSplitContainer();

    public override void _Ready()
    {
        this._PropertyView = this.GetNode<MarginContainer>("WorkSpace/PropView");

        this._PropertyPanel.AddChild(new Inspector());
        this._PropertyPanel.AddChild(new LayersList());
        
        this._PropertyView.AddChild(this._PropertyPanel);
    }
}