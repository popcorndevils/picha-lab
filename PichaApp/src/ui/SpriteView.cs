using Godot;

public class SpriteView : Control
{
    public GenCanvas Sprite;
    private bool _Dragging;

    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_handler");
        this.RectClipContent = true;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton btn)
        {
            if(btn.ButtonIndex == (int)ButtonList.Right)
            {
                if(btn.Pressed)
                {
                    this._Dragging = true;
                }
                else
                {
                    this._Dragging = false;
                }
            }
            else if(btn.ButtonIndex == (int)ButtonList.WheelDown)
            {

            }
            else if(btn.ButtonIndex == (int)ButtonList.WheelUp)
            {
                
            }
        }

        if(@event is InputEventMouseMotion mtn && this._Dragging)
        {
            this.Sprite.Position += mtn.Relative;
        }
    }

    public void LoadCanvas(GenCanvas c)
    {
        this.Sprite = c;

        this.AddChild(c);
        c.Scale = new Vector2(20, 20);
        c.Position = (this.RectSize / 2) - ((c.Size / 2) * c.Scale);

        this.GetTree().CallGroup("gp_canvas_gui", "LoadCanvas", c);
    }
}
