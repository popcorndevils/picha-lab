using System;
using Godot;

public class CanvasInspect : ScrollContainer
{
    public GenCanvas Canvas;

    private VBoxContainer _Contents;
    private GridContainer _Settings;

    private CheckBox _AutoGenEdit;
    private SpinBox _AutoGenTimeEdit;
    private ColorPickerButton _FGEdit;
    private ColorPickerButton _BGEdit;
    private SpinBox _WidthEdit;
    private SpinBox _HeightEdit;

    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_gui");
        
        this._Contents = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Settings = new GridContainer() {
            Columns = 2,
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        var _autoGenLabel = new Label() {
            Text = "Auto-Gen",
            Align = Label.AlignEnum.Right,
        };


        this._AutoGenEdit = new CheckBox() {
            SizeFlagsHorizontal = 0,
        };

        var _autoGenTimeLabel = new Label() {
            Text = "Time-to-Gen",
            Align = Label.AlignEnum.Right,
        };

        var _bgColorsLabel = new Label() {
            Text = "Transparency",
            Align = Label.AlignEnum.Right,
        };

        var _bgColorsBox = new HBoxContainer() {
            SizeFlagsHorizontal = 0,
        };

        this._AutoGenTimeEdit = new SpinBox() {
            MinValue = 0,
            MaxValue = 99,
            Step = .1f,
        };

        var _sizeLabel = new Label() {
            Text = "Size",
            Align = Label.AlignEnum.Right,
        };

        var _sizeBox = new HBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        };

        this._WidthEdit = new SpinBox() {
            MinValue = 1,
            MaxValue = 9999,
            Step = 1f,
        };

        this._HeightEdit = new SpinBox() {
            MinValue = 1,
            MaxValue = 9999,
            Step = 1f,
        };

        this._BGEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
        };

        this._FGEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
        };

        this._Contents.AddChild(this._Settings);
        this.AddChild(this._Contents);

        _bgColorsBox.AddChildren(this._FGEdit, this._BGEdit);
        _sizeBox.AddChildren(this._WidthEdit, this._HeightEdit);

        this._Settings.AddChildren(
            _autoGenLabel, this._AutoGenEdit,
            _autoGenTimeLabel, this._AutoGenTimeEdit,
            _bgColorsLabel, _bgColorsBox,
            _sizeLabel, _sizeBox);

        this._AutoGenEdit.Connect("pressed", this, "OnCanvasEdit");
        this._AutoGenTimeEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._WidthEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._HeightEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._BGEdit.Connect("color_changed", this, "OnCanvasEdit");
        this._FGEdit.Connect("color_changed", this, "OnCanvasEdit");
    }

    public void LoadCanvas(GenCanvas c)
    {
        this.Canvas = c;

        this._AutoGenEdit.Pressed = c.AutoGen;
        this._FGEdit.Color = c.FG;
        this._BGEdit.Color = c.BG;

        this._AutoGenTimeEdit.Disconnect("value_changed", this, "OnCanvasEdit");
        this._WidthEdit.Disconnect("value_changed", this, "OnCanvasEdit");
        this._HeightEdit.Disconnect("value_changed", this, "OnCanvasEdit");
        this._AutoGenTimeEdit.Value = c.TimeToGen;
        var _size = c.Size;
        this._WidthEdit.Value = _size.x;
        this._HeightEdit.Value = _size.y;
        this._AutoGenTimeEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._WidthEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._HeightEdit.Connect("value_changed", this, "OnCanvasEdit");
    }

    public void OnCanvasEdit(params object[] args)
        { this.OnCanvasEdit(); }
    public void OnCanvasEdit()
    {
        this.Canvas.BG = this._BGEdit.Color;
        this.Canvas.FG = this._FGEdit.Color;
        this.Canvas.AutoGen = this._AutoGenEdit.Pressed;
        this.Canvas.TimeToGen = (float)this._AutoGenTimeEdit.Value;
        this.Canvas.Size = new Vector2((int)this._WidthEdit.Value, (int)this._HeightEdit.Value);
    }
}
