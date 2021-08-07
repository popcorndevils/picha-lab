using Godot;

using PichaLib;

public class CycleProperties : BaseSection
{
    public GenLayer Layer;
    public Cycle Cycle;

    private Button _Delete = new Button() {
        SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        Icon = GD.Load<Texture>("res://res/icons/delete_white.svg"),
        FocusMode = FocusModeEnum.None,
        HintTooltip = PDefaults.ToolHints.Cycle.DeleteCycle,
    };

    private Button _NewPolicy = new Button() {
        SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        Icon = GD.Load<Texture>("res://res/icons/add_white.svg"),
        FocusMode = FocusModeEnum.None,
        HintTooltip = PDefaults.ToolHints.Cycle.AddPolicy,
    };

    public override void _Ready()
    {
        base._Ready();
        this.Theme = GD.Load<Theme>("./res/theme/SectionAlt.tres");
        this._Delete.Connect("pressed", this, "OnCycleDelete");
        this._NewPolicy.Connect("pressed", this, "OnAddPolicy");
        this.HeaderContainer.AddChildren(this._NewPolicy, this._Delete);
    }

    public void LoadCycle(GenLayer l, Cycle c)
    {
        this.Layer = l;
        this.Cycle = c;

        foreach(Policy _p in c.Policies)
        {
            var _props = new PolicyProperties() { 
                SectionTitle = $"{l.Pixels[_p.Input].Name} -> {l.Pixels[_p.Output].Name}",
            };
            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.LoadPolicy(l, _p);
        }
    }

    public void RenamePixel(string oldName, string newName)
    {
        foreach(Node n in this.SectionGrid.GetChildren())
        {
            if(n is PolicyProperties p)
            {
                p.RenamePixel(oldName, newName);
            }
        }
    }

    public void AddNewPixel(Pixel p)
    {
        foreach(Node n in this.SectionGrid.GetChildren())
        {
            if(n is PolicyProperties pol)
            {
                pol.AddNewPixel(p);
            }
        }
    }

    public void OnAddPolicy()
    {
        GD.Print("NEW POLICY");
        // var _props = new PolicyProps() { 
        //     SectionTitle = $"NONE -> NONE",
        // };

        // var _policy = PDefaults.Policy;

        // this.SectionGrid.AddChild(_props);
        // _props.SectionHeader.Align = Button.TextAlign.Left;
        // _props.LoadPolicy(this.Layer, _policy);
        // this.Cycle.Policies.Add(_policy);
    }

    public void OnCycleDelete()
    {
        GD.Print($"DELETING CYCLE {this.SectionTitle}");
    }
}