using System;
using Godot;

using PichaLib;

public delegate void PixelChangedHandler(Pixel p, string property, object value);
public delegate void PixelDeleteHandler(Pixel p);

public class PixelProps : BaseSection
{
    public event PixelChangedHandler PixelChanged;
    public event PixelDeleteHandler PixelDeleted;

    public Pixel Pixel;

    private bool _IgnoreSignals = false;
    private bool _NameEdited = false;
    private string _OldName = "";

    public bool GenColDisabled {
        get => this.ColorEdit.Disabled;
        set {
            this.ColorEdit.Disabled = value;
            if(value) 
            {
                this._GenColLabel.Modulate = new Color(.75f, .75f, .75f);
                this.ColorEdit.Modulate = new Color(.75f, .75f, .75f);
            }
            else
            {
                this.ColorEdit.Modulate = new Color(1f, 1f, 1f);
                this._GenColLabel.Modulate = new Color(1f, 1f, 1f);
            }
        }
    }

    // SETTINGS
    public LineEdit NameEdit;
    public ColorPickerButton ColorEdit;
    public ColorPickerButton PaintEdit;
    public CheckBox RandomColEdit;
    public OptionButton FadeDirectionEdit;
    public SpinBox BrightNoiseEdit;
    public SpinBox MinSaturationEdit;
    private Button _Delete;
    private StyleBoxFlat _PanelStyle;
    private Label _GenColLabel;

    public override void _Ready()
    {
        base._Ready();
        this.SectionGrid.Columns = 2;

        this.Theme = GD.Load<Theme>("res://res/theme/SubSection.tres");

        this._PanelStyle = new StyleBoxFlat() {
            BorderWidthTop = 0,
            BorderWidthBottom = 0,
            BorderWidthLeft = 5,
            BorderWidthRight = 0,
            ContentMarginBottom = 4,
            ContentMarginLeft = 15,
            ContentMarginRight = 4,
            ContentMarginTop = 4,
            BgColor = Chroma.CreateFromBytes(60, 60, 60, 255).ToGodotColor(),
        };

        this.AddStyleboxOverride("panel", this._PanelStyle);

        this._Delete = new Button() {
            SizeFlagsHorizontal = (int)SizeFlags.ShrinkEnd,
            Icon = GD.Load<Texture>("res://res/icons/delete_white.svg"),
            FocusMode = FocusModeEnum.None,
            HintTooltip = PDefaults.ToolHints.Pixel.DeletePixel,
        };

        this.NameEdit = new LineEdit() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            HintTooltip = PDefaults.ToolHints.Pixel.PixelName,
        };

        this._GenColLabel = new Label() {
            Text = "Color",
            Align = Label.AlignEnum.Right,
            HintTooltip = PDefaults.ToolHints.Pixel.Color,
            MouseFilter = MouseFilterEnum.Pass,
        }; 

        this.ColorEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
            HintTooltip = PDefaults.ToolHints.Pixel.Color,
        };

        this.PaintEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
        };

        this.RandomColEdit = new CheckBox();

        this.FadeDirectionEdit = new OptionButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this.BrightNoiseEdit = new SpinBox() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            MinValue = 0.0f,
            MaxValue = 1.0f,
            Step = .05f,
        };

        this.MinSaturationEdit = new SpinBox() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            MinValue = 0.0f,
            MaxValue = 1.0f,
            Step = .05f,
        };

        var _nameLabel = new Label() {
            Text = "Pixel Name",
            Align = Label.AlignEnum.Right,
            HintTooltip = PDefaults.ToolHints.Pixel.PixelName,
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
        this.SectionGrid.AddChild(this.NameEdit);

        this.SectionGrid.AddChild(_randomColLabel);
        this.SectionGrid.AddChild(this.RandomColEdit);

        this.SectionGrid.AddChild(this._GenColLabel);
        this.SectionGrid.AddChild(this.ColorEdit);

        this.SectionGrid.AddChild(_paintLabel);
        this.SectionGrid.AddChild(this.PaintEdit);

        this.SectionGrid.AddChild(_fadeDirectionLabel);
        this.SectionGrid.AddChild(this.FadeDirectionEdit);

        this.SectionGrid.AddChild(_brightNoiseLabel);
        this.SectionGrid.AddChild(this.BrightNoiseEdit);

        this.SectionGrid.AddChild(_minSaturationLabel);
        this.SectionGrid.AddChild(this.MinSaturationEdit);
        
        this._Delete.Connect("pressed", this, "OnDeletePixel");

        // Setting Signals
        this.SectionHeader.Connect("pressed", this, "OnSectionPress");
        this.NameEdit.Connect("text_changed", this, "OnNameEdit");
        this.NameEdit.Connect("text_entered", this, "OnNameEntered");
        this.ColorEdit.Connect("color_changed", this, "OnColorEdit");
        this.PaintEdit.Connect("color_changed", this, "OnPaintEdit");
        this.RandomColEdit.Connect("pressed", this, "OnRandomColEdit");
        this.FadeDirectionEdit.Connect("item_selected", this, "OnDirectionEdit");
        this.BrightNoiseEdit.Connect("value_changed", this, "OnBrightEdit");
        this.MinSaturationEdit.Connect("value_changed", this, "OnMinSatEdit");
    }

    public void PixelLoad(Pixel p)
    {
        this.Pixel = p;
        this.NameEdit.Text = p.Name;
        this.ColorEdit.Color = p.Color.ToGodotColor();
        this.PaintEdit.Color = p.Paint.ToGodotColor();
        this.RandomColEdit.Pressed = p.RandomCol;
        this.GenColDisabled = p.RandomCol;
        this._PanelStyle.BorderColor = p.Paint.ToGodotColor();
        this.FadeDirectionEdit.Clear();

        foreach(int i in Enum.GetValues(typeof(FadeDirection)))  
        {  
            this.FadeDirectionEdit.AddItem(Enum.GetName(typeof(FadeDirection), i), i);
        }

        this.FadeDirectionEdit.Selected = (int)p.FadeDirection;
        
        this._IgnoreSignals = true;

        this.BrightNoiseEdit.Value = p.BrightNoise;
        this.MinSaturationEdit.Value = p.MinSaturation;

        this._IgnoreSignals = false;
    }


    // HANDLE SIGNALS
    public void OnNameEdit(string text)
    {
        if(!this._NameEdited)
        {
            this._OldName = this.SectionTitle;
            this.SectionTitle = $"*{this.SectionTitle}";
            this._NameEdited = true;
        }
    }

    public void OnSectionPress()
    {
        if(this._NameEdited)
        {
            this.OnNameEntered(this.NameEdit.Text);
        }
    }

    public void OnNameEntered(string text)
    {
        if(this._NameEdited)
        {
            if(text != this._OldName)
            {
                this.SectionTitle = text;
                this.PixelChanged?.Invoke(this.Pixel, "Name", text);
            }
            else
            {
                this.SectionTitle = this._OldName;
            }
        }
        this._NameEdited = false;
    }

    public void OnRandomColEdit()
    {
        this.GenColDisabled = this.RandomColEdit.Pressed;
        this.PixelChanged?.Invoke(this.Pixel, "RandomCol", this.GenColDisabled);
    }

    public void OnBrightEdit(float value)
    {
        if(!this._IgnoreSignals)
        {
            this.PixelChanged?.Invoke(this.Pixel, "BrightNoise", value);
        }
    }

    public void OnMinSatEdit(float value)
    {
        if(!this._IgnoreSignals)
        {
            this.PixelChanged?.Invoke(this.Pixel, "MinSaturation", value);
        }
    }

    public void OnColorEdit(Color c)
    {
        this.PixelChanged.Invoke(this.Pixel, "Color", c.ToChroma());
    }

    public void OnPaintEdit(Color c)
    {
        this._PanelStyle.BorderColor = this.PaintEdit.Color;
        this.PixelChanged.Invoke(this.Pixel, "Paint", c.ToChroma());
    }

    public void OnDirectionEdit(int selected)
    {
        this.PixelChanged?.Invoke(this.Pixel, "FadeDirection", (FadeDirection)this.FadeDirectionEdit.Selected);
    }

    public void OnDeletePixel()
    {
        this.QueueFree();
        this.PixelDeleted?.Invoke(this.Pixel);
    }
}