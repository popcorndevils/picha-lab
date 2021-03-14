using Godot;

public class WorkArea : MarginContainer
{
    public override void _Ready()
    {
        this.GetNode<MarginContainer>("WorkSpace/Right").AddChild(new Inspector());
    }
}