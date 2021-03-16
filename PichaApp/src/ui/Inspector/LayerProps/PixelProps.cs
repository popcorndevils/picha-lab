using Godot;

using PichaLib;

public delegate void PixelChangedHandler(Pixel p);

public class PixelProps : BaseSection
{
    public event PixelChangedHandler PixelChanged;
    public Pixel Pixel;

    public override void _Ready()
    {
        base._Ready();
        this.SectionGrid.Columns = 2;
    }

    public void PixelLoad(Pixel p)
    {
        this._ClearChildren();
        this.Pixel = p;

        var _nameLabel = new Label() {
            Text = "NAME",
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            Align = Label.AlignEnum.Right,
        };

        var _nameEdit = new LineEdit() {
            Text = p.Name,
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this.SectionGrid.AddChild(_nameLabel);
        this.SectionGrid.AddChild(_nameEdit);

        _nameEdit.Connect("text_changed", this, "EditName");
    }

    private void _ClearChildren()
    {
        foreach(Node n in this.SectionGrid.GetChildren())
        {
            this.SectionGrid.RemoveChild(n);
        }
    }

    public void EditName(string s)
    {
        this.SectionTitle = s; 
        this.Pixel.Name = s;
        PixelChanged?.Invoke(Pixel);
    }
}