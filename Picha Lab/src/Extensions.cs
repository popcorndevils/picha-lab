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
        
        var _output = new Image();
        _output.Create(img.GetWidth(), img.GetHeight(), false, Image.Format.Rgba8);

        _output.Lock();
        layer.Lock();
        img.Lock();

        for(int x = 0; x < img.GetWidth(); x++)
        {
            for(int y = 0; y < img.GetHeight(); y++)
            {
                var _imgC = img.GetPixel(x, y);

                int _lX = (int)position.x;
                int _lX2 = _lX + layer.GetWidth();

                int _lY = (int)position.y;
                int _lY2 = _lY + layer.GetHeight();

                if(x >= _lX && x < _lX2 && y >= _lY && y < _lY2)
                {
                    var _layC = layer.GetPixel(x - _lX, y - _lY);
                    if(_layC.a != 0f)
                    {
                        _output.SetPixel(x, y, _layC);
                    }
                    else
                    {
                        _output.SetPixel(x, y, _imgC);
                    }
                }
                else
                {
                    _output.SetPixel(x, y, _imgC);
                }
            }
        }

        _output.Unlock();
        layer.Unlock();
        img.Unlock();

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

