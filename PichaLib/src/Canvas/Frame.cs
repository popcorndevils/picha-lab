using OctavianLib;

namespace PichaLib
{
    public class Frame
    {
        public string[,] Data;
        public int Timing = 1;

        public int GetWidth() { return this.Data.GetWidth(); }
        public int GetHeight() { return this.Data.GetHeight(); }
    }
}