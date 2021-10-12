using System;
using System.Collections.Generic;

namespace PichaLib
{
    public static partial class PFactory
    {
        private static Frame RunPolicies(Frame cells, List<Policy> cycle)
        {
            (int W, int H) _size = (cells.GetWidth(), cells.GetHeight());

            var _output = cells.Copy();
            _output.Data = new string[_size.H, _size.W];

            for(int _x = 0; _x < _size.W; _x++)
            {
                for(int _y = 0; _y < _size.H; _y++)
                {
                    string _cType = cells.Data[_y, _x];
                    var _policies = cycle.FindAll(p => p.Input == _cType);
                    if(_policies.Count > 0)
                    { 
                        foreach(Policy _p in _policies)
                        {
                            _output.Data[_y, _x] = cells.RunPolicy(_p, _x, _y);
                        }
                    }
                    else
                        { _output.Data[_y, _x] = _cType; }
                }
            }
            
            return _output;
        }

        private static string RunPolicy(this Frame frame, Policy p, int x, int y)
        {
            if(PFactory._Random.NextDouble() <= p.Rate)
            {
                if(p.ConditionA != ConditionTarget.NONE && p.ConditionLogic != ConditionExpression.NONE)
                {
                    switch(p.ConditionA)
                    {
                        case ConditionTarget.NEIGHBOR:
                            if(p.ConditionLogic == ConditionExpression.IS &&
                               frame.Data._NeighborIs(x, y, p.ConditionB))
                                { return p.Output; }
                            else if(p.ConditionLogic == ConditionExpression.IS_NOT &&
                                    frame.Data._NeighborIsNot(x, y, p.ConditionB))
                                { return p.Output; }
                            else
                                { return frame.Data[y, x]; }
                        default:
                            return frame.Data[y, x];
                    }
                }
                else 
                    { return p.Output; }
            }
            else
                { return frame.Data[y, x]; }
        }
    }
}