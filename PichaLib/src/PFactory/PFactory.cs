using System;
using SysColor = System.Drawing.Color;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Collections.Generic;
using OctavianLib;

namespace PichaLib
{
    public static partial class PFactory
    {
        private static Random _Random = new Random();

        public static List<Bitmap> Generate(Canvas c)
        {
            var _output = new List<Bitmap>();

            foreach(Layer _l in c.Layers)
            {
                var _canvasLayer = new Bitmap(c.Size.H, c.Size.W, PixelFormat.Canonical);
                var _layerData = Generate(_l)[0];
                _output.Add(_layerData);
            }

            return _output;
        }

        public static List<Bitmap> Generate(Layer l)
        {
            return PFactory._GenColors(PFactory._RunCycles(l), l);
        }

        private static Bitmap Generate(Frame f, CellData data)
        {
            int _w = f.GetWidth();
            int _h = f.GetHeight();
            var _color = new SysColor[_h, _w];

            for(int y = 0; y < _h; y++)
            {
                for(int x = 0; x < _w; x++)
                {
                    var _cell = f.Data[y, x];
                    var _cSet = data.Pixels[_cell];
                    if(_cell != Pixel.NULL)
                    {
                        float _grade = 0f;

                        switch(data.Pixels[_cell].FadeDirection)
                        {
                            case FadeDirection.NORTH:
                                _grade = (float)((y + 1f) / _h);
                                break;
                            case FadeDirection.WEST:
                                _grade = (float)((x + 1f) / _w);
                                break;
                            case FadeDirection.SOUTH:
                                _grade = 1f - (float)((y + 1f) / _h);
                                break;
                            case FadeDirection.EAST:
                                _grade = 1f - (float)((x + 1f) / _w);
                                break;
                            case FadeDirection.NONE:
                                _grade = 1f;
                                break;
                        }

                        float u_sin = (float)Math.Cos(_grade * Math.PI);
                        float _l = (float)(PFactory._Random.RandfRange(0f, data.Pixels[_cell].BrightNoise) * u_sin) + _cSet.HSL.l;

                        _color[y, x] = Chroma.CreateFromHSL(_cSet.HSL.h, _cSet.Sat, _l, _cSet.HSL.a).ToColor();
                    }
                    else
                    {
                        // is the cell is null just fill with transparent pixel.
                        _color[y, x] = Chroma.CreateFromBytes(0, 0, 0, 0).ToColor();
                    }
                }
            }

            if(data.MirrorX) { _color = _color.MirrorX(); }
            if(data.MirrorY) { _color = _color.MirrorY(); }

            var _output = new Bitmap(_color.GetWidth(), _color.GetHeight(), PixelFormat.Format32bppArgb);

            for(int x = 0; x < _color.GetWidth(); x++)
            {
                for(int y = 0; y < _color.GetHeight(); y++)
                {
                    _output.SetPixel(x, y, _color[y, x]);
                }
            }

            return _output;
        }
    }
}