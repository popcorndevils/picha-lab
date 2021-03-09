namespace PichaLib
{
    public class Pixel
    {
        public int ID;
        public string Name;
        public (byte r, byte g, byte b, byte a) Color;
        public bool RandomCol;
        public FadeDirection FadeDirection;
        public float BrightNoise;
        public float MinSaturation;
    }
}