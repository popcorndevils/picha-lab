using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

        _i.Unlock();
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

    public static void InsertChild(this Node self, int i, Node child)
    {
        self.AddChild(child);
        self.MoveChild(child, i);
    }

    public static void ClearChildren(this Node self)
    {
        foreach(Node n in self.GetChildren())
        {
            self.RemoveChild(n);
        }
    }

    public static (int x, int y) ToIntPair(this Vector2 self)
    {
        return ((int)self.x, (int)self.y);
    }

    public static Vector2 ToVector2(this (int x, int y) self)
    {
        return new Vector2(self.x, self.y);
    }

    public static T GetDefault<T>(this Godot.Collections.Dictionary self, object key, T val)
    {
        if(self.Contains(key))
        {
            return (T)self[key];
        }
        else
        {
            return val;
        }

    }


    public static Image BlitLayer(this Image img, Image layer, Vector2 position) {
        var _output = img;

        var _i = layer;
        var _i_p = position;
        var _i_s = img.GetSize();

        int _x_off = (int)_i_p.x + (int)(img.GetSize().x / 2);
        int _y_off = (int)_i_p.y + (int)(img.GetSize().y / 2);

        for(int x = 0; x < img.GetSize().x; x++) 
        {
            for(int y = 0; y < img.GetSize().y; y++) 
            {
                int _x = x - _x_off;
                int _y = y - _y_off;

                if(!(_x < 0 | _y < 0 | _x >= _i_s.x | _y >= _i_s.y)) 
                {
                    var _c = _i.GetPixel(_x, _y);
                    if(_c.a != 0f) 
                    { 
                        _output.SetPixel(x, y, _c); 
                    }
                }
            }
        }

        return _output;
    }

    public static Image ToImage(this Chroma[,] layer)
    {
        var _output = new Image();
        _output.Create(layer.GetWidth(), layer.GetHeight(), false, Image.Format.Rgba8);
        _output.Lock();

        for(int x = 0; x < layer.GetWidth(); x++) 
        {
            for(int y = 0; y < layer.GetHeight(); y++) 
            {   
                _output.SetPixel(x, y, layer[y, x].ToGodotColor());
            }
        }

        return _output;
    }
}

