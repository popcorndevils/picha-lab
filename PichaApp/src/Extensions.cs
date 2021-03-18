using Godot;

using PichaLib;

using OctavianLib;

public static class PichaExtensions
{
    public static Color ToGodotColor(this Chroma col)
        { return new Color(col.R, col.G, col.B, col.A); }

    public static Chroma ToChroma(this Color c)
        { return new Chroma(c.r, c.g, c.b, c.a); }

    public static ImageTexture ToGodotTex(this Chroma[,] array)
    {
        var _output = new ImageTexture();

        var _i = new Image();

        var _width = array.GetWidth();
        var _height = array.GetHeight();

        _i.Create(_width, _height, false, Image.Format.Rgba8);
        _i.Lock();

        for(int x = 0; x < _width; x++) 
        {
            for(int y = 0; y < _height; y++) 
            {
                // use transpose
                _i.SetPixel(x, y, array[y, x].ToGodotColor());
            }
        }

        _output.CreateFromImage(_i, 0);
        return _output;
    }

    public static void AddChildren(this Node self, params Node[] nodes)
    {
        foreach(Node n in nodes)
        {
            self.AddChild(n);
        }
    }
}

