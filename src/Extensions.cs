using System.IO;
using SysDraw = System.Drawing;
using SkiaSharp;
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
        _output.CreateFromImage(array.ToImage(), 0);
        return _output;
    }

    public static ImageTexture ToGodotTex(this SKBitmap bmp)
    {
        var _output = new ImageTexture();
        _output.CreateFromImage(bmp.ToImage(), 0);
        return _output;
    }

    public static ImageTexture ToGodotTex(this SysDraw.Bitmap img)
    {
        var _output = new ImageTexture();
        _output.CreateFromImage(img.ToImage(), 0);
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
    
    // helpers for BlitLayer
    public static Image BlitLayer(this Image img, Image layer, (int x, int y) position)
        { return img.BlitLayer(layer, position.ToVector2()); }
    public static Image BlitLayer(this Image img, Image layer, int x, int y)
        { return img.BlitLayer(layer, (x, y).ToVector2()); }

    public static Image ToImage(this Chroma[,] layer)
    {
        var _output = new Image();
        _output.Create(layer.GetWidth(), layer.GetHeight(), false, Image.Format.Rgba8);
        _output.Lock();

        for(int x = 0; x < layer.GetWidth(); x++) 
        {
            for(int y = 0; y < layer.GetHeight(); y++) 
            {   
                var _c = layer[y, x].ToGodotColor();
                _output.SetPixel(x, y, _c);
            }
        }

        return _output;
    }

    public static Image ToImage(this SysDraw.Bitmap img)
    {
        var _output = new Image();
        using (MemoryStream ms = new MemoryStream()) {
            img.Save(ms, SysDraw.Imaging.ImageFormat.Png);
            ms.Position = 0;
            _output.LoadPngFromBuffer(ms.ToArray());
        }
        return _output;
    }

    public static Image ToImage(this SKBitmap bmp)
    {
        return bmp.ToSystem().ToImage();
    }

    public static SysDraw.Bitmap[] ToSystem(this SKBitmap[] bmps)
    {
        var output = new SysDraw.Bitmap[bmps.Length];
        for(int i = 0; i < bmps.Length; i++)
        {
            output[i] = bmps[i].ToSystem();
        }
        return output;
    }

    public static SysDraw.Bitmap ToSystem(this SKBitmap bmp)
    {
        var output = new SysDraw.Bitmap(bmp.Width, bmp.Height);            
        for(int x = 0; x < bmp.Width; x++)
        {
            for(int y = 0; y < bmp.Height; y++)
            {
                var c = bmp.GetPixel(x, y);
                output.SetPixel(x, y, c.ToSystem());
            }
        }
        return output;
    }

    public static SysDraw.Color ToSystem(this SKColor c)
    {
        return SysDraw.Color.FromArgb(c.Alpha, c.Red, c.Green, c.Blue);
    }
}

