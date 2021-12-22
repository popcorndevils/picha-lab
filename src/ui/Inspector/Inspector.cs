using Godot;

using PichaLib;

public class Inspector : TabContainer
{
    private LayerInspector _LayerInspector;
    private CanvasInspector _CanvasInspector;
    private PixelsInspector _PixelsInspector;

    public override void _Ready()
    {
        this.AddToGroup("inspector");

        this.RectMinSize = new Vector2(260, 200);
        this.TabAlign = TabAlignEnum.Left;
        this.DragToRearrangeEnabled = true;

        this._CanvasInspector = new CanvasInspector() {
            Name = "Canvas",
            RectMinSize = new Vector2(260, 200),
        };

        this._PixelsInspector = new PixelsInspector() {
            Name = "Pixels",
            RectMinSize = new Vector2(260, 200),
        };

        this._LayerInspector = new LayerInspector() {
            Name = "Layer",
            RectMinSize = new Vector2(260, 200),
        };

        this._PixelsInspector.PixelDeleted += this.OnPixelDelete;
        this._PixelsInspector.NewPixelAdded += this.OnNewPixelAdded;
        this._PixelsInspector.PixelNameChange += this.OnPixelNameChange;

        this.AddChildren(this._CanvasInspector, this._PixelsInspector, this._LayerInspector);
    }

    public void OnPixelDelete(Pixel p)
    {
        // this.GetTree().CallGroup("gp_layer_gui", "LoadLayer", this._Canvas);
    }

    public void OnNewPixelAdded(Pixel p)
    {
        this._LayerInspector.OnNewPixelAdded(p);
    }

    public void OnPixelNameChange(string oldName, string newName)
    {
        this._LayerInspector.ChangePixelName(oldName, newName);
    }

    public void LoadCanvas()
    {
        this._CanvasInspector.LoadCanvas();
        this._PixelsInspector.LoadCanvas();
        this._LayerInspector.LoadLayer();
    }

    public void LoadCanvas(GenCanvas canvas)
    {
        this._CanvasInspector.LoadCanvas(canvas);
        this._PixelsInspector.LoadCanvas(canvas);
        
        if(canvas.Layers.Count > 0)
        {
            this._LayerInspector.LoadLayer(canvas.Layers[0]);
        }
        else
        {
            this._LayerInspector.LoadLayer();
        }
    }

    public void AddLayer(GenLayer layer)
    {
        this._LayerInspector.LoadLayer(layer);
    }

    public void LoadLayer()
    {
        this._LayerInspector.LoadLayer();
    }

    public void LoadLayer(GenLayer layer)
    {
        this.CurrentTab = this._LayerInspector.GetIndex();
        this._LayerInspector.LoadLayer(layer);
    }
}