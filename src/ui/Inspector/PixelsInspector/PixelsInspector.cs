using Godot;
using PichaLib;

public delegate void NewPixelAddedHandler(Pixel p);
public delegate void PixelDeletedHandler(Pixel p);
public delegate void PixelNameChangedHandler(string oldName, string newName);

public class PixelsInspector : ScrollContainer
{
    public event NewPixelAddedHandler NewPixelAdded;
    public event PixelDeletedHandler PixelDeleted;
    public event PixelNameChangedHandler PixelNameChange;

    public string Title {
        get => this._Header.Text;
        set => this._Header.Text = value;
    }

    private GenCanvas _Canvas;
    private Button _NewPixel;

    private VBoxContainer _SectionContent = new VBoxContainer();
    private GridContainer _SectionGrid = new GridContainer();
    private HBoxContainer _HeaderContainer = new HBoxContainer();
    private Label _Header = new Label() { Text = "", Align = Label.AlignEnum.Center };

    private Godot.Collections.Array _Pixels => this._SectionGrid.GetChildren();

    public override void _Ready()
    {
        this._NewPixel = new Button() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Icon = GD.Load<Texture>("res://res/icons/add_white.svg"),
            FocusMode = FocusModeEnum.None,
            HintTooltip = PDefaults.ToolHints.Pixel.NewPixel,
            Disabled = true,
        };

        this._HeaderContainer.AddChildren(this._Header, this._NewPixel);
        this._SectionContent.AddChildren(this._HeaderContainer, this._SectionGrid);
        this.AddChild(this._SectionContent);

        this.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
        this.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this._Header.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this._SectionContent.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this._HeaderContainer.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this._SectionGrid.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;

        this._NewPixel.Connect("pressed", this, "OnNewPixel");
        base._Ready();
    }

    public void OnPixelChange(Pixel p, string property, object value)
    {
        switch(property)
        {
            case "Name":
                var _oldName = p.Name;
                var _newName = this._Canvas.ChangePixelName(p, (string)value);
                if(_newName != (string)value)
                {
                    this.CorrectPixelName((string)value, _newName);
                }
                this.PixelNameChange?.Invoke(_oldName, _newName);
                break;
            case "RandomCol":
                p.RandomCol = (bool)value;
                break;
            case "BrightNoise":
                p.BrightNoise = (float)value;
                break;
            case "MinSaturation":
                p.MinSaturation = (float)value;
                break;
            case "Color":
                p.Color = (Chroma)value;
                break;
            case "Paint":
                p.Paint = (Chroma)value;
                break;
            case "FadeDirection":
                p.FadeDirection = (FadeDirection)value;
                break;
        }
    }
    
    public void CorrectPixelName(string oldName, string newName)
    {
        foreach(Node n in this._Pixels)
        {
            if(n is PixelProps p)
            {
                if(p.NameEdit.Text != p.Pixel.Name)
                {
                    p.NameEdit.Text = newName;
                    p.NameEdit.CaretPosition = newName.Length;
                    p.SectionTitle = newName;
                }
            }
        }
    }

    public void LoadCanvas(GenCanvas c)
    {
        this._Canvas = c;
        this._NewPixel.Disabled = false;

        foreach(Node n in this._SectionGrid.GetChildren())
            { this._SectionGrid.RemoveChild(n); }

        foreach(Pixel p in c.Pixels.Values)
        { 
            this.AddNewPixel(p);
        }
    }

    public void LoadCanvas()
    {
        this._Canvas = null;
        this._NewPixel.Disabled = true;

        foreach(Node n in this._SectionGrid.GetChildren())
            { this._SectionGrid.RemoveChild(n); }
    }
        
    public void AddNewPixel(Pixel p)
    {
        var _props = new PixelProps() {
            SectionTitle = p.Name
        };

        _props.PixelDeleted += this.OnDeletePixel;
        _props.PixelChanged += this.OnPixelChange;

        this._SectionGrid.AddChild(_props);
        _props.SectionHeader.Align = Button.TextAlign.Left;
        _props.PixelLoad(p);
    }

    public void OnNewPixel()
    {
        var _newPixel = this._Canvas.NewPixel();
        this.AddNewPixel(_newPixel);

        this.NewPixelAdded?.Invoke(_newPixel);
    }

    public void OnDeletePixel(Pixel p)
    {
        this._Canvas.DeletePixel(p);
        this.PixelDeleted?.Invoke(p);
    }
}