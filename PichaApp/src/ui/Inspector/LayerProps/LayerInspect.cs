using System;
using Godot;

using PichaLib;

public class LayerInspect : ScrollContainer
{
    public Layer Layer;

    private VBoxContainer _Contents;
    private GridContainer _GenSettings;
    private PixelSection _Pixels;
    private CycleSection _Cycles;

    private LineEdit _NameEdit;
    private CheckBox _MirrorXEdit;
    private CheckBox _MirrorYEdit;

    private MenuButton _AddMenu;
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

        this._AddMenu = new MenuButton() {
            Text = "+",
            Align = Button.TextAlign.Right,
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Flat = false,
        };

        var _pop = this._AddMenu.GetPopup();
        _pop.Connect("id_pressed", this, "OnAddMenuSelected");
        foreach(int v in Enum.GetValues(typeof(AddMenuOption)))
        {
            _pop.AddItem(Enum.GetName(typeof(AddMenuOption), v), v);
        }

        this._OpenTemplate = new Button() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Icon = GD.Load<Texture>("res://res/icons/grid-white.svg"),
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

        this._Contents.AddChild(this._GenSettings);
        this._Contents.AddChild(this._Pixels);
        this._Contents.AddChild(this._Cycles);
        this.AddChild(this._Contents);

        _mirrorGroup.AddChild(this._MirrorXEdit);
        _mirrorGroup.AddChild(this._MirrorYEdit);

        _buttons.AddChild(this._OpenTemplate);
        _buttons.AddChild(this._AddMenu);
        this._GenSettings.AddChild(new Control());
        this._GenSettings.AddChild(_buttons);
        this._GenSettings.AddChild(_nameLabel);
        this._GenSettings.AddChild(this._NameEdit);
        this._GenSettings.AddChild(_mirrorLabel);
        this._GenSettings.AddChild(_mirrorGroup);

        this._NameEdit.Connect("text_changed", this, "OnLayerSettingEdit");
        this._MirrorXEdit.Connect("pressed", this, "OnLayerSettingEdit");
        this._MirrorYEdit.Connect("pressed", this, "OnLayerSettingEdit");
    }

    public void OnLayerSettingEdit(params object[] args)
        { this.OnLayerSettingEdit(); }

    public void OnLayerSettingEdit()
    {
        this.Layer.Name = this._NameEdit.Text;
        this.Layer.MirrorX = this._MirrorXEdit.Pressed;
        this.Layer.MirrorY = this._MirrorYEdit.Pressed;
    }

    public void LoadLayer(GenLayer l)
    {
        this._Pixels.LoadLayer(l.Data);
        this._Cycles.LoadLayer(l.Data);

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
