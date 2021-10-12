using System;
using System.Collections.Generic;

namespace PichaLib
{
    public static partial class PFactory
    {
        private static List<Frame> _RunCycles(Layer l)
        {
            var _output = new List<Frame>();

            foreach(Frame _frame in l.Frames)
            {
                _output.Add(PFactory._RunCycles(_frame, l.Cycles));
            }
            return _output;
        }

        private static Frame _RunCycles(Frame f, List<Cycle> cycles)
        {
            var _output = new Frame() { Timing = f.Timing };
            _output.Data = f.Data.Copy();

            foreach(Cycle _cycle in cycles)
            {
                _output = PFactory.RunPolicies(_output, _cycle.Policies);
            }

            return _output;
        }
    }
}