using Godot;

public class GenSprite : Node2D
{
    public override void _Ready()
    {
        this.AddChild(new GenLayer());
        this.AddChild(new GenLayer() {Position = new Vector2(16, 0)});
    }

    public void Generate()
    {
        foreach(Node n in this.GetChildren())
        {
            var _s = n as GenLayer;
            _s.Generate();
        }
    }
}