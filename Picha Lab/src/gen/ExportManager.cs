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

    public List<Image> GetSprite(int scale = 1)
    {
        var _output = new List<Image>();

        var _s = this.Canvas.Size;

        var _frameNums = this.Canvas.FrameCount;
        var _totalFrames = MathX.LCD(_frameNums);

        var _layerFrames = new List<List<Image>>();

        foreach(Layer l in this.Canvas.Layers)
        {
            _layerFrames.Add(ExportManager.GetLayerImages(l, this.Canvas.Size));
        }

        for(int i = 0; i < _totalFrames; i++)
        {
            var _spriteFrame = new Image();
            _spriteFrame.Create(this.Canvas.Size.W, this.Canvas.Size.H, false, Image.Format.Rgba8);
            foreach(List<Image> f in _layerFrames)
            {
                _spriteFrame = _spriteFrame.BlitLayer(f[(int)(i / f.Count)], (0, 0));
            }
            _output.Add(_spriteFrame);
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