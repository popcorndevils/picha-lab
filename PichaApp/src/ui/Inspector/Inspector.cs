using Godot;

public class Inspector : TabContainer
{
    private LayerInspect _LayerProps;
    private CanvasInspect _CanvasProps;

    public override void _Ready()
    {
        this.RectMinSize = new Vector2(260, 200);

        this.TabAlign = TabAlignEnum.Left;
        this.DragToRearrangeEnabled = true;

        this._CanvasProps = new CanvasInspect() {
            Name = "Canvas",
            RectMinSize = new Vector2(260, 200),
        };

        this._LayerProps = new LayerInspect() {
            Name = "Layer",
            RectMinSize = new Vector2(260, 200),
        };

        this.AddChild(this._CanvasProps);
        this.AddChild(this._LayerProps);
    }
}