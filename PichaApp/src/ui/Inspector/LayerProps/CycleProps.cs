using Godot;

using PichaLib;

public class CycleProps : BaseSection
{

    public override void _Ready()
    {
        base._Ready();
    }

    public void LoadCycle(Layer l, Cycle c)
    {
        foreach(Policy _p in c.Policies.Values)
        {
            var _props = new RuleProps() { 
                SectionTitle = $"{l.Pixels[_p.Input].Name} -> {l.Pixels[_p.Output].Name}",
            };
            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.LoadPolicy(l, _p);
        }
    }
}