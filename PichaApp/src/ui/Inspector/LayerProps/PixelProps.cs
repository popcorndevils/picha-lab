using Godot;

using PichaLib;

public class PixelProps : BaseSection
{
    public override void _Ready()
    {
        base._Ready();
        this.SectionGrid.Columns = 2;
    }

    public void PixelLoad(Pixel p)
    {
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

    public void EditName(string s) { this.SectionTitle = s; }
}