using SysDraw = System.Drawing;

namespace PichaLib
{
    public partial class Chroma
    {
        public float R = 0f;
        public float G = 0f;
        public float B = 0f;
        public float A = 1f;
        public (float h, float s, float v, float a) HSV = (0f, 0f, 0f, 1f);
        public (float h, float s, float l, float a) HSL = (0f, 0f, 0f, 1f);
        public (int r, int g, int b, int a) IntRGBA = (0, 0, 0, 255);


        public (float r, float g, float b) RGB => (this.R, this.G, this.B); 
        public (float r, float g, float b, float a) RGBA => (this.R, this.G, this.B, this.A); 

        public Chroma()
        {
            this.R = 0f;
            this.G = 0f;
            this.B = 0f;
            this.A = 1f;
        }

        public Chroma((float r, float g, float b) col)
            { this._InitChroma(col.r, col.g, col.b, 1f); }
        public Chroma((float r, float g, float b, float a) col)
            { this._InitChroma(col.r, col.g, col.b, col.a); }
        public Chroma(float r, float g, float b, float a = 1f)
            { this._InitChroma(r, g, b, a); }

        private void _InitChroma(float r, float g, float b, float a = 1f)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;

            if(this.R < 0)  { this.R = 0f; }
            if(this.G < 0)  { this.G = 0f; }
            if(this.B < 0)  { this.B = 0f; }
            if(this.A < 0)  { this.A = 0f; }
            if(this.R > 1f) { this.R = 1f; }
            if(this.G > 1f) { this.G = 1f; }
            if(this.B > 1f) { this.B = 1f; }
            if(this.A > 1f) { this.A = 1f; }

            this.IntRGBA = ((int)(this.R * 255), (int)(this.G * 255), (int)(this.B * 255), (int)(this.A * 255));
            this.HSL = Chroma.RGBtoHSL(this.RGBA);
            this.HSV = Chroma.RGBtoHSV(this.RGBA);
        }

        public override string ToString()
            { return $"PichaLib.Chroma {{R: {this.R}, G: {this.G}, B: {this.B}, A: {this.A}}}"; }

        
        public SysDraw.Color ToColor()
        {
            var _c = this.IntRGBA;
            return SysDraw.Color.FromArgb(_c.a, _c.r, _c.g, _c.b);
        }
    }
}