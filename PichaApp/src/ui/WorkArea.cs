using Godot;

public class WorkArea : MarginContainer
{
    public override void _Ready()
    {
        this.GetNode<MarginContainer>("WorkSpace/PropView").AddChild(new Inspector());
    }
}