using System;
using Godot;

using PichaLib;

public class PixelSection : BaseSection
{
    private GenLayer _Layer;

    public override void _Ready()
    {
        this.SectionTitle = "Pixel Types";
        base._Ready();
    }

    public void LoadLayer(Layer l)
    {
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
}