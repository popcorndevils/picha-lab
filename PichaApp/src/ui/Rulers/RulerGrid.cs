using Godot;

public class RulerGrid : GridContainer
{

    public CenterButton Center = new CenterButton();
    public HRuler Horiz = new HRuler();
    public VRuler Vert = new VRuler();

    public override void _Ready()
    {
        this.Columns = 2;
        this.MouseFilter = MouseFilterEnum.Ignore;

        this.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
        this.AddChildren(this.Center, this.Horiz, this.Vert);

        this.AddConstantOverride("vseparation", 0);
        this.AddConstantOverride("hseparation", 0);
    }
}