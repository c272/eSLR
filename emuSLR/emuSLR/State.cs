using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    public static class State
    {
        //Enumeration for all possible states of the processor.
        public enum ProcessorStates
        {
            TERMINATED,
            NORMAL,
            ERROR,
            EXPECTING_CBP,
            HALTED
        }
    }
}
