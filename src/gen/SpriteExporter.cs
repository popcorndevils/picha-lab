using System;
using Bitmap = System.Drawing.Bitmap;
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
        var _numFrames = ExMath.LCD(args.Canvas.FrameCounts);
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
                    (int W, int H) _size;

                    if(args.MapToCanvas)
                        { _size = args.ClipContent ? args.Canvas.Size : args.Canvas.TrueSize; }
                    else
                        { _size = l.Size; }

                    _layerImage.Add(new Image());
                    _layerImage[_sf].Create(
                        (args.Columns * (args.SplitFrames ? 1 : _numFrames) * _size.W),
                        (args.Rows * _size.H),
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

                    (int W, int H) _lSize;
                    (int X, int Y) _offset = (0, 0);

                    if(args.MapToCanvas)
                    {
                        _lSize = args.ClipContent ? args.Canvas.Size : args.Canvas.TrueSize;
                        _offset.X = args.ClipContent ? 0 : Math.Abs(args.Canvas.Extents.MinX);
                        _offset.Y = args.ClipContent ? 0 : Math.Abs(args.Canvas.Extents.MinY);
                    }
                    else
                        { _lSize = (0, 0); }

                    var _layerFrames = this.GetSpriteLayers(args.Canvas, _lSize, _offset.X, _offset.Y);

                    foreach((Layer L, List<Image> F) val in _layerFrames)
                    {
                        (int W, int H) _size;
                        if(args.MapToCanvas)
                            { _size = args.ClipContent ? args.Canvas.Size : args.Canvas.TrueSize; }
                        else
                            { _size = val.L.Size; }

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
                    (int W, int H) _size;
                    if(args.MapToCanvas)
                        { _size = args.ClipContent ? args.Canvas.Size : args.Canvas.TrueSize; }
                    else
                        { _size = val.Key.Size; }
                    
                    var _w = args.Scale * _size.W;
                    var _h = args.Scale * _size.H;

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
        for(int s = 0; s < args.Sheets; s++)
        {
            if(args.SplitFrames)
            {

                var _frames = PFactory.GenerateFrameSheets(
                    args.Canvas, args.Columns, args.Rows, args.Scale, args.ClipContent);

                var _totalFrames = args.Sheets * _frames.Length;

                for(int f = 0; f < _frames.Length; f++)
                {
                    var _fI = f + (s * args.Sheets) + 1;
                    _frames[f].Save($"{args.OutputPath}/{args.SpriteName}_{f}.png");

                    this.EmitSignal(
                        nameof(SpriteExporter.ProgressChanged),
                        _fI, _totalFrames, $"Processing frame {_fI} of {args.Sheets * _frames.Length}...");
                }
            }
            else
            {
                var _sheet = PFactory.GenerateSpriteSheet(
                    args.Canvas, args.Columns, args.Rows, args.Scale, args.ClipContent);
                var _fileName = $"{args.OutputPath}/{args.SpriteName}_{s}.png";
                _sheet.Save(_fileName);
                this.EmitSignal(
                    nameof(SpriteExporter.ProgressChanged),
                    s + 1,
                    args.Sheets,
                    $"Processing sheet {s + 1} of {args.Sheets}...");
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

    public List<Image> GetSprite(Canvas canvas) { return this.GetSprite(canvas, canvas.Size, 0, 0); }
    public List<Image> GetSprite(Canvas canvas, (int W, int H) size, int offsetX = 0, int offsetY = 0)
    {
        var _output = new List<Image>();
        var _frameNums = canvas.FrameCounts;
        var _totalFrames = ExMath.LCD(_frameNums);

        var _layerFrames = this.GetSpriteLayers(canvas, size, offsetX, offsetY);

        for(int i = 0; i < _totalFrames; i++)
        {
            var _spriteFrame = new Image();
            _spriteFrame.Create(size.W, size.H, false, Image.Format.Rgba8);

            foreach((Layer layer, List<Image> imgs) f in _layerFrames)
            {
                // When the number of frame imgs is above the count, we get a divide by zero error
                // TODO no longer produces an error, but need to correctly export frames by timing
                _spriteFrame = _spriteFrame.BlitLayer(f.imgs[i / (_totalFrames / f.imgs.Count)], (0, 0));
            }

            _output.Add(_spriteFrame);
        }

        return _output;
    }

    public List<(Layer, List<Image>)> GetSpriteLayers(
        Canvas canvas, (int w, int h) size, int offsetX = 0, int offsetY = 0)
    {
        var _output = new List<(Layer, List<Image>)>();

        foreach(Layer l in canvas.Layers)
        {
            var _frames = SpriteExporter.GetLayerImages(l, size, offsetX, offsetY);
            _output.Add((l, _frames));
        }

        return _output;
    }

    public static List<Image> GetLayerImages(Layer layer, (int w, int h) canvas, int offsetX = 0, int offsetY = 0)
    {
        var _output = new List<Image>();
        var _data = layer.Generate();
        foreach(Bitmap val in _data)
        {
            var _img = val.ToImage();

            if(canvas == (0, 0))
            {
                _output.Add(_img);
            }
            else 
            {
                var _pos = new Vector2(layer.Position.X + offsetX, layer.Position.Y + offsetY);
                var _can = new Image();
                _can.Create(canvas.w, canvas.h, false, Image.Format.Rgba8);
                _can.Fill(new Color(1f, 1f, 1f, 0f));
                _can = _can.BlitLayer(_img, _pos);
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