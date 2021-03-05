namespace PichaLib
{
    public partial class Chroma
    {
        public float R = 0f;
        public float G = 0f;
        public float B = 0f;
        public float A = 1f;

        public (float r, float g, float b) RGB => (this.R, this.G, this.B);
        public (float h, float s, float v) HSV => Chroma.RGBtoHSV(this.RGB);
        public (float h, float s, float l) HSL => Chroma.RGBtoHSL(this.RGB);

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

        public override string ToString()
            { return $"PichaLib.Chroma {{R: {this.R}, G: {this.G}, B: {this.B}, A: {this.A}}}"; }
    }
}