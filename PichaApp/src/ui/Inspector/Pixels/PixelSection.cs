using Godot;

using PichaLib;

public delegate void PixelNameChangeHandler(string oldName, string newName);
public delegate void NewPixelAddedHandler(Pixel p);

public class PixelSection : BaseSection
{
    public event PixelNameChangeHandler PixelNameChange;
    public event NewPixelAddedHandler NewPixelAdded;

    private GenLayer _Layer;
    private Button _NewPixel;

    public override void _Ready()
    {
        base._Ready();

        this.SectionHeader.Disabled = true;

        this._NewPixel = new Button() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Icon = GD.Load<Texture>("res://res/icons/add_white.svg"),
            FocusMode = FocusModeEnum.None,
            HintTooltip = PDefaults.ToolHints.Pixel.NewPixel,
            Disabled = true,
        };

        this.HeaderContainer.AddChild(this._NewPixel);

        this.SectionTitle = "Pixel Types";

        this._NewPixel.Connect("pressed", this, "OnNewPixel");
    }

    public void LoadLayer(GenLayer l)
    {
        this._Layer = l;
        this._NewPixel.Disabled = false;
        this.SectionHeader.Disabled = false;

        foreach(Node n in this.SectionGrid.GetChildren())
        { this.SectionGrid.RemoveChild(n); }

        foreach(Pixel p in l.Pixels.Values)
        { 
            this.AddNewPixel(p);
        }
    }

    public void AddNewPixel(Pixel p)
    {
        var _props = new PixelProperties() {
            SectionTitle = p.Name
        };

        _props.PixelDeleted += this.OnDeletePixel;
        _props.PixelChanged += this.OnChangePixel;

        this.SectionGrid.AddChild(_props);
        _props.SectionHeader.Align = Button.TextAlign.Left;
        _props.PixelLoad(p);
    }

    public void OnNewPixel()
    {
        var _newPixel = this._Layer.NewPixel();
        this.AddNewPixel(_newPixel);
        this.NewPixelAdded?.Invoke(_newPixel);
    }

    public void OnChangePixel(PixelProperties p, string property, object value)
    {
        switch(property)
        {
            case "Name":
                var _oldName = p.Pixel.Name;
                var _newName = this._Layer.ChangePixelName(p.Pixel, (string)value);
                if(_newName != (string)value)
                {
                    p.NameEdit.Text = _newName;
                }

                this.PixelNameChange?.Invoke(_oldName, _newName);
                break;
        }
    }

    public void OnDeletePixel(Pixel p)
    {
        this._Layer.DeletePixel(p);
        this.GetTree().CallGroup("gp_layer_gui", "LoadLayer", this._Layer);
    }
}