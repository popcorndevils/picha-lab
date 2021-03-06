using Godot;

public class Application : Node
{
    private MenuBar _Menu;

    public override void _Ready()
    {
        this._Menu = this.FindNode("MenuBar") as MenuBar;
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
            default:
                GD.PrintErr($"Unable to Parse MenuItem action \"{menu.Action}\".");
                break;
        }
    }
}