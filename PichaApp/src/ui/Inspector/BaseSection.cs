using Godot;

public abstract class BaseSection : VBoxContainer
{
    public string SectionTitle;
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
    }

    public void _SectionClicked()
    {
        GD.Print(this.SectionTitle);
    }
}