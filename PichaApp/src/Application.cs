using Godot;

public class Application : Node
{
    private MenuBar _Menu;
    private Control _GUI;
    private GenCanvas _Sprite;

    public override void _Ready()
    {
        this._Menu = this.GetNode<MenuBar>("PichaGUI/WSVert/MenuBar");
        this._GUI = this.GetNode<Control>("PichaGUI");
        this._RegisterSignals();

        this._Sprite = new GenCanvas();

        Control _vp = this.GetNode<Control>("PichaGUI/WSVert/WorkArea/WorkSpace/Middle/SpriteView");

        _vp.AddChild(this._Sprite);

        this._Sprite.Generate();
        this._Sprite.Scale = new Vector2(20, 20);
        this._Sprite.Position = (this._Sprite.GetParent<Control>().RectSize / 2) - ((this._Sprite.Size / 2) * this._Sprite.Scale);

        this.GetTree().CallGroup("gp_canvas_gui", "LoadCanvas", this._Sprite);
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