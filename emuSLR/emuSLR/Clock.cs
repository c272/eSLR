using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    //Internal GameBoy clock, with event handlers contained in a class.
    public class Clock
    {
        public static void Tick(int ticks=1)
        {
            elapsed += ticks;
        }
        public static long elapsed=0;
    }
}
