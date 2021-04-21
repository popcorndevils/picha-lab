using System;
using Godot;

using PichaLib;

public delegate void PixelChangedHandler(Pixel p);

public class PixelProps : BaseSection
{
    public event PixelChangedHandler PixelChanged;
    public Pixel Pixel;

    private bool _IgnoreSignals = false;

    // SETTINGS
    private LineEdit _NameEdit;
    private ColorPickerButton _ColorEdit;
    private ColorPickerButton _PaintEdit;
    private CheckBox _RandomColEdit;
    private OptionButton _FadeDirectionEdit;
    private SpinBox _BrightNoiseEdit;
    private SpinBox _MinSaturationEdit;
    private Button _Delete;

    public override void _Ready()
    {
        base._Ready();
        this.SectionGrid.Columns = 2;

        this._Delete = new Button() {
            Text = "x",
            SizeFlagsHorizontal = 0,
        };

        this._NameEdit = new LineEdit() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this._ColorEdit = new ColorPickerButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
            RectMinSize = new Vector2(40, 0),
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

        var _colorLabel = new Label() {
            Text = "Gen Color",
            Align = Label.AlignEnum.Right,
        }; 

        var _paintLabel = new Label() {
            Text = "Template Color",
            Align = Label.AlignEnum.Right,
        }; 

        var _randomColLabel = new Label() {
            Text = "Randomize Color",
            Align = Label.AlignEnum.Right,
        }; 

        var _fadeDirectionLabel = new Label() {
            Text = "Fade Direction",
            Align = Label.AlignEnum.Right,
        }; 

        var _brightNoiseLabel = new Label() {
            Text = "Brightness Noise",
            Align = Label.AlignEnum.Right,
        }; 

        var _minSaturationLabel = new Label() {
            Text = "Minimum Saturation",
            Align = Label.AlignEnum.Right,
        }; 

        this.HeaderContainer.AddChild(this._Delete);

        this.SectionGrid.AddChild(_nameLabel);
        this.SectionGrid.AddChild(this._NameEdit);

        this.SectionGrid.AddChild(_colorLabel);
        this.SectionGrid.AddChild(this._ColorEdit);

        this.SectionGrid.AddChild(_paintLabel);
        this.SectionGrid.AddChild(this._PaintEdit);

        this.SectionGrid.AddChild(_randomColLabel);
        this.SectionGrid.AddChild(this._RandomColEdit);

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
        this._ColorEdit.Disabled = p.RandomCol;
        this._PaintEdit.Color = p.Paint.ToGodotColor();
        this._RandomColEdit.Pressed = p.RandomCol;

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
            this._ColorEdit.Disabled = this._RandomColEdit.Pressed;

            this.Pixel.BrightNoise = (float)this._BrightNoiseEdit.Value;
            this.Pixel.MinSaturation = (float)this._MinSaturationEdit.Value;
            this.Pixel.Color = this._ColorEdit.Color.ToChroma();
            this.Pixel.Paint = this._PaintEdit.Color.ToChroma();
            this.Pixel.FadeDirection = (FadeDirection)this._FadeDirectionEdit.Selected;

            this.PixelChanged?.Invoke(this.Pixel);
        }
    }

    public void OnDeletePixel()
    {
        this.GetTree().CallGroup("gp_layer_gui", "DeletePixel", this);
        this.GetParent().RemoveChild(this);
    }
}