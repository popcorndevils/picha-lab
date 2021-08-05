using System;
using Godot;

using PichaLib;


public class PixelSection : BaseSection
{
    private GenLayer _Layer;
    private Button _NewPixel;

    public override void _Ready()
    {
        base._Ready();
        this._NewPixel = new Button() {
            Text = "+",
            SizeFlagsHorizontal = 0,
            FocusMode = FocusModeEnum.None,
        };

        this.HeaderContainer.AddChild(this._NewPixel);

        this.SectionTitle = "Pixel Types";

        this._NewPixel.Connect("pressed", this, "OnNewPixel");
    }

    public void LoadLayer(GenLayer l)
    {
        this._Layer = l;
        this._NewPixel.Disabled = false;

        foreach(Node n in this.SectionGrid.GetChildren())
        { this.SectionGrid.RemoveChild(n); }

        foreach(Pixel p in l.Pixels.Values)
        { 
            var _props = new PixelProperties() {
                SectionTitle = p.Name
            };

            _props.PixelDeleted += this.OnDeletePixel;
            _props.PixelChanged += this.OnChangePixel;

            this.SectionGrid.AddChild(_props);
            _props.SectionHeader.Align = Button.TextAlign.Left;
            _props.PixelLoad(p);
        }
    }

    public void OnNewPixel()
    {
        GD.Print("NEW PIXEL NOW!");
    }

    public void OnChangePixel(PixelProperties p)
    {

    }
    public void OnDeletePixel(PixelProperties p)
    {
        this._Layer.DeletePixel(p.Pixel);
        p.QueueFree();
        this.GetTree().CallGroup("gp_layer_gui", "LoadLayer", this._Layer);
    }
}