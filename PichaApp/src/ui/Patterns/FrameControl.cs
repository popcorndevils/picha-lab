using Godot;

using OctavianLib;

public class FrameControl : Node2D
{
    public Vector2 Size => this._Texture.Size * this.Scale;

    private PatternTexture _Texture = new PatternTexture() {
        MouseFilter = Control.MouseFilterEnum.Pass,
        SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill,
    };

    public override void _Ready()
    {
        this.Scale = new Vector2(20, 20);

        this._Texture.Texture = this.CreateTexture();

        var Tex = new PatternTexture() {
            MouseFilter = Control.MouseFilterEnum.Pass,
            Texture = this.CreateTexture(),
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill,
        };

        this.AddChild(Tex);
    }

    public ImageTexture CreateTexture(GenLayer l = null)
    {
        var _im = new Image();
        var _imTex = new ImageTexture();

        var _l = PDefaults.Frames[0];
        var _p = PDefaults.Pixels;

        _im.Create(_l.GetWidth(), _l.GetWidth(), false, Image.Format.Rgba8);
        _im.Lock();

        if(l != null)
        {

        }
        else
        {
            for(int _x = 0; _x < _l.GetWidth(); _x++)
            {
                for(int _y = 0; _y < _l.GetHeight(); _y++)
                {
                    var _cell = _l[_y, _x];
                    var _col = _p[_cell].Paint;
                    _im.SetPixel(_x, _y, _col.ToGodotColor());
                }
            }
        }

        _imTex.CreateFromImage(_im, 0);
        return _imTex;
    }
}