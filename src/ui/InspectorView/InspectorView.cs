using Godot;

public class InspectorView : TabContainer
{
    private LayerInspector _LayerInspector;
    private CanvasInspector _CanvasInspector;

    public override void _Ready()
    {
        this.RectMinSize = new Vector2(260, 200);

        this.TabAlign = TabAlignEnum.Left;
        this.DragToRearrangeEnabled = true;

        this._CanvasInspector = new CanvasInspector() {
            Name = "Canvas",
            RectMinSize = new Vector2(260, 200),
        };

        this._LayerInspector = new LayerInspector() {
            Name = "Layer",
            RectMinSize = new Vector2(260, 200),
        };

        this.AddChildren(this._CanvasInspector, this._LayerInspector);

        
        this._CanvasInspector._Pixels.NewPixelAdded += this._LayerInspector.OnNewPixelAdded;
    }

    public void LayerLoaded()
    {
        this.CurrentTab = this._LayerInspector.GetIndex();
    }
}