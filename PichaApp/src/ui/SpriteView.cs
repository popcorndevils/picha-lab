using Godot;

public class SpriteView : TabContainer
{
    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_handler");
    }

    public void AddCanvas(GenCanvas c)
    {
        var _view = new SpriteContainer() { Name = c.Name };
        this.AddChild(_view);
        _view.Canvas = c;
        this.CurrentTab = this.GetChildren().Count - 1;
    }
}
