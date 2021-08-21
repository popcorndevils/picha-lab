using System.Collections.Generic;

using PichaLib;

using Godot;


/// <summary>
/// Special class for generating images and spritesheets using canvas data.
/// </summary>
public class SpriteImage
{
    public Canvas Canvas;
    public List<(Layer Layer, List<Image> Frames)> LayerImages = new List<(Layer, List<Image>)>();

    private int _Scale = 1;
    public int Scale {
        get => this._Scale;
        set {
            this._Scale = value;
            this._ResizeFrames(value);
        }
    }

    public SpriteImage(Canvas canvas)
    {
        this.Canvas = canvas;
        foreach(Layer l in this.Canvas.Layers)
        {
            this.LayerImages.Add((l, SpriteImage.GetLayerImages(l)));
        }
    }

    public static List<Image> GetLayerImages(Layer layer)
    {
        var _output = new List<Image>();
        var _data = PFactory.ProcessLayer(layer);
        foreach(Chroma[,] val in _data)
        {
            _output.Add(val.ToImage());
        }
        return _output;
    }

    private void _ResizeFrames(int scale)
    {
        foreach((Layer Layer, List<Image> Frames) data in this.LayerImages)
        {
            foreach(Image img in data.Frames)
            {
                img.Unlock();
                img.Resize(data.Layer.Size.W * scale, data.Layer.Size.H * scale, Image.Interpolation.Nearest);
                img.Lock();
            }
        }
    }
}