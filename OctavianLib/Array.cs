using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OctavianLib
{
    public static class ArrayExtensions
    {

        public static T[] ToArray<T>(this SortedList<int, T> list) 
        {
            var _output = new T[list.Count];
            var _i = 0;

            foreach(int k in list.Keys) 
                { _output[_i] = list[k]; }

            return _output;
        }

        public static T[,] Fill<T>(this T[,] array, T value) 
        {
            var _h = array.GetLength(0);
            var _w = array.GetLength(1);

            T[,] _output = (T[,])array.Clone();

            for(int x = 0; x < _w; x++) 
            {
                for(int y = 0; y < _h; y++) { _output[y, x] = value; }
            }

            return _output;
        }

        public static T[,] ReplaceVal<T>(this T[,] array, T fromValue, T toValue) 
        {
            var _h = array.GetLength(0);
            var _w = array.GetLength(1);

            T[,] _output = (T[,])array.Clone();

            for(int x = 0; x < _w; x++) 
            {
                for(int y = 0; y < _h; y++) 
                {
                    if(EqualityComparer<T>.Default.Equals(array[y, x], fromValue))
                        { _output[y, x] = toValue; }
                }
            }

            return _output;
        }

        public static string ToPrintOut<T>(this T[] array)
        {
            string _output = "[";

            foreach(T t in array)
            {
                _output = $"{_output}{t.ToString()}, ";
            }

            return $"{_output}]";
        }
        
        public static string ToPrintOut<T>(this T[,] matrix) 
        {
            if(matrix is null)
                { throw new ArgumentNullException("matrix"); }

            var _matrix_height = matrix.GetLength(0);
            var _matrix_width = matrix.GetLength(1);

            string _output = "[";
            int _max = 0;

            for(int y = 0; y < _matrix_height; y++)
            {
                for(int x = 0; x < _matrix_width; x++)
                {
                    var _l = matrix[y, x].ToString().Length;
                    if(_l > _max)
                    {
                        _max = _l;
                    }
                }
            }
            
            for(int y = 0; y < _matrix_height; y++)
            {
                _output = $"{_output}\n\t";
                for(int x = 0; x < _matrix_width; x++)
                {
                    var _val = matrix[y, x].ToString();
                    _output = $"{_output}{_val},";
                    for(int r = 0; r < (_max - _val.Length + 1); r++)
                    {
                        _output = $"{_output} ";
                    }
                }
            }

            return $"{_output}\n]";
        }

        public static T[] GetRow<T>(this T[,] array, int row)
        {
            if(!typeof(T).IsPrimitive)
                { throw new InvalidOperationException("Not supported for managed types"); }
            if(array is null)
                { throw new ArgumentNullException("array"); }

            int _cols = array.GetUpperBound(1) + 1;
            T[] _output = new T[_cols];

            int _size;

            if(typeof(T) == typeof(bool))
                { _size = 1; }
            else if(typeof(T) == typeof(char))
                { _size = 2; }
            else
                {_size = Marshal.SizeOf<T>(); }

            Buffer.BlockCopy(array, row * _cols * _size, _output, 0, _cols * _size); 

            return _output;
        }

        public static T[,] MirrorY<T>(this T[,] array) 
        {
            int _w = array.GetLength(0);
            int _h = array.GetLength(1);

            T[,] _output = new T[_w * 2, _h];

            for(int x = 0; x < _w; x++) 
            {
                for(int y = 0; y < _h; y++) 
                {
                    _output[x, y] = array[x, y];
                    _output[(_w * 2) - 1 - x, y] = array[x, y]; 
                }
            }

            return _output;
        }

        public static T[,] MirrorX<T>(this T[,] array) 
        {
            int _w = array.GetLength(0);
            int _h = array.GetLength(1);

            T[,] _output = new T[_w, _h * 2];

            for(int x = 0; x < _w; x++) 
            {
                for(int y = 0; y < _h; y++) 
                {
                    _output[x, y] = array[x, y];
                    _output[x, (_h * 2) - 1 - y] = array[x, y]; 
                }
            }

            return _output;
        }

        public static T[,] MirrorXY<T>(this T[,] array) 
            { return array.MirrorX().MirrorY(); }

        public static int GetHeight<T>(this T[,] array) 
            { return array.GetLength(0); }

        public static int GetWidth<T>(this T[,] array) 
            { return array.GetLength(1); }


        public static T[] Flatten<T>(this T[,] array) 
        {
            var _h = array.GetHeight();
            var _w = array.GetWidth();


            T[] _output = new T[_w * _h];

            for(int y = 0; y < _h; y++) 
            {
                for(int x = 0; x < _w; x++) { _output[y * _w + x] = array[y, x]; }
            }

            return _output;
        }
        
        public static T[,] To2D<T>(this T[] array, int h, int w) 
        {
            T[,] _output = new T[h, w];

            for(int y = 0; y < h; y++) 
            {
                for(int x = 0; x < w; x++) { _output[y, x] = array[y * w + x]; }
            }

            return _output;
        }
    }
}