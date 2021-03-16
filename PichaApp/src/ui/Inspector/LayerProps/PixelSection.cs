using Godot;

using PichaLib;

public class PixelSection : BaseSection
{
    public override void _Ready()
    {
        this.SectionTitle = "Pixels";
        base._Ready();
    }

    public void LayerLoad(GenLayer l)
    {
        foreach(Pixel p in l.Data.Pixels.Values)
        { 
            var _props = new PixelProps() {
                SectionTitle = p.Name
            };
            
            this.SectionGrid.AddChild(_props);
            _props.PixelLoad(p);
        }
    }
}