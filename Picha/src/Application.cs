using Godot;

using PichaLib;

using OctavianLib;

public class Application : Node
{
    private MenuBar _Menu;

    public override void _Ready()
    {
        this._Menu = this.FindNode("MenuBar") as MenuBar;
        this._RegisterSignals();

        var _test = PDefaults.Layer;
        GD.Print("****STAGE 00****");
        GD.Print(_test.Frames[0].ToPrintOut());
        var _testChanged = PFactory.ProcessLayer(_test);

        var _width = _testChanged[0].GetWidth();
        var _height = _testChanged[0].GetWidth();

        var _i = new Image();

        _i.Create(_width, _height, false, Image.Format.Rgba8);
        _i.Lock();

        for(int x = 0; x < _width; x++) 
        {
            for(int y = 0; y < _height; y++) 
            {
                // use transpose
                _i.SetPixel(x, y, _testChanged[0][y, x].ToGodotColor());
            }
        }

        var _tex = new ImageTexture();
        _tex.CreateFromImage(_i, 0);
        var _sprite = this.FindNode("TestSprite") as Sprite;
        _sprite.Texture = _tex;
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