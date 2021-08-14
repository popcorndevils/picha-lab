using System;
using Godot;

public class CanvasInspector : VBoxContainer
{
    public GenCanvas Canvas;

    private bool _IgnoreSignals = false;

    private GridContainer _Settings;
    private CheckBox _AutoGenEdit;
    private Button _Regen;
    private SpinBox _AutoGenTimeEdit;
    private SpinBox _AnimTimeEdit;
    private ColorPickerButton _FGEdit;
    private ColorPickerButton _BGEdit;
    private SpinBox _WidthEdit;
    private SpinBox _HeightEdit;

    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_gui");
        this.SizeFlagsVertical = (int)SizeFlags.Fill;
        this.SizeFlagsHorizontal = (int)SizeFlags.Fill;

        this._Settings = new GridContainer() {
            Columns = 2,
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        var _autoGenLabel = new Label() {
            Text = "Auto-Gen",
            Align = Label.AlignEnum.Right,
        };

        var _genBox = new HBoxContainer() {
            SizeFlagsHorizontal = 0,
            FocusMode = FocusModeEnum.None,
        };

        this._AutoGenEdit = new CheckBox() {
            SizeFlagsHorizontal = 0,
            FocusMode = FocusModeEnum.None,
            HintTooltip = PDefaults.ToolHints.Canvas.AutoGenerate,
        };

        this._Regen = new Button() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Icon = GD.Load<Texture>("res://res/icons/all_inclusive_white.svg"),
            FocusMode = FocusModeEnum.None,
            HintTooltip = PDefaults.ToolHints.Canvas.ReGenerate,
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
            MinValue = .1,
            MaxValue = 99,
            Step = .1f,
            FocusMode = FocusModeEnum.None,
        };

        var _sizeLabel = new Label() {
            Text = "Canvas Size",
            Align = Label.AlignEnum.Right,
        };

        var _sizeBox = new HBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        };

        this._WidthEdit = new SpinBox() {
            MinValue = 1,
            MaxValue = 9999,
            Step = 1f,
            FocusMode = FocusModeEnum.None,
        };

        this._HeightEdit = new SpinBox() {
            MinValue = 1,
            MaxValue = 9999,
            Step = 1f,
            FocusMode = FocusModeEnum.None,
        };

        this._BGEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
            FocusMode = FocusModeEnum.None,
        };

        this._FGEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
            FocusMode = FocusModeEnum.None,
        };

        this.AddChild(this._Settings);

        _genBox.AddChildren(this._AutoGenEdit, this._Regen);
        _bgColorsBox.AddChildren(this._FGEdit, this._BGEdit);
        _sizeBox.AddChildren(this._WidthEdit, this._HeightEdit);

        this._Settings.AddChildren(
            _autoGenLabel, _genBox,
            _autoGenTimeLabel, this._AutoGenTimeEdit,
            _bgColorsLabel, _bgColorsBox,
            _sizeLabel, _sizeBox);

        this._AutoGenEdit.Connect("pressed", this, "OnCanvasEdit");
        this._AutoGenTimeEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._Regen.Connect("pressed", this, "OnRegen");
        this._WidthEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._HeightEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._BGEdit.Connect("color_changed", this, "OnCanvasEdit");
        this._FGEdit.Connect("color_changed", this, "OnCanvasEdit");
    }

    public void LoadCanvas(GenCanvas c)
    {
        this.Canvas = c;
        var _size = c.Size;

        this._AutoGenEdit.Pressed = c.AutoGen;
        this._FGEdit.Color = c.FG;
        this._BGEdit.Color = c.BG;

        this._IgnoreSignals = true;
        this._AutoGenTimeEdit.Value = c.TimeToGen;
        this._WidthEdit.Value = _size.x;
        this._HeightEdit.Value = _size.y;
        this._IgnoreSignals = false;
    }

    public void OnRegen() { if(this.Canvas != null) { this.Canvas.Generate(); } }
    public void OnCanvasEdit(params object[] args) { this.OnCanvasEdit(); }
    public void OnCanvasEdit()
    {
        if(!this._IgnoreSignals && this.Canvas != null)
        {
            this.Canvas.BG = this._BGEdit.Color;
            this.Canvas.FG = this._FGEdit.Color;
            this.Canvas.AutoGen = this._AutoGenEdit.Pressed;
            this.Canvas.TimeToGen = (float)this._AutoGenTimeEdit.Value;
            this.Canvas.Size = new Vector2((int)this._WidthEdit.Value, (int)this._HeightEdit.Value);
        }
    }
}
