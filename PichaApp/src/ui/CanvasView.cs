using Godot;

public class CanvasView : TabContainer
{
    public GenCanvas Active {
        get => this.GetChild<CanvasContainer>(this.CurrentTab).Canvas;
    }
    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_handler");
    }

    public void AddCanvas(GenCanvas c)
    {
        var _i = this.GetChildren().Count;
        var _view = new CanvasContainer();
        this.AddChild(_view);
        c.Position = new Vector2(_view.RectSize.x / 2, _view.RectSize.y / 2);
        _view.Canvas = c;
        this.CurrentTab = _i;
        this.SetTabTitle(this.CurrentTab, c.Name);
    }

    public void NameCurrentTab(string s)
    {
        this.SetTabTitle(this.CurrentTab, s);
    }
}
