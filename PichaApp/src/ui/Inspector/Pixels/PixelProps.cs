using System;
using Godot;

using PichaLib;

public delegate void PixelChangedHandler(Pixel p);

public class PixelProps : BaseSection
{
    public event PixelChangedHandler PixelChanged;
    public Pixel Pixel;

    private bool _IgnoreSignals = false;

    public bool GenColDisabled {
        get => this._ColorEdit.Disabled;
        set {
            this._ColorEdit.Disabled = value;
            if(value) 
            {
                this._GenColLabel.Modulate = new Color(.75f, .75f, .75f);
                this._ColorEdit.Modulate = new Color(.75f, .75f, .75f);
            }
            else
            {
                this._ColorEdit.Modulate = new Color(1f, 1f, 1f);
                this._GenColLabel.Modulate = new Color(1f, 1f, 1f);
            }
        }
    }

    // SETTINGS
    private LineEdit _NameEdit;
    private ColorPickerButton _ColorEdit;
    private ColorPickerButton _PaintEdit;
    private CheckBox _RandomColEdit;
    private OptionButton _FadeDirectionEdit;
    private SpinBox _BrightNoiseEdit;
    private SpinBox _MinSaturationEdit;
    private Button _Delete;
    private StyleBoxFlat _PanelStyle;
    private Label _GenColLabel;

    // Tool Hints
    private const string _HintColor = "When enabled, sets color used in\ngenerated layer for pixel type.";

    public override void _Ready()
    {
        base._Ready();
        this.SectionGrid.Columns = 2;

        this._PanelStyle = new StyleBoxFlat() {
            BorderWidthTop = 0,
            BorderWidthBottom = 0,
            BorderWidthLeft = 5,
            BorderWidthRight = 0,
            ContentMarginBottom = 6,
            ContentMarginLeft = 10,
            ContentMarginRight = 6,
            ContentMarginTop = 6,
            BgColor = Chroma.CreateFromHex("#44463C").ToGodotColor(),
        };

        this.AddStyleboxOverride("panel", this._PanelStyle);

        this._Delete = new Button() {
            Text = "x",
            SizeFlagsHorizontal = 0,
            FocusMode = FocusModeEnum.None,
        };

        this._NameEdit = new LineEdit() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this._GenColLabel = new Label() {
            Text = "Color",
            Align = Label.AlignEnum.Right,
            HintTooltip = _HintColor,
            MouseFilter = MouseFilterEnum.Pass,
        }; 

        this._ColorEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
            HintTooltip = _HintColor,
        };

        this._PaintEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
        };

        this._RandomColEdit = new CheckBox();

        this._FadeDirectionEdit = new OptionButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this._BrightNoiseEdit = new SpinBox() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            MinValue = 0.0f,
            MaxValue = 1.0f,
            Step = .05f,
        };

        this._MinSaturationEdit = new SpinBox() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            MinValue = 0.0f,
            MaxValue = 1.0f,
            Step = .05f,
        };

        var _nameLabel = new Label() {
            Text = "Pixel Name",
            Align = Label.AlignEnum.Right,
        };

        var _paintLabel = new Label() {
            Text = "Paint",
            Align = Label.AlignEnum.Right,
        }; 

        var _randomColLabel = new Label() {
            Text = "Gen Color",
            Align = Label.AlignEnum.Right,
        }; 

        var _fadeDirectionLabel = new Label() {
            Text = "Fade",
            Align = Label.AlignEnum.Right,
        }; 

        var _brightNoiseLabel = new Label() {
            Text = "Noise",
            Align = Label.AlignEnum.Right,
        }; 

        var _minSaturationLabel = new Label() {
            Text = "Min Sat",
            Align = Label.AlignEnum.Right,
        }; 

        this.HeaderContainer.AddChild(this._Delete);

        this.SectionGrid.AddChild(_nameLabel);
        this.SectionGrid.AddChild(this._NameEdit);

        this.SectionGrid.AddChild(_randomColLabel);
        this.SectionGrid.AddChild(this._RandomColEdit);

        this.SectionGrid.AddChild(this._GenColLabel);
        this.SectionGrid.AddChild(this._ColorEdit);

        this.SectionGrid.AddChild(_paintLabel);
        this.SectionGrid.AddChild(this._PaintEdit);

        this.SectionGrid.AddChild(_fadeDirectionLabel);
        this.SectionGrid.AddChild(this._FadeDirectionEdit);

        this.SectionGrid.AddChild(_brightNoiseLabel);
        this.SectionGrid.AddChild(this._BrightNoiseEdit);

        this.SectionGrid.AddChild(_minSaturationLabel);
        this.SectionGrid.AddChild(this._MinSaturationEdit);
        
        this._Delete.Connect("pressed", this, "OnDeletePixel");
        this._NameEdit.Connect("text_changed", this, "OnPixelSettingEdit");
        this._ColorEdit.Connect("color_changed", this, "OnPixelSettingEdit");
        this._PaintEdit.Connect("color_changed", this, "OnPixelSettingEdit");
        this._RandomColEdit.Connect("pressed", this, "OnPixelSettingEdit");
        this._FadeDirectionEdit.Connect("item_selected", this, "OnPixelSettingEdit");
        this._BrightNoiseEdit.Connect("value_changed", this, "OnPixelSettingEdit");
        this._MinSaturationEdit.Connect("value_changed", this, "OnPixelSettingEdit");
    }

    public void PixelLoad(Pixel p)
    {
        this.Pixel = p;
        this._NameEdit.Text = p.Name;
        this._ColorEdit.Color = p.Color.ToGodotColor();
        this._PaintEdit.Color = p.Paint.ToGodotColor();
        this._RandomColEdit.Pressed = p.RandomCol;
        this.GenColDisabled = p.RandomCol;

        this._PanelStyle.BorderColor = p.Paint.ToGodotColor();

        this._FadeDirectionEdit.Clear();
        foreach(int i in Enum.GetValues(typeof(FadeDirection)))  
        {  
            this._FadeDirectionEdit.AddItem(Enum.GetName(typeof(FadeDirection), i), i);
        }

        this._FadeDirectionEdit.Selected = (int)p.FadeDirection;
        
        this._IgnoreSignals = true;

        this._BrightNoiseEdit.Value = p.BrightNoise;
        this._MinSaturationEdit.Value = p.MinSaturation;

        this._IgnoreSignals = false;
    }

    public void OnPixelSettingEdit(params object[] args) { this.OnPixelSettingEdit(); }
    public void OnPixelSettingEdit()
    {
        if(!this._IgnoreSignals)
        {
            this.SectionTitle = this._NameEdit.Text; 
            this.Pixel.Name = this._NameEdit.Text;
            this.Pixel.RandomCol = this._RandomColEdit.Pressed;
            this.GenColDisabled = this._RandomColEdit.Pressed;
            this.Pixel.BrightNoise = (float)this._BrightNoiseEdit.Value;
            this.Pixel.MinSaturation = (float)this._MinSaturationEdit.Value;
            this.Pixel.Color = this._ColorEdit.Color.ToChroma();
            this.Pixel.Paint = this._PaintEdit.Color.ToChroma();
            this.Pixel.FadeDirection = (FadeDirection)this._FadeDirectionEdit.Selected;
            this._PanelStyle.BorderColor = this._PaintEdit.Color;

            this.PixelChanged?.Invoke(this.Pixel);
        }
    }

    public void OnDeletePixel()
    {
        GD.Print($"DELETE PIXEL {this.SectionTitle}");
    }
}