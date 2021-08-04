using System;
using Godot;

using PichaLib;

public class LayerInspector : ScrollContainer
{
    public GenLayer GenLayer;
    public Layer Layer;

    private VBoxContainer _Contents;
    private GridContainer _GenSettings;
    private PixelSection _Pixels;
    private CycleProperties _Cycles;

    private LineEdit _NameEdit;
    private CheckBox _MirrorXEdit;
    private CheckBox _MirrorYEdit;

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
        this._Cycles = new CycleProperties();

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

        this._GenSettings.AddChildren(_nameLabel, 
            this._NameEdit, _mirrorLabel, _mirrorGroup);

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

    public void DeletePixel(PixelProps p)
    {
        GD.Print($"DELETING {p.Pixel.Name}");
    }
}

public enum AddMenuOption
{
    Pixel,
    Cycle,
}
