using Godot;

using PichaLib;

public class CycleSection : BaseSection
{
    private GenLayer _Layer;

    private Button _NewCycle = new Button() {
        SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        Icon = GD.Load<Texture>("res://res/icons/add_white.svg"),
        FocusMode = FocusModeEnum.None,
        HintTooltip = PDefaults.ToolHints.Cycle.NewCycle,
        Disabled = true,
    };

    public override void _Ready()
    {
        base._Ready();

        this.SectionHeader.Disabled = true;
        this.SectionTitle = "Cycles";
        this.HeaderContainer.AddChild(this._NewCycle);

        this._NewCycle.Connect("pressed", this, "OnNewCycle");
    }

    public void LoadLayer(GenLayer l)
    {
        this._Layer = l;
        this._NewCycle.Disabled = false;
        this.SectionHeader.Disabled = false;
        
        foreach(Node n in this.SectionGrid.GetChildren())
        {
            this.SectionGrid.RemoveChild(n);
        }

        foreach(Cycle _c in l.Cycles.Values)
        {
            var _props = new CycleProperties() { SectionTitle = _c.Name };
            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.LoadCycle(l, _c);
            _props.CycleDelete += this.OnCycleDelete;
        }
    }

    public void PixelNameChange(string oldName, string newName)
    {
        foreach(Node n in this.SectionGrid.GetChildren())
        {
            if(n is CycleProperties c)
            {
                c.RenamePixel(oldName, newName);
            }
        }
    }

    public void AddNewPixel(Pixel p)
    {
        foreach(Node n in this.SectionGrid.GetChildren())
        {
            if(n is CycleProperties c)
            {
                c.AddNewPixel(p);
            }
        }
    }

    public void OnNewCycle()
    {
        var _newCycle = this._Layer.NewCycle();

        var _props = new CycleProperties() { SectionTitle = _newCycle.Name };
        this.SectionGrid.AddChild(_props);
        _props.SectionHeader.Align = Button.TextAlign.Left;
        _props.LoadCycle(this._Layer, _newCycle);
        _props.CycleDelete += this.OnCycleDelete;
    }

    public void OnCycleDelete(CycleProperties c)
    {
        this._Layer.DeleteCycle(c.Cycle);
        c.QueueFree();
    }
}