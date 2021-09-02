using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using PichaLib;

using Godot;

/// <summary>
/// Special class for generating images and spritesheets using canvas data.
/// </summary>
public class ExportManager : Node
{
    public Canvas Canvas;

    [Signal] public delegate void ProgressChanged(int i, int total);
    [Signal] public delegate void ProgressFinished();

    public ExportManager(Canvas canvas)
    {
        this.Canvas = canvas;
    }

    public void ExportSprite(ExportData args)
    {
        var _spriteFrameNum = MathX.LCD(this.Canvas.FrameCounts);

        var _spriteWidth = this.Canvas.Size.W * (args.SplitFrames ? 1: _spriteFrameNum);
        var _spriteHeight = this.Canvas.Size.H;

        var _sheetWidth = _spriteWidth * args.Columns;
        var _sheetHeight = _spriteHeight * args.Rows;

        var _spriteNumTotal = args.Columns * args.Rows * args.Sheets;

        for(int s = 0; s < args.Sheets; s++)
        {
            var _sheetImage = new Image();
            _sheetImage.Create(_sheetWidth, _sheetHeight, false, Image.Format.Rgba8);

            for(int x = 0; x < args.Columns; x++)
            {
                for(int y = 0; y < args.Rows; y++)
                {
                    var _spriteNum = (y * args.Columns) + (x * args.Rows) + (s * args.Rows * args.Columns);

                    if(!args.AsLayers)
                    {
                        var _sprites = this.GetSprite();

                        for(int i = 0; i < _sprites.Count; i++)
                        {
                            var _x = (x * this.Canvas.Size.W * _spriteFrameNum) + (i * this.Canvas.Size.W);
                            var _y = y * this.Canvas.Size.H;

                            _sheetImage = _sheetImage.BlitLayer(_sprites[i], _x, _y);
                        }

                        this.EmitSignal(nameof(ExportManager.ProgressChanged), _spriteNum, _spriteNumTotal);
                    }
                }
            }

            if(args.Scale != 1)
            {
                _sheetImage.Unlock();
                _sheetImage.Resize(args.Scale * _sheetWidth, args.Scale * _sheetHeight, Image.Interpolation.Nearest);
                _sheetImage.Lock();
            }

            _sheetImage.SavePng($"{args.OutputPath}/{args.SpriteName}_{s}.png");
        }
        
        this.EmitSignal(nameof(ExportManager.ProgressFinished));
    }

    public Image GetSpriteSheet(int cols = 1, int rows = 1, int scale = 1)
    {
        int _frames = MathX.LCD(this.Canvas.FrameCounts);
        int _w = this.Canvas.Size.W * _frames * cols;
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
                    var _x = (x * this.Canvas.Size.W * _frames) + (i * this.Canvas.Size.W);
                    var _y = y * this.Canvas.Size.H;

                    _output = _output.BlitLayer(_sprites[i], _x, _y);
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

        var _frameNums = this.Canvas.FrameCounts;
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

public class ExportData : Node
{
    public int Columns;
    public int Rows;
    public int Sheets;
    public int Scale;
    public bool SplitFrames;
    public bool AsLayers;
    public bool FullSizedLayers;
    public string SpriteName;
    public string OutputPath;
}