using Godot;

public class InspectorView : TabContainer
{
    private LayerInspector _LayerProps;
    private CanvasInspector _CanvasProps;

    public override void _Ready()
    {
        this.RectMinSize = new Vector2(260, 200);

        this.TabAlign = TabAlignEnum.Left;
        this.DragToRearrangeEnabled = true;

        this._CanvasProps = new CanvasInspector() {
            Name = "Canvas",
            RectMinSize = new Vector2(260, 200),
        };

        this._LayerProps = new LayerInspector() {
            Name = "Layer",
            RectMinSize = new Vector2(260, 200),
        };

        this.AddChild(this._CanvasProps);
        this.AddChild(this._LayerProps);
    }
}