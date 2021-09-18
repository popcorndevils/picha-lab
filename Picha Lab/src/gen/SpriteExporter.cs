using System;
using System.Collections.Generic;

using PichaLib;

using Godot;

/// <summary>
/// Special class for generating images and spritesheets using canvas data.
/// </summary>
public class SpriteExporter : Node
{
    [Signal] public delegate void ProgressChanged(int i, int total);
    [Signal] public delegate void StatusUpdate(string s);
    [Signal] public delegate void ProgressFinished();

    public void ExportLayers(ExportData args)
    {
        var _numFrames = MathX.LCD(args.Canvas.FrameCounts);
        var _framesPerSprite = (args.SplitFrames ? _numFrames : 1);

        var _spriteNumTotal = args.Columns * args.Rows * args.Sheets;

        for(int s = 0; s < args.Sheets; s++)
        {
            var _layerNames = new SortedSet<string>();
            var _sheetImage = new Dictionary<Layer, List<Image>>();

            // prepare output canvases
            foreach(Layer l in args.Canvas.Layers)
            {
                var _layerImage = new List<Image>();
                _sheetImage.Add(l, _layerImage);

                for(int _sf = 0; _sf < _framesPerSprite; _sf++)
                {
                    var _lWidth = (args.MapToCanvas ? args.Canvas.Size.W : l.Size.W);
                    var _lHeight = (args.MapToCanvas ? args.Canvas.Size.H : l.Size.H);

                    _layerImage.Add(new Image());
                    _layerImage[_sf].Create(
                        (args.Columns * (args.SplitFrames ? 1 : _numFrames) * _lWidth),
                        (args.Rows * _lHeight),
                        false, Image.Format.Rgba8
                    );
                }
            }

            // cycle through rows and columns, generating new sprites and placing them on their respective canvases
            for(int x = 0; x < args.Columns; x++)
            {
                for(int y = 0; y < args.Rows; y++)
                {
                    var _spriteNum = ((s * (args.Rows * args.Columns)) + (x * args.Rows) + y) + 1;
                    this.EmitSignal(nameof(SpriteExporter.ProgressChanged), _spriteNum, _spriteNumTotal);

                    var _layerFrames = this.GetSpriteLayers(args.Canvas, args.MapToCanvas ? args.Canvas.Size : (0, 0));

                    foreach((Layer L, List<Image> F) val in _layerFrames)
                    {
                        var _size = args.MapToCanvas ? args.Canvas.Size : val.L.Size;
                        var _y = y * _size.H;

                        for(int f = 0; f < _numFrames; f++)
                        {
                            var _x = args.SplitFrames ? x * _size.W : (x * _size.W * _numFrames) + (f * _size.W);
                            var _frame = val.F[f / (_numFrames / val.F.Count)];
                            var _i = (args.SplitFrames ? f : 0);
                            
                            _sheetImage[val.L][_i] = _sheetImage[val.L][_i].BlitLayer(_frame, (_x, _y));
                        }
                    }
                }
            }

            if(args.Scale != 1)
            {
                foreach(KeyValuePair<Layer, List<Image>> val in _sheetImage)
                {
                    var _w = args.Scale * (args.MapToCanvas ? args.Canvas.Size.W : val.Key.Size.W);
                    var _h = args.Scale * (args.MapToCanvas ? args.Canvas.Size.H : val.Key.Size.H);
                    this.EmitSignal(nameof(SpriteExporter.StatusUpdate), $"Resizing Sheet {val.Key.Name}_{s}.png");

                    foreach(Image i in val.Value)
                    {
                        i.Unlock();
                        i.Resize(_w * (args.SplitFrames ? 1 : _numFrames), _h, Image.Interpolation.Nearest);
                        i.Lock();
                    }
                }
            }

            foreach(KeyValuePair<Layer, List<Image>> val in _sheetImage)
            {
                var _name = val.Key.Name;

                if(_layerNames.Contains(_name))
                {
                    int _i = 0;
                    while(_layerNames.Contains($"{_name}({_i})")) { _i++; }
                    _name = $"{_name}({_i})";
                }

                _layerNames.Add(_name);

                for(int i = 0; i < val.Value.Count; i++)
                {
                    var _fileEnding = args.SplitFrames ? $"_{i}.png" : ".png";
                    var _fileName = $"{args.OutputPath}/{_name}_{s}{_fileEnding}";
                    val.Value[i].SavePng(_fileName);
                }
            }
        }

        // once complete, emit signal that process is over
        this.EmitSignal(nameof(SpriteExporter.ProgressFinished));
    }

    public void ExportSprite(ExportData args)
    {
        var _spriteFrameNum = MathX.LCD(args.Canvas.FrameCounts);

        var _spriteWidth = args.Canvas.Size.W * (args.SplitFrames ? 1 : _spriteFrameNum);
        var _spriteHeight = args.Canvas.Size.H;

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
                    this.EmitSignal(nameof(SpriteExporter.ProgressChanged), _spriteNum, _spriteNumTotal);

                    var _spriteFrames = this.GetSprite(args.Canvas);
                    var _y = y * args.Canvas.Size.H;

                    for(int f = 0; f < _spriteFrames.Count; f++)
                    {
                        var _x = args.SplitFrames ? x * args.Canvas.Size.W : 
                            (x * args.Canvas.Size.W * _spriteFrameNum) + (f * args.Canvas.Size.W);
                        var _index = args.SplitFrames ? f : 0;
                        _sheetImage[_index] = _sheetImage[_index].BlitLayer(_spriteFrames[f], _x, _y);
                    }
                }
            }

            if(args.Scale != 1)
            {
                this.EmitSignal(nameof(SpriteExporter.StatusUpdate), $"Resizing Sheet {args.SpriteName}_{s}.png");

                foreach(Image i in _sheetImage)
                {
                    i.Unlock();
                    i.Resize(args.Scale * _sheetWidth, args.Scale * _sheetHeight, Image.Interpolation.Nearest);
                    i.Lock();
                }
            }
            
            for(int i = 0; i < _sheetImage.Count; i++)
            {
                var _fileEnding = args.SplitFrames ? $"_{i}.png" : ".png";
                var _fileName = $"{args.OutputPath}/{args.SpriteName}_{s}{_fileEnding}";
                _sheetImage[i].SavePng(_fileName);
            }     
        }
        
        this.EmitSignal(nameof(SpriteExporter.ProgressFinished));
    }

    public Image GetSpriteFrame(Canvas canvas, int i, int scale = 1)
    {
        var _frame = this.GetSprite(canvas)[i];

        if(scale != 1)
        {
            _frame.Unlock();
            _frame.Resize(scale * _frame.GetWidth(), scale * _frame.GetHeight(), Image.Interpolation.Nearest);
            _frame.Lock();
        }

        return _frame;
    }

    public List<Image> GetSprite(Canvas canvas)
    {
        var _output = new List<Image>();
        var _frameNums = canvas.FrameCounts;
        var _totalFrames = MathX.LCD(_frameNums);

        var _layerFrames = this.GetSpriteLayers(canvas, canvas.Size);

        for(int i = 0; i < _totalFrames; i++)
        {
            var _spriteFrame = new Image();
            _spriteFrame.Create(canvas.Size.W, canvas.Size.H, false, Image.Format.Rgba8);

            foreach((Layer layer, List<Image> imgs) f in _layerFrames)
            {
                _spriteFrame = _spriteFrame.BlitLayer(f.imgs[i / (_totalFrames / f.imgs.Count)], (0, 0));
            }

            _output.Add(_spriteFrame);
        }

        return _output;
    }

    public List<(Layer, List<Image>)> GetSpriteLayers(Canvas canvas, (int w, int h) size)
    {
        var _output = new List<(Layer, List<Image>)>();

        foreach(Layer l in canvas.Layers)
        {
            var _frames = SpriteExporter.GetLayerImages(l, size);
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
    public Canvas Canvas;
    public int Columns;
    public int Rows;
    public int Sheets;
    public int Scale;
    public bool SplitFrames;
    public bool MapToCanvas;
    public bool ClipContent;
    public bool NoCopies;
    public string SpriteName;
    public string OutputPath;
}