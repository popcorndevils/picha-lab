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
    [Signal] public delegate void StatusUpdate(string s);
    [Signal] public delegate void ProgressFinished();

    public ExportManager(Canvas canvas)
    {
        this.Canvas = canvas;
    }

    public void ExportLayers(ExportData args)
    {

    }

    public void ExportSprite(ExportData args)
    {
        var _spriteFrameNum = MathX.LCD(this.Canvas.FrameCounts);

        var _spriteWidth = this.Canvas.Size.W * (args.SplitFrames ? 1 : _spriteFrameNum);
        var _spriteHeight = this.Canvas.Size.H;

        var _sheetWidth = _spriteWidth * args.Columns;
        var _sheetHeight = _spriteHeight * args.Rows;

        var _spriteNumTotal = args.Columns * args.Rows * args.Sheets;

        for(int s = 0; s < args.Sheets; s++)
        {
            var _sheetImage = new List<Image>();

            for(int _sf = 0; _sf < (args.SplitFrames ? _spriteFrameNum : 1); _sf++)
            {
                _sheetImage.Add(new Image());
                _sheetImage[_sf].Create(_sheetWidth, _sheetHeight, false, Image.Format.Rgba8);
            }

            for(int x = 0; x < args.Columns; x++)
            {
                for(int y = 0; y < args.Rows; y++)
                {
                    var _spriteNum = ((s * (args.Rows * args.Columns)) + (x * args.Rows) + y) + 1;

                    var _spriteFrames = this.GetSprite();

                    for(int f = 0; f < _spriteFrames.Count; f++)
                    {
                        if(args.SplitFrames)
                        {
                            var _x = x * this.Canvas.Size.W;
                            var _y = y * this.Canvas.Size.H;

                            _sheetImage[f] = _sheetImage[f].BlitLayer(_spriteFrames[f], _x, _y);
                        }
                        else 
                        {
                            var _x = (x * this.Canvas.Size.W * _spriteFrameNum) + (f * this.Canvas.Size.W);
                            var _y = y * this.Canvas.Size.H;

                            _sheetImage[0] = _sheetImage[0].BlitLayer(_spriteFrames[f], _x, _y);
                        }
                    }

                    this.EmitSignal(nameof(ExportManager.ProgressChanged), _spriteNum, _spriteNumTotal);
                }
            }

            if(args.Scale != 1)
            {
                this.EmitSignal(nameof(ExportManager.StatusUpdate), $"Resizing Sheet {args.SpriteName}_{s}.png");

                foreach(Image i in _sheetImage)
                {
                    i.Unlock();
                    i.Resize(args.Scale * _sheetWidth, args.Scale * _sheetHeight, Image.Interpolation.Nearest);
                    i.Lock();
                }
            }

            if(args.SplitFrames)
            {
                for(int i = 0; i < _sheetImage.Count; i++)
                {
                    _sheetImage[i].SavePng($"{args.OutputPath}/{args.SpriteName}_{s}_{i}.png");
                }
            }
            else
            {
                _sheetImage[0].SavePng($"{args.OutputPath}/{args.SpriteName}_{s}.png");
            }            
        }
        
        this.EmitSignal(nameof(ExportManager.ProgressFinished));
    }

    public List<Image> GetSprite()
    {
        var _output = new List<Image>();
        var _frameNums = this.Canvas.FrameCounts;
        var _totalFrames = MathX.LCD(_frameNums);

        var _layerFrames = this.GetSpriteLayers(this.Canvas.Size);

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

    public List<(Layer, List<Image>)> GetSpriteLayers((int w, int h) canvas)
    {
        var _output = new List<(Layer, List<Image>)>();

        foreach(Layer l in this.Canvas.Layers)
        {
            var _frames = ExportManager.GetLayerImages(l, canvas);
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
    public bool FullSizedLayers;
    public string SpriteName;
    public string OutputPath;
}