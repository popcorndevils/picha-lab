using Godot;

public class FileBrowse : FileDialog
{
    DialogMode DialogMode;

    public override void _Ready()
    {
        this.AddToGroup("gp_filebrowse");
        this.Connect("file_selected", this, "OnDialogConfirm");
        this.Connect("dir_selected", this, "OnDialogConfirm");
    }

    public void OpenDialog(DialogMode mode)
    {
        this.DialogMode = mode;

        switch(mode)
        {
            case DialogMode.NONE:
                break;
            case DialogMode.SAVE_CANVAS_AS_NEW:
                this.Mode = ModeEnum.SaveFile;
                this.Filters = new string[] {"*.plab ; Picha Lab"};
                this.WindowTitle = "Save Canvas";
                break;
            case DialogMode.OPEN_CANVAS:
                this.Mode = ModeEnum.OpenFile;
                this.Filters = new string[] {"*.plab ; Picha Lab"};
                this.WindowTitle = "Open Canvas";
                break;
        }

        this.PopupCentered();
    }

    public void OnDialogConfirm(string selected = null)
    {
        switch(this.DialogMode)
        {
            case DialogMode.NONE:
                break;
            case DialogMode.SAVE_CANVAS_AS_NEW:
                this.GetTree().CallGroup("gp_canvas_handler", "WriteFile", selected);
                break;
            case DialogMode.OPEN_CANVAS:
                this.GetTree().CallGroup("gp_canvas_handler", "OpenCanvas", selected);
                break;
        }
    }
}

public enum DialogMode
{
    NONE,
    SAVE_CANVAS_AS_NEW,
    OPEN_CANVAS,
    EXPORT_SPRITE,
}