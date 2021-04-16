using Godot;

public class VRuler : Button
{
    public override void _Ready()
    {
        this.RectMinSize = new Vector2(20, 0);
        this.SizeFlagsVertical = (int)SizeFlags.ExpandFill;
        this.FocusMode = FocusModeEnum.None;
        this.MouseDefaultCursorShape = CursorShape.Hsize;
        // this.Flat = true;
        // this.MouseFilter = MouseFilterEnum.Ignore;
    }
}