using System;
using System.Collections;
using System.Collections.Generic;
using OctavianLib;

namespace PichaLib
{
    public static class PFactory
    {
        private static Random _Random = new Random();

        public static List<Chroma[,]> Generate(Canvas c)
        {
            var _output = new List<Chroma[,]>();

            foreach(Layer _l in c.Layers)
            {
                var _canvasLayer = new Chroma[c.Size.H, c.Size.W];
                var _layerData = Generate(_l)[0];

                for(int y = 0; y < _l.Size.H; y++)
                {
                    for(int x = 0; x < _l.Size.W; x++)
                    {
                        _canvasLayer[y + _l.Position.Y, x + _l.Position.X] = _layerData[y, x].Copy();
                    }
                }
                _output.Add(_canvasLayer);
            }

            return _output;
        }

        public static List<Chroma[,]> Generate(Layer l)
        {
            return PFactory._GenColors(PFactory.RunCycles(l), l);
        }

        private static Chroma[,] Generate(Frame f, CellData data)
        {
            int _w = f.GetWidth();
            int _h = f.GetHeight();
            var _frameOut = new Chroma[_h, _w];

            for(int _y = 0; _y < _h; _y++)
            {
                for(int _x = 0; _x < _w; _x++)
                {
                    var _cell = f.Data[_y, _x];
                    var _cSet = data.Pixels[_cell];
                    if(_cell != Pixel.NULL)
                    {
                        float _grade = 0f;

                        switch(data.Pixels[_cell].FadeDirection)
                        {
                            case FadeDirection.NORTH:
                                _grade = (float)((_y + 1f) / _h);
                                break;
                            case FadeDirection.WEST:
                                _grade = (float)((_x + 1f) / _w);
                                break;
                            case FadeDirection.SOUTH:
                                _grade = 1f - (float)((_y + 1f) / _h);
                                break;
                            case FadeDirection.EAST:
                                _grade = 1f - (float)((_x + 1f) / _w);
                                break;
                            case FadeDirection.NONE:
                                _grade = 1f;
                                break;
                        }

                        float u_sin = (float)Math.Cos(_grade * Math.PI);
                        float _l = (float)(PFactory._Random.RandfRange(0f, data.Pixels[_cell].BrightNoise) * u_sin) + _cSet.HSL.l;

                        _frameOut[_y, _x] = Chroma.CreateFromHSL(_cSet.HSL.h, _cSet.Sat, _l, _cSet.HSL.a);
                    }
                    else
                    {
                        // is the cell is null just fill with transparent pixel.
                        _frameOut[_y, _x] = Chroma.CreateFromBytes(0, 0, 0, 0);
                    }
                }
            }

            if(data.MirrorX) { _frameOut = _frameOut.MirrorX(); }
            if(data.MirrorY) { _frameOut = _frameOut.MirrorY(); }

            return _frameOut;
        }

        private static List<Frame> RunCycles(Layer l)
        {
            var _output = new List<Frame>();

            foreach(Frame _frame in l.Frames)
            {
                _output.Add(PFactory.RunCycles(_frame, l.Cycles));
            }
            return _output;
        }

        private static Frame RunCycles(Frame f, List<Cycle> cycles)
        {
            var _output = new Frame() { Timing = f.Timing };
            _output.Data = f.Data.Copy();

            foreach(Cycle _cycle in cycles)
            {
                _output = PFactory.RunPolicies(_output, _cycle.Policies);
            }

            return _output;
        }

        private static CellData GenerateColors(Layer l)
        {
            var _output = new CellData() {
                MirrorX = l.MirrorX,
                MirrorY = l.MirrorY,
                Pixels = new Dictionary<string, PixelColors>(),
            };

            foreach(Pixel _type in l.Pixels.Values)
            {
                var _dat = new PixelColors() {
                    FadeDirection = l.Pixels[_type.Name].FadeDirection,
                    BrightNoise = l.Pixels[_type.Name].BrightNoise,
                };
                
                if(_type.RandomCol)
                {
                    _dat.RGB = new Chroma(
                        (float)PFactory._Random.NextDouble(),
                        (float)PFactory._Random.NextDouble(),
                        (float)PFactory._Random.NextDouble());
                }
                else
                    { _dat.RGB = _type.Color; }

                _dat.HSL = _dat.RGB.ToHSL();
                _dat.Sat = (float)PFactory._Random.RandfRange(_type.MinSaturation * _dat.HSL.s, _dat.HSL.s);

                _output.Pixels.Add(_type.Name, _dat);
            }

            return _output;
        }

        private static List<Chroma[,]> _GenColors(List<Frame> cells, Layer l)
        {
            var _output = new List<Chroma[,]>();
            var _colors = PFactory.GenerateColors(l);

            foreach(Frame f in cells)
            {
                var _product = PFactory.Generate(f, _colors);
                for(int i = 0; i < f.Timing; i++) { _output.Add(_product); }
            }

            return _output;
        }

        private static Frame RunPolicies(Frame cells, List<Policy> cycle)
        {
            (int W, int H) _size = (cells.GetWidth(), cells.GetHeight());

            var _output = cells.Copy();
            _output.Data = new string[_size.H, _size.W];

            for(int _x = 0; _x < _size.W; _x++)
            {
                for(int _y = 0; _y < _size.H; _y++)
                {
                    string _cType = cells.Data[_y, _x];
                    var _policies = cycle.FindAll(p => p.Input == _cType);
                    if(_policies.Count > 0)
                    { 
                        foreach(Policy _p in _policies)
                        {
                            _output.Data[_y, _x] = cells.RunPolicy(_p, _x, _y);
                        }
                    }
                    else
                        { _output.Data[_y, _x] = _cType; }
                }
            }
            
            return _output;
        }

        private static string RunPolicy(this Frame frame, Policy p, int x, int y)
        {
            if(PFactory._Random.NextDouble() <= p.Rate)
            {
                if(p.ConditionA != ConditionTarget.NONE && p.ConditionLogic != ConditionExpression.NONE)
                {
                    switch(p.ConditionA)
                    {
                        case ConditionTarget.NEIGHBOR:
                            if(p.ConditionLogic == ConditionExpression.IS &&
                               frame.Data.NeighborIs(x, y, p.ConditionB))
                                { return p.Output; }
                            else if(p.ConditionLogic == ConditionExpression.IS_NOT &&
                                    frame.Data.NeighborIsNot(x, y, p.ConditionB))
                                { return p.Output; }
                            else
                                { return frame.Data[y, x]; }
                        default:
                            return frame.Data[y, x];
                    }
                }
                else 
                    { return p.Output; }
            }
            else
                { return frame.Data[y, x]; }
        }

        private static bool NeighborIs(this string[,] array, int x, int y, string value) 
        {
            var _testVals = array.GatherNeighbors(x, y);
            foreach(string _v in _testVals) { if(_v == value) { return true; } }
            return false;
        }

        private static bool NeighborIsNot(this string[,] array, int x, int y, string value) 
        {
            var _testVals = array.GatherNeighbors(x, y);
            foreach(string _v in _testVals) { if(_v != value) { return true; } }
            return false;
        }

        private static List<string> GatherNeighbors(this string[,] array, int x, int y)
        {
            List<string> _output = new List<string>();
            List<(int xT, int yT)> _addresses = new List<(int xT, int yT)>() {
                (x - 1, y),
                (x + 1, y),
                (x, y - 1),
                (x, y + 1),
            };

            foreach((int xT, int yT) _a in _addresses)
            {
                try { _output.Add(array[_a.yT, _a.xT]); }
                catch {}
            }

            return _output;
        }
    }

    public struct CellData
    {
        public bool MirrorX;
        public bool MirrorY;
        public Dictionary<string, PixelColors> Pixels;
    }

    public struct PixelColors
    {
        public Chroma RGB;
        public (float h, float s, float l, float a) HSL;
        public float Sat;
        public FadeDirection FadeDirection;
        public float BrightNoise;
    }
}