using Godot;

public class Application : Node
{
    private MenuBar _Menu;
    private CanvasView _Canvases;

    public override void _Ready()
    {
        OS.MinWindowSize = new Vector2(820, 550);

        this._Menu = this.GetNode<MenuBar>("PichaGUI/WSVert/MenuBar");
        this._Canvases = this.GetNode<WorkArea>("PichaGUI/WSVert/WorkArea").CanvasView;

        this._Menu.ItemSelected += this.HandleMenu;
    }
    
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("undo"))
        {
            this.GetTree().CallGroup("gp_canvas_handler", "UndoChange");
        }
    }

    public void HandleMenu(MenuBarItem menu)
    {
        switch(menu.Action)
        {
            case "new_canvas":
                this.GetTree().CallGroup("gp_canvas_handler", "AddCanvas", new GenCanvas());
                break;
            case "open_canvas":
                this.GetTree().CallGroup("gp_filebrowse", "OpenDialog", DialogMode.OPEN_CANVAS);
                break;
            case "save_canvas":
                this.GetTree().CallGroup("gp_canvas_handler", "Save");
                break;
            case "save_canvas_as":
                this.GetTree().CallGroup("gp_canvas_handler", "SaveCanvas");
                break;
            case "open_docs":
                this.GetTree().CallGroup("gp_helpdialog", "OpenHelp");
                break;
            default:
                GD.PrintErr($"Unable to Parse MenuItem action \"{menu.Action}\".");
                break;
        }
    }
}