using Godot;
using PichaLib;

public class Application : Node
{
    private MenuBar _Menu;
    private Control _GUI;
    private GenSprite _Sprite;

    public override void _Ready()
    {
        this._Menu = this.GetNode<MenuBar>("PichaGUI/WSVert/MenuBar");
        this._GUI = this.GetNode<Control>("PichaGUI");
        this._RegisterSignals();

        this._Sprite = new GenSprite();

        this._GUI.AddChild(this._Sprite);
        this._Sprite.Scale = new Vector2(20, 20);
        this._Sprite.Position = new Vector2(180, 300);

        this._Sprite.Generate();
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
                this._Sprite.Generate();
                break;
            default:
                GD.PrintErr($"Unable to Parse MenuItem action \"{menu.Action}\".");
                break;
        }
    }
}