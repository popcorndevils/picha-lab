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

    public ExportManager(Canvas canvas)
    {
        this.Canvas = canvas;
    }

    public Image GetSpriteSheet(int cols = 1, int rows = 1, int scale = 1)
    {
        int _w = this.Canvas.Size.W * MathX.LCD(this.Canvas.FrameCount) * cols;
        int _h = this.Canvas.Size.H * rows;

        var _output = new Image();
        _output.Create(_w, _h, false, Image.Format.Rgba8);
        _output.Fill(new Color(1f, 1f, 1f, 0f));

        for(int x = 0; x < cols; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                var _sprites = this.GetSprite();
                for(int i = 0; i < _sprites.Count; i++)
                {
                    var _x = (i * this.Canvas.Size.W) + (x * _w);
                    var _y = y * this.Canvas.Size.H;

                    _output = _output.BlitLayer( _sprites[i], _x, _y);
                }
            }
        }

        if(scale != 1)
        {
            _output.Unlock();
            _output.Resize(scale * _w, scale * _h, Image.Interpolation.Nearest);
            _output.Lock();
        }

        return _output;
    }

    public List<Image> GetSprite()
    {
        var _output = new List<Image>();

        var _s = this.Canvas.Size;

        var _frameNums = this.Canvas.FrameCount;
        var _totalFrames = MathX.LCD(_frameNums);

        var _layerFrames = this.GetSpriteLayers();

        for(int i = 0; i < _totalFrames; i++)
        {
            var _spriteFrame = new Image();
            _spriteFrame.Create(this.Canvas.Size.W, this.Canvas.Size.H, false, Image.Format.Rgba8);

            foreach((Layer layer, List<Image> imgs) f in _layerFrames)
            {
                _spriteFrame = _spriteFrame.BlitLayer(f.imgs[i / (_totalFrames / f.imgs.Count)], (0, 0));
            }

            _output.Add(_spriteFrame);
        }

        return _output;
    }

    public List<(Layer, List<Image>)> GetSpriteLayers()
    {
        var _output = new List<(Layer, List<Image>)>();

        foreach(Layer l in this.Canvas.Layers)
        {
            var _frames = ExportManager.GetLayerImages(l, this.Canvas.Size);
            _output.Add((l, _frames));
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