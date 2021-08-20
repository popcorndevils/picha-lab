using Godot;
using PichaLib;

public class LayerInspector : ScrollContainer
{
    // public GenLayer GenLayer;
    public GenLayer Layer;

    private VBoxContainer _Contents;
    private GridContainer _GenSettings;
    private Button _Delete;
    private PixelSection _Pixels;
    private CycleSection _Cycles;

    private LineEdit _NameEdit;
    private CheckBox _MirrorXEdit;
    private CheckBox _MirrorYEdit;

    public override void _Ready()
    {
        this.AddToGroup("gp_layer_gui");

        this.RectMinSize = new Vector2(350, 0);

        this._Delete = new Button() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Icon = GD.Load<Texture>("res://res/icons/delete_white.svg"),
            FocusMode = FocusModeEnum.None,
            HintTooltip = PDefaults.ToolHints.Layer.DeleteLayer,
        };
        
        this._Contents = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._GenSettings = new GridContainer() {
            Columns = 2,
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Pixels = new PixelSection();
        this._Cycles = new CycleSection();

        this._Pixels.PixelChanged += this.OnPixelChange;
        this._Pixels.NewPixelAdded += this.OnNewPixelAdded;

        this._NameEdit = new LineEdit() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            Editable = false,
        };
        
        this._MirrorXEdit = new CheckBox() {
            SizeFlagsHorizontal = 0,
            Text = "X",
            Disabled = true,
            FocusMode = FocusModeEnum.None,
        };

        this._MirrorYEdit = new CheckBox() {
            SizeFlagsHorizontal = 0,
            Text = "Y",
            Disabled = true,
            FocusMode = FocusModeEnum.None,
        };

        var _mirrorGroup = new HBoxContainer() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Fill
        };

        var _nameLabel = new Label() {
            Text = "Layer Name",
            Align = Label.AlignEnum.Right,
        };

        var _mirrorLabel = new Label() {
            Text = "Mirror Settings",
            Align = Label.AlignEnum.Right,
        };

        var _nameDeleteBox = new HBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Contents.AddChildren(this._GenSettings, this._Pixels, this._Cycles);
        this.AddChild(this._Contents);

        _mirrorGroup.AddChildren(this._MirrorXEdit, this._MirrorYEdit);
        _nameDeleteBox.AddChildren(this._NameEdit, this._Delete);

        this._GenSettings.AddChildren(_nameLabel, _nameDeleteBox, _mirrorLabel, _mirrorGroup);

        this._NameEdit.Connect("text_changed", this, "OnLayerNameChange");
        this._MirrorXEdit.Connect("pressed", this, "OnLayerMirrorXChange");
        this._MirrorYEdit.Connect("pressed", this, "OnLayerMirrorYChange");
    }

    public void OnPixelChange(Pixel p, string property, object value)
    {
        switch(property)
        {
            case "Name":
                var _oldName = p.Name;
                var _newName = this.Layer.ChangePixelName(p, (string)value);
                this._Cycles.PixelNameChange(_oldName, _newName);
                if(_newName != (string)value)
                {
                    this.CorrectPixelName((string)value, _newName);
                }
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

    public void AddLayer(GenLayer layer)
    {
        this.LoadLayer(layer);
    }

    public void OnNewPixelAdded(Pixel p)
    {
        this._Cycles.AddNewPixel(p);
    }

    public void OnLayerNameChange(string s)
    {
        if(this.Layer != null)
        {   
            this.Layer.LayerName = this._NameEdit.Text;
        }
    }

    public void OnLayerMirrorXChange()
    {
        if(this.Layer != null)
        {
            this.Layer.MirrorX = this._MirrorXEdit.Pressed;
        }
    }

    public void OnLayerMirrorYChange()
    {
        if(this.Layer != null)
        {
            this.Layer.MirrorY = this._MirrorYEdit.Pressed;
        }
    }

    public void OnEditTemplate()
    {
        this.GetTree().CallGroup("pattern_designer", "EditLayer", this.Layer);
    }

    public void LoadLayer(GenLayer l)
    {
        this._Pixels.LoadLayer(l);
        this._Cycles.LoadLayer(l);

        this._NameEdit.Editable = true;
        this._MirrorXEdit.Disabled = false;
        this._MirrorYEdit.Disabled = false;

        this.Layer = l;
        this._NameEdit.Text = l.Data.Name;
        this._MirrorXEdit.Pressed = l.Data.MirrorX;
        this._MirrorYEdit.Pressed = l.Data.MirrorY;
    }

    public void LoadCanvas(GenCanvas c)
    {
        if(c.Layers.Count > 0)
        {
            this.LoadLayer(c.Layers[0]);
        }
    }

    public void CorrectPixelName(string oldName, string newName)
    {
        foreach(Node n in this._Pixels.Pixels)
        {
            if(n is PixelProperties p)
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

    public void ActivateLayerInspectorTab()
    {
        var _par = this.GetParent<InspectorView>();
        _par.CurrentTab = this.GetIndex();
    }
}

public enum AddMenuOption
{
    Pixel,
    Cycle,
}
