using System;
using Godot;

using PichaLib;

public class PixelSection : BaseSection
{
    private GenLayer _Layer;
    private Button _NewPixel = new Button() {
        Text = "Add Pixel Type",
        RectMinSize = new Vector2(0, 30),
        Disabled = true,
        FocusMode = FocusModeEnum.None,
    };

    public override void _Ready()
    {
        this.SectionTitle = "Pixel Types";
        this.SectionContent.AddChild(this._NewPixel);

        this._NewPixel.Connect("pressed", this, "OnNewPixel");

        base._Ready();
    }

    public void LoadLayer(Layer l)
    {
        this._NewPixel.Disabled = false;

        foreach(Node n in this.SectionGrid.GetChildren())
        { this.SectionGrid.RemoveChild(n); }

        foreach(Pixel p in l.Pixels.Values)
        { 
            var _props = new PixelProps() {
                SectionTitle = p.Name
            };

            
            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.PixelLoad(p);
        }
    }

    public void OnNewPixel()
    {
        GD.Print("NEW PIXEL");
    }
}