using Godot;

public class LayerInspect : VBoxContainer
{
    public override void _Ready()
    {
        this.AddChild(new PixelProps());
        this.AddChild(new RuleProps());
        this.AddChild(new GenProps());
    }
}
