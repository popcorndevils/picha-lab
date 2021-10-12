using System.Collections.Generic;

namespace PichaLib
{
    public static partial class PFactory
    {
        private static bool _NeighborIs(this string[,] array, int x, int y, string value) 
        {
            var _testVals = array._GatherNeighbors(x, y);
            foreach(string _v in _testVals) { if(_v == value) { return true; } }
            return false;
        }

        private static bool _NeighborIsNot(this string[,] array, int x, int y, string value) 
        {
            var _testVals = array._GatherNeighbors(x, y);
            foreach(string _v in _testVals) { if(_v != value) { return true; } }
            return false;
        }

        private static List<string> _GatherNeighbors(this string[,] array, int x, int y)
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
}