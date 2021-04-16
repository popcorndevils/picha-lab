using Godot;

public class RulerGrid : GridContainer
{

    public override void _Ready()
    {
        this.Columns = 2;
        this.MouseFilter = MouseFilterEnum.Ignore;

        this.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
        this.AddChildren(new CenterButton(), new HRuler(), new VRuler());

        this.AddConstantOverride("vseparation", 0);
        this.AddConstantOverride("hseparation", 0);
    }
}