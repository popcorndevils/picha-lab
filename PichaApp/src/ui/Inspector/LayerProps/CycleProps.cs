using Godot;

using PichaLib;

public class CycleProps : BaseSection
{
    public Layer Layer;
    public Cycle Cycle;

    private Button _Delete;
    private Button _AddPolicy;

    public override void _Ready()
    {
        base._Ready();

        this._Delete = new Button() {
            Icon = GD.Load<Texture>("res://res/icons/clear-white.svg"),
            SizeFlagsHorizontal = 0,
        };

        this._AddPolicy = new Button() {
            Text = "+",
            SizeFlagsHorizontal = 0,
        };

        this.HeaderContainer.AddChild(this._AddPolicy);
        this.HeaderContainer.AddChild(this._Delete);

        this._AddPolicy.Connect("pressed", this, "OnAddPolicy");
        this._Delete.Connect("pressed", this, "OnCycleDelete");
    }

    public void LoadCycle(Layer l, Cycle c)
    {
        this.Layer = l;
        this.Cycle = c;

        foreach(Policy _p in c.Policies)
        {
            var _props = new PolicyProps() { 
                SectionTitle = $"{l.Pixels[_p.Input].Name} -> {l.Pixels[_p.Output].Name}",
            };
            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.LoadPolicy(l, _p);
        }
    }

    public void OnAddPolicy()
    {
        var _props = new PolicyProps() { 
            SectionTitle = $"NONE -> NONE",
        };

        var _policy = PDefaults.Policy;

        this.SectionGrid.AddChild(_props);
        _props.SectionHeader.Align = Button.TextAlign.Left;
        _props.LoadPolicy(this.Layer, _policy);
        this.Cycle.Policies.Add(_policy);
    }

    public void OnCycleDelete()
    {
        GD.Print($"DELETING {this.Name}");
        this.GetParent().RemoveChild(this);
    }
}