using Godot;

public class Application : Node
{
    private MenuBar _Menu;
    private Control _GUI;
    private WindowDialog _PatternDesigner;

    public override void _Ready()
    {
        this._Menu = this.GetNode<MenuBar>("PichaGUI/WSVert/MenuBar");
        this._GUI = this.GetNode<Control>("PichaGUI");
        this._PatternDesigner = this.GetNode<WindowDialog>("PichaGUI/PatternDesigner");
        this._RegisterSignals();
    }

    private void _RegisterSignals()
    {
        this._Menu.ItemSelected += this.HandleMenu;
    }

    public void HandleMenu(MenuBarItem menu)
    {
        switch(menu.Action)
        {
            case "new_canvas":
            this.GetTree().CallGroup("gp_canvas_handler", "AddCanvas", new GenCanvas() { Name = "[unsaved]"});
                break;
            default:
                GD.PrintErr($"Unable to Parse MenuItem action \"{menu.Action}\".");
                this._PatternDesigner.PopupCentered();
                break;
        }
    }
}