using Godot;

public class BaseSection : VBoxContainer
{
    private string _SectionTitle;
    public string SectionTitle {
        get => this._SectionTitle;
        set {
            this._SectionTitle = value;
            if(this.SectionHeader != null)
                { this.SectionHeader.Text = value; }
        }
    }
    public Button SectionHeader;
    public MarginContainer SectionContent;
    public GridContainer SectionGrid;

    public override void _Ready()
    {
        this.SectionHeader = new Button() {
            Text = this.SectionTitle,
            Flat = true,
        };

        this.SectionContent = new MarginContainer();
        this.SectionGrid = new GridContainer();

        this.SectionContent.AddChild(this.SectionGrid);
        this.AddChild(this.SectionHeader);
        this.AddChild(this.SectionContent);
        this.SectionHeader.Connect("pressed", this, "_SectionClicked");
        this.SectionContent.Visible = false;
        this.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this.SectionContent.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this.SectionGrid.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
    }

    public void _SectionClicked()
        { this.SectionContent.Visible = !this.SectionContent.Visible; }
}