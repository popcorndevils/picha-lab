namespace PichaLib
{
    public class Pixel
    {
        public static string NULL = "[NULL]";

        public string Name;
        public Chroma Color;
        public Chroma Paint;
        public bool RandomCol;
        public FadeDirection FadeDirection;
        public float BrightNoise;
        public float MinSaturation;
    }
}