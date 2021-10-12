namespace PichaLib
{
    public partial class Chroma
    {
        public float R = 0f;
        public float G = 0f;
        public float B = 0f;
        public float A = 1f;

        public (float r, float g, float b, float a) RGBA => (this.R, this.G, this.B, this.A);
        public (float h, float s, float v, float a) HSV => Chroma.RGBtoHSV(this.RGBA);
        public (float h, float s, float l, float a) HSL => Chroma.RGBtoHSL(this.RGBA);
        public (int r, int g, int b, int a) IntRGBA {
            get {
                var _f_rgba = this.RGBA;
                (int r, int g, int b, int a) _output = (0, 0, 0, 0);

                if(_f_rgba.r < 0) { _f_rgba.r = 0; }
                if(_f_rgba.g < 0) { _f_rgba.g = 0; }
                if(_f_rgba.b < 0) { _f_rgba.b = 0; }
                if(_f_rgba.a < 0) { _f_rgba.a = 0; }

                _output.r = (int)(_f_rgba.r * 255);
                _output.g = (int)(_f_rgba.g * 255);
                _output.b = (int)(_f_rgba.b * 255);
                _output.a = (int)(_f_rgba.a * 255);
                return _output;
            }
        }

        public Chroma()
        {
            this.R = 0f;
            this.G = 0f;
            this.B = 0f;
            this.A = 1f;
        }

        public Chroma((float r, float g, float b) col)
        {
            this.R = col.r;
            this.G = col.g;
            this.B = col.b;
        }

        public Chroma((float r, float g, float b, float a) col)
        {
            this.R = col.r;
            this.G = col.g;
            this.B = col.b;
            this.A = col.a;
        }

        public Chroma(float r, float g, float b, float a = 1f)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public (float h, float s, float l, float a) ToHSL()
            { return Chroma.RGBtoHSL(this.RGBA); }

        public override string ToString()
            { return $"PichaLib.Chroma {{R: {this.R}, G: {this.G}, B: {this.B}, A: {this.A}}}"; }
    }
}