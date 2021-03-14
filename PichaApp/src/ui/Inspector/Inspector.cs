using Godot;

public class Inspector : TabContainer
{
    private LayerInspect _LayerProps;
    private CanvasInspect _CanvasProps;

    public override void _Ready()
    {
        this.TabAlign = TabAlignEnum.Left;
        this.DragToRearrangeEnabled = true;

        this._CanvasProps = new CanvasInspect() {
            Name = "Canvas"
        };

        this._LayerProps = new LayerInspect() {
            Name = "Layer"
        };

        this.AddChild(this._CanvasProps);
        this.AddChild(this._LayerProps);
    }
}