
namespace PichaLib
{
    public static class PFactory
    {
        public static (float r, float g, float b, float a)[,] ProcessLayer(Layer l)
        {
            var _out = new (float r, float g, float b, float a)[8, 8];
            return _out;
        }
    }
}