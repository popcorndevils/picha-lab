using System.Collections.Generic;
using Godot;

using PichaLib;

public class CycleProperties : BaseSection
{
    private GenLayer _Layer;

    private Button _NewCycle = new Button() {
        Text = "Add Cycle",
        RectMinSize = new Vector2(0, 30),
        Disabled = true,
        FocusMode = FocusModeEnum.None,
    };

    public override void _Ready()
    {
        this.SectionTitle = "Cycles";
        this.SectionContent.AddChild(this._NewCycle);

        this._NewCycle.Connect("pressed", this, "OnNewCycle");

        base._Ready();
    }

    public void LoadLayer(Layer l)
    {
        this._NewCycle.Disabled = false;
        
        foreach(Node n in this.SectionGrid.GetChildren())
        {
            this.SectionGrid.RemoveChild(n);
        }

        foreach(Cycle _c in l.Cycles.Values)
        {
            var _props = new CycleProps() { SectionTitle = _c.Name };
            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.LoadCycle(l, _c);
        }
    }

    public void OnNewCycle()
    {
        GD.Print("NEW CYCLE");
    }
}