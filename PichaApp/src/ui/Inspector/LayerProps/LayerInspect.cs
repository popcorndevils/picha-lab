using System;
using Godot;

using PichaLib;

public class LayerInspect : ScrollContainer
{
    public GenLayer GenLayer;
    public Layer Layer;

    private VBoxContainer _Contents;
    private GridContainer _GenSettings;
    private PixelSection _Pixels;
    private CycleSection _Cycles;

    private LineEdit _NameEdit;
    private CheckBox _MirrorXEdit;
    private CheckBox _MirrorYEdit;
    private Button _OpenTemplate;

    public override void _Ready()
    {
        this.AddToGroup("gp_layer_gui");
        
        this._Contents = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._GenSettings = new GridContainer() {
            Columns = 2,
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Pixels = new PixelSection();
        this._Cycles = new CycleSection();

        this._OpenTemplate = new Button() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Icon = GD.Load<Texture>("res://res/icons/grid-white.svg"),
            FocusMode = FocusModeEnum.None,
        };

        var _buttons = new HBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        };

        this._NameEdit = new LineEdit() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };
        
        this._MirrorXEdit = new CheckBox() {
            SizeFlagsHorizontal = 0,
            Text = "X",
        };

        this._MirrorYEdit = new CheckBox() {
            SizeFlagsHorizontal = 0,
            Text = "Y",
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

        this._Contents.AddChildren(this._GenSettings, this._Pixels, this._Cycles);
        this.AddChild(this._Contents);

        _mirrorGroup.AddChildren(this._MirrorXEdit, this._MirrorYEdit);
        _buttons.AddChild(this._OpenTemplate);

        this._GenSettings.AddChildren(new Control(), _buttons, _nameLabel, 
            this._NameEdit, _mirrorLabel, _mirrorGroup);

        this._OpenTemplate.Connect("pressed", this, "OnEditTemplate");
        this._NameEdit.Connect("text_changed", this, "OnLayerSettingEdit");
        this._MirrorXEdit.Connect("pressed", this, "OnLayerSettingEdit");
        this._MirrorYEdit.Connect("pressed", this, "OnLayerSettingEdit");
    }

    public void OnLayerSettingEdit(params object[] args) { this.OnLayerSettingEdit(); } 
    public void OnLayerSettingEdit()
    {
        if(this.Layer != null)
        {
            this.Layer.Name = this._NameEdit.Text;
            this.Layer.MirrorX = this._MirrorXEdit.Pressed;
            this.Layer.MirrorY = this._MirrorYEdit.Pressed;
        }
    }

    public void OnEditTemplate()
    {
        this.GetTree().CallGroup("pattern_designer", "EditLayer", this.GenLayer);
    }

    public void LoadLayer(GenLayer l)
    {
        this._Pixels.LoadLayer(l.Data);
        this._Cycles.LoadLayer(l.Data);

        this.GenLayer = l;
        this.Layer = l.Data;
        this._NameEdit.Text = l.Data.Name;
        this._MirrorXEdit.Pressed = l.Data.MirrorX;
        this._MirrorYEdit.Pressed = l.Data.MirrorY;
    }

    public void OnAddMenuSelected(int i)
    {
        switch((AddMenuOption)i)
        {
            case AddMenuOption.Pixel:
                GD.Print("PIXEL");
                break;
            case AddMenuOption.Cycle:
                GD.Print("CYCLE");
                break;
            default:
                GD.PrintErr($"Unable to Parse Add Menu action \"{(AddMenuOption)i}\".");
                break;
        }
    }

    public void DeletePixel(PixelProps p)
    {
        GD.Print($"DELETING {p.Pixel.Name}");
        // this.Layer.DeletePixel(p.Pixel);
    }
}

public enum AddMenuOption
{
    Pixel,
    Cycle,
}
