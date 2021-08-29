using Godot;

public class ExportDialog : WindowDialog
{
    public MarginContainer Contents;

    public override void _Ready()
    {
        this.AddToGroup("diag_export");

        this.Contents = this.GetNode<MarginContainer>("Contents");
    }

    public void Open(ExportManager export)
    {
        this.RectSize = this.Contents.RectSize;
        this.PopupCentered();
    }
}
