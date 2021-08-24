using System.Collections.Generic;

using PichaLib;

using Godot;
using System;

/// <summary>
/// Special class for generating images and spritesheets using canvas data.
/// </summary>
public class ExportManager
{
    public Canvas Canvas;

    public Image GetSprite(int scale = 1)
    {
        var _output = new Image();

        var _s = this.Canvas.Size;

        _output.Create(_s.W, _s.H, false, Image.Format.Rgba8);
        _output.Fill(new Color(1f, 1f, 1f, 0f));

        var _frameNums = new int[this.Canvas.Layers.Count];

        for(int i = 0; i < _frameNums.Length; i++)
        {
            _frameNums[i] = this.Canvas.Layers[i].Frames.Count;
        }

        var _totalFrames = MathX.LCD(_frameNums);
        GD.Print(_totalFrames);

        foreach(Layer l in this.Canvas.Layers)
        {
            _output = _output.BlitLayer(ExportManager.GetLayerImages(l, this.Canvas.Size)[0], (0, 0).ToVector2());
        }

        return _output;
    }

    public static List<Image> GetLayerImages(Layer layer, (int w, int h) canvas)
    {
        var _output = new List<Image>();
        var _data = PFactory.ProcessLayer(layer);
        foreach(Chroma[,] val in _data)
        {
            var _img = val.ToImage();

            if(canvas == (0, 0))
            {
                _output.Add(_img);
            }
            else 
            {
                var _can = new Image();
                _can.Create(canvas.w, canvas.h, false, Image.Format.Rgba8);
                _can.Fill(new Color(1f, 1f, 1f, 0f));
                _can = _can.BlitLayer(_img, layer.Position.ToVector2());
                _output.Add(_can);
            }
        }
        return _output;
    }
}