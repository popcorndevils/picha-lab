using Godot;

public class BaseSection : PanelContainer
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

    public VBoxContainer _ALL = new VBoxContainer() {
        SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill,
    };
    public Button SectionHeader = new Button() {
        Text = "BLANK",
        Flat = true,
        SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
        FocusMode = FocusModeEnum.None,
    };
    public VBoxContainer SectionContent = new VBoxContainer();
    public GridContainer SectionGrid = new GridContainer();
    public HBoxContainer HeaderContainer = new HBoxContainer();

    public override void _Ready()
    {
        this.SectionHeader.Text = this.SectionTitle;

        this.HeaderContainer.AddChild(this.SectionHeader);
        this.SectionContent.AddChild(this.SectionGrid);

        this._ALL.AddChild(this.HeaderContainer);
        this._ALL.AddChild(this.SectionContent);

        this.AddChild(this._ALL);

        this.SectionHeader.Connect("pressed", this, "_SectionClicked");
        
        this.SectionContent.Visible = false;
        this.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
        this.SectionContent.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this.SectionGrid.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
    }

    public void _SectionClicked()
        { this.SectionContent.Visible = !this.SectionContent.Visible; }
}