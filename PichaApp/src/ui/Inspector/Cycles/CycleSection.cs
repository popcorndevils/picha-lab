using Godot;

using PichaLib;

public class CycleSection : BaseSection
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

    public void LoadLayer(GenLayer l)
    {
        this._Layer = l;
        this._NewCycle.Disabled = false;
        
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
        GD.Print("NEW CYCLE");
    }
}