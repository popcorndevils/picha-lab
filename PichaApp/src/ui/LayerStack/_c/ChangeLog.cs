using Godot;

public class ChangeLog : RichTextLabel
{
    public override void _Ready()
    {
        base._Ready();
        this.BbcodeEnabled = true;
        this.ScrollFollowing = true;
        this.AddToGroup("change_log");
    }

    public void LogItem(string log)
    {
        this.AppendBbcode(log);
    }
}