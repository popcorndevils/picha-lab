using Godot;

public class CanvasContainer : Control
{
    private bool _Dragging;
    private RulerGrid Rulers = new RulerGrid();

    public Vector2 Centered => new Vector2(this.RectSize.x / 2, this.RectSize.y / 2);

    private GenCanvas _Canvas;
    public GenCanvas Canvas {
        get => this._Canvas;
        set {
            if(this._Canvas != null)
            { 
                var _old = this._Canvas;
                this.RemoveChild(this._Canvas);
                _old.DeleteSelf();
            }
            this._Canvas = value;
            this.AddChild(value);
            this.MoveChild(value, 0);
            value.Scale = new Vector2(20, 20);
            value.Position = (this.RectSize / 2) - ((value.Size / 2) * value.Scale);
        }
    }

    public override void _Ready()
    {
        this.RectClipContent = true;
        this.AddChild(this.Rulers);

        this.Connect("visibility_changed", this, "OnVisibleChanged");
        this.Rulers.Center.Connect("pressed", this, "OnCenterCanvas");
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
                this.Canvas.Scale *= new Vector2(.95f, .95f);
            }
            else if(btn.ButtonIndex == (int)ButtonList.WheelUp)
            {
                this.Canvas.Scale *= new Vector2(1.05f, 1.05f);
            }
        }

        if(@event is InputEventMouseMotion mtn && this._Dragging && this.Visible)
        {
            this.Canvas.Position += mtn.Relative;
        }
    }

    public void OnVisibleChanged()
    {
        if(this.Visible) 
        { 
            this.GetTree().CallGroup("inspector", "LoadCanvas", this.Canvas);
            this.GetTree().CallGroup("layerstack", "LoadCanvas", this.Canvas);
        }
    }

    public void OnCenterCanvas()
    {
        this._Canvas.Position = (this.RectSize / 2) - ((this._Canvas.Size / 2) * this._Canvas.Scale);
    }
}
