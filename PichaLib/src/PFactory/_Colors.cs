using System.Collections.Generic;
using SysDraw = System.Drawing;
using OctavianLib;

namespace PichaLib
{
    public static partial class PFactory
    {
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

                _dat.Sat = (float)PFactory._Random.RandfRange(_type.MinSaturation * _dat.HSL.s, _dat.HSL.s);

                _output.Pixels.Add(_type.Name, _dat);
            }

            return _output;
        }

        private static List<SysDraw.Bitmap> _GenColors(List<Frame> frames, Layer l)
        {
            var _output = new List<SysDraw.Bitmap>();
            var _colors = PFactory.GenerateColors(l);

            foreach(Frame f in frames)
            {
                var _product = PFactory.Generate(f, _colors);
                for(int i = 0; i < f.Timing; i++) { _output.Add(_product); }
            }

            return _output;
        }
    }
}