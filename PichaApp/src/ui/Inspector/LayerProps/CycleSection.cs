using System.Collections.Generic;
using Godot;

using PichaLib;

public class CycleSection : BaseSection
{
    private GenLayer _Layer;

    public override void _Ready()
    {
        this.SectionTitle = "Cycles";
        base._Ready();
    }

    public void LoadLayer(Layer l)
    {
        foreach(Cycle _c in l.Cycles.Values)
        {
            var _props = new CycleProps() { SectionTitle = _c.Name };
            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.LoadCycle(l, _c);
        }
    }
}