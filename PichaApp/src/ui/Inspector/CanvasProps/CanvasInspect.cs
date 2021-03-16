using Godot;

public class CanvasInspect : VBoxContainer
{
    public override void _Ready()
    {
        this.AddToGroup("canvas_gui_props");
        this.AddChild(new AnimProps());
    }

    public void CanvasLoad(GenSprite g)
    {
        
    }
}
