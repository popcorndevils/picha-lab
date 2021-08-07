using Godot;

public class CenterButton : Button
{
    public override void _Ready()
    {
        this.FocusMode = FocusModeEnum.None;
        this.RectMinSize = new Vector2(20, 20);
        this.SizeFlagsHorizontal = (int)SizeFlags.Fill;
        this.SizeFlagsVertical = (int)SizeFlags.Fill;
        this.Icon = GD.Load<Texture>("res://res/icons/arrows-move.svg");
        this.MouseDefaultCursorShape = CursorShape.Move;
        this.Flat = true;
        this.HintTooltip = "Center Sprite in View.";
    }
}