using Godot;

using PichaLib;

public class BaseSection : PanelContainer
{
    public bool ContentVisibility {
        get => this.SectionContent.Visible;
        set {
            this.SectionContent.Visible = value;
            if(value)
            {
                this.SectionHeader.Icon = this._ContractIcon;
            }
            else
            {
                this.SectionHeader.Icon = this._ExpandIcon;
            }
        }
    }

    private string _SectionTitle;
    public string SectionTitle {
        get => this._SectionTitle;
        set {
            this._SectionTitle = value;
            if(this.SectionHeader != null)
            {
                this.SectionHeader.Text = value; 
            }
        }
    }

    public VBoxContainer _ALL = new VBoxContainer() {
        SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill,
    };

    public Button SectionHeader = new Button() {
        ExpandIcon = true,
        Text = "BLANK",
        Flat = false,
        SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
        FocusMode = FocusModeEnum.None,
    };

    public VBoxContainer SectionContent = new VBoxContainer();
    public GridContainer SectionGrid = new GridContainer();
    public HBoxContainer HeaderContainer = new HBoxContainer();
    private Texture _ExpandIcon = GD.Load<Texture>("res/icons/section_expand.png");
    private Texture _ContractIcon = GD.Load<Texture>("res/icons/section_contract.png");
    private StyleBoxFlat _PanelStyle;

    public override void _Ready()
    {
        this.Theme = GD.Load<Theme>("./res/theme/Section.tres");

        this.HeaderContainer.AddChild(this.SectionHeader);
        this.SectionContent.AddChild(this.SectionGrid);

        this._ALL.AddChild(this.HeaderContainer);
        this._ALL.AddChild(this.SectionContent);

        this.AddChild(this._ALL);

        this.SectionHeader.Connect("pressed", this, "_SectionClicked");
        
        this.ContentVisibility = false;
        this.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
        this.SectionContent.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        this.SectionGrid.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
    }

    public void _SectionClicked()
    { 
        this.ContentVisibility = !this.ContentVisibility; 
    }
}