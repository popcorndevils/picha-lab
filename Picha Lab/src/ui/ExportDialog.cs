using Godot;

public class ExportDialog : AcceptDialog
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddToGroup("diag_export");
    }

    public void Open(ExportManager export)
    {
        this.PopupCentered();
    }
}
