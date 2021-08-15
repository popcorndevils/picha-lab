using Godot;

using PichaLib;

public delegate void CycleDeleteHandler(CycleProperties c);
public delegate void CycleChangeHandler(Cycle c);

public class CycleProperties : BaseSection
{
    public event CycleDeleteHandler CycleDeleted;
    public event CycleChangeHandler CycleChanged;

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
        this.SectionHeader.Draggable = true;
        this.SectionHeader.HeaderDropped += this.OnHeaderDrop;
        this.Theme = GD.Load<Theme>("./res/theme/SubSection.tres");
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
            _props.PolicyDeleted += this.OnPolicyDeleted;
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
        var _policy = this.Layer.NewPolicy(this.Cycle);

        var _props = new PolicyProperties() { 
            SectionTitle = $"{this.Layer.Pixels[_policy.Input].Name} -> {this.Layer.Pixels[_policy.Output].Name}",
        };
        this.SectionGrid.AddChild(_props);
        _props.SectionHeader.Align = Button.TextAlign.Left;
        _props.LoadPolicy(this.Layer, _policy);
        _props.PolicyDeleted += this.OnPolicyDeleted;
    }

    public void OnCycleDelete()
    {
        this.CycleDeleted?.Invoke(this);
    }

    public void OnPolicyDeleted(PolicyProperties p)
    {
        this.Layer.DeletePolicy(this.Cycle, p.Policy);
        p.QueueFree();
    }

    public void OnHeaderDrop(SectionHeader header, SectionHeader dropped)
    {
        Node _headerControl = header.GetParent().GetParent().GetParent();
        Node _droppedControl = dropped.GetParent().GetParent().GetParent();
        Node _list = _headerControl.GetParent();

        if(_droppedControl is CycleProperties _d)
        {
            var _h = (CycleProperties)_headerControl;
            _d.Layer.MoveCycle(_d.Cycle, _h.GetIndex());
        }

        _list.MoveChild(_droppedControl, _headerControl.GetIndex());
    }
}