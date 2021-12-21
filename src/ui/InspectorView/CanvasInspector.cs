using System;
using Godot;
using PichaLib;

public class CanvasInspector : ScrollContainer
{
    public GenCanvas Canvas;

    private bool _IgnoreSignals = false;

    private VBoxContainer _Contents;
    private GridContainer _Settings;
    private CheckBox _AutoGenEdit;
    private Button _Regen;
    private SpinBox _AutoGenTimeEdit;
    private SpinBox _AnimTimeEdit;
    private ColorPickerButton _FGEdit;
    private ColorPickerButton _BGEdit;
    private SpinBox _WidthEdit;
    private SpinBox _HeightEdit;

    public PixelsInspect _Pixels;

    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_gui");

        this.SizeFlagsVertical = (int)SizeFlags.Fill;
        this.SizeFlagsHorizontal = (int)SizeFlags.Fill;

        this._Contents = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill
        };

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

        var _animTimeLabel = new Label() {
            Text = "Anim Length",
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

        this._AnimTimeEdit = new SpinBox() {
            MinValue = .1,
            MaxValue = 99,
            Step = .1f,
            FocusMode = FocusModeEnum.None,
        };

        var _sizeLabel = new Label() {
            Text = "Canvas Size",
            Align = Label.AlignEnum.Right,
        };

        var _sizeBox = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
        };

        this._WidthEdit = new SpinBox() {
            MinValue = 1,
            MaxValue = 9999,
            Step = 1f,
            FocusMode = FocusModeEnum.None,
            Prefix = "WD",
            Suffix = "px",
            HintTooltip = PDefaults.ToolHints.Canvas.CanvasWidth,
        };

        this._HeightEdit = new SpinBox() {
            MinValue = 1,
            MaxValue = 9999,
            Step = 1f,
            FocusMode = FocusModeEnum.None,
            Prefix = "HT",
            Suffix = "px",
            HintTooltip = PDefaults.ToolHints.Canvas.CanvasHeight,
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


        _genBox.AddChildren(this._AutoGenEdit, this._Regen);
        _bgColorsBox.AddChildren(this._FGEdit, this._BGEdit);
        _sizeBox.AddChildren(this._WidthEdit, this._HeightEdit);

        this._Pixels = new PixelsInspect();

        this._Pixels.PixelChanged += this.OnPixelChange;

        this.AddChild(this._Contents);
        this._Contents.AddChildren(this._Settings, this._Pixels);

        this._Settings.AddChildren(
            _autoGenLabel, _genBox,
            _autoGenTimeLabel, this._AutoGenTimeEdit,
            _animTimeLabel, this._AnimTimeEdit,
            _bgColorsLabel, _bgColorsBox,
            _sizeLabel, _sizeBox);

        this._AutoGenEdit.Connect("pressed", this, "OnCanvasEdit");
        this._AutoGenTimeEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._AnimTimeEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._Regen.Connect("pressed", this, "OnRegen");
        this._WidthEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._HeightEdit.Connect("value_changed", this, "OnCanvasEdit");
        this._BGEdit.Connect("color_changed", this, "OnCanvasEdit");
        this._FGEdit.Connect("color_changed", this, "OnCanvasEdit");
    }

    public void CorrectPixelName(string oldName, string newName)
    {
        foreach(Node n in this._Pixels.Pixels)
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

    public void OnPixelChange(Pixel p, string property, object value)
    {
        switch(property)
        {
            case "Name":
                var _oldName = p.Name;
                var _newName = this.Canvas.ChangePixelName(p, (string)value);
                // this._Cycles.PixelNameChange(_oldName, _newName);
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

    public void LoadCanvas()
    {
        this.Canvas = null;

        this._AutoGenEdit.Pressed = false;
        this._Pixels.LoadCanvas();

        this._IgnoreSignals = true;
        this._AutoGenTimeEdit.Value = 0;
        this._AnimTimeEdit.Value = 0;
        this._WidthEdit.Value = 0;
        this._HeightEdit.Value = 0;
        this._IgnoreSignals = false;
    }

    public void LoadCanvas(GenCanvas c)
    {
        this.Canvas = c;
        var _size = c.Size;

        this._Pixels.LoadCanvas(c);

        this._AutoGenEdit.Pressed = c.AutoGen;
        this._FGEdit.Color = c.FG;
        this._BGEdit.Color = c.BG;

        this._IgnoreSignals = true;
        this._AutoGenTimeEdit.Value = c.TimeToGen;
        this._AnimTimeEdit.Value = c.AnimTime;
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
            this.Canvas.AnimTime = (float)this._AnimTimeEdit.Value;
            this.Canvas.Size = new Vector2((int)this._WidthEdit.Value, (int)this._HeightEdit.Value);
        }
    }
}
