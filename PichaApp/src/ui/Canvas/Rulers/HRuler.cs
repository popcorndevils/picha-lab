using Godot;

public class HRuler : Button
{
    public override void _Ready()
    {
        this.RectMinSize = new Vector2(0, 20);
        this.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        this.FocusMode = FocusModeEnum.None;
        this.MouseDefaultCursorShape = CursorShape.Vsize;
        // this.Flat = true;
        // this.MouseFilter = MouseFilterEnum.Ignore;
    }
}