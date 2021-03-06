using Godot;
using PichaLib;

public class LayerInspector : ScrollContainer
{
    // public GenLayer GenLayer;
    public GenLayer Layer;

    private VBoxContainer _Contents;
    private GridContainer _GenSettings;
    private Button _Delete;
    private CycleSection _Cycles;

    private LineEdit _NameEdit;
    private CheckBox _MirrorXEdit;
    private CheckBox _MirrorYEdit;

    public override void _Ready()
    {
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

        this._Cycles = new CycleSection();

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
            Text = "Name",
            Align = Label.AlignEnum.Right,
        };

        var _mirrorLabel = new Label() {
            Text = "Mirror",
            Align = Label.AlignEnum.Right,
        };

        var _nameDeleteBox = new HBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Contents.AddChildren(this._GenSettings, this._Cycles);
        this.AddChild(this._Contents);

        _mirrorGroup.AddChildren(this._MirrorXEdit, this._MirrorYEdit);
        _nameDeleteBox.AddChildren(this._NameEdit, this._Delete);

        this._GenSettings.AddChildren(_nameLabel, _nameDeleteBox, _mirrorLabel, _mirrorGroup);

        this._NameEdit.Connect("text_changed", this, "OnLayerNameChange");
        this._MirrorXEdit.Connect("pressed", this, "OnLayerMirrorXChange");
        this._MirrorYEdit.Connect("pressed", this, "OnLayerMirrorYChange");
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

    public void ChangePixelName(string oldName, string newName)
    {
        this._Cycles.PixelNameChange(oldName, newName);
    }

    public void OnEditTemplate()
    {
        this.GetTree().CallGroup("pattern_designer", "EditLayer", this.Layer);
    }

    public void LoadLayer(GenLayer l)
    {
        this._Cycles.LoadLayer(l);

        this._NameEdit.Editable = true;
        this._MirrorXEdit.Disabled = false;
        this._MirrorYEdit.Disabled = false;

        this.Layer = l;
        this._NameEdit.Text = l.Data.Name;
        this._MirrorXEdit.Pressed = l.Data.MirrorX;
        this._MirrorYEdit.Pressed = l.Data.MirrorY;
    }

    public void LoadLayer()
    {
        this._Cycles.LoadLayer();

        this._NameEdit.Editable = false;
        this._MirrorXEdit.Disabled = true;
        this._MirrorYEdit.Disabled = true;

        this.Layer = null;
        this._NameEdit.Text = "";
        this._MirrorXEdit.Pressed = false;
        this._MirrorYEdit.Pressed = false;
    }
}

public enum AddMenuOption
{
    Pixel,
    Cycle,
}
