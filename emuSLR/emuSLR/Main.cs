using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    public class Main
    {
        //Creating the registers.
        public Registers reg = new Registers();
        //Tick counter.
        public uint ticks = 0;

        
        ////////////////////////////////
        /// EMULATED ZILOG FUNCTIONS ///
        ////////////////////////////////

        //NOP (No Operation Performed)
        public void NOP()
        {
            //One NOP is 1 CPU tick.
            ticks += 1;
        }

        //...
        //todo
    }
}
