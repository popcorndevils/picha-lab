using Godot;

public class CanvasInspect : VBoxContainer
{
    public override void _Ready()
    {
        this.AddChild(new AnimProps());
    }
}
