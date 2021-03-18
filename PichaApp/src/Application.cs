using Godot;

public class Application : Node
{
    private MenuBar _Menu;
    private Control _GUI;

    public override void _Ready()
    {
        this._Menu = this.GetNode<MenuBar>("PichaGUI/WSVert/MenuBar");
        this._GUI = this.GetNode<Control>("PichaGUI");
        this._RegisterSignals();
        this.GetTree().CallGroup("gp_canvas_handler", "LoadCanvas", new GenCanvas());
    }

    private void _RegisterSignals()
    {
        this._Menu.ItemSelected += this.HandleMenu;
    }

    public void HandleMenu(MenuBarItem menu)
    {
        switch(menu.Action)
        {
            case "gen_sprite":
                break;
            default:
                GD.PrintErr($"Unable to Parse MenuItem action \"{menu.Action}\".");
                break;
        }
    }
}