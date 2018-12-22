using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    public partial class Main
    {
        //Creating the registers.
        public Registers reg = new Registers();
        //Tick counter.
        public uint ticks = 0;
        //Setting the two "SaveByte" and "LoadByte" delegates.
        //All locations in the Sharp's internal memory are 16 bit.
        public delegate void SaveByteDelegate(byte toSave, ushort location);
        public delegate byte LoadByteDelegate(ushort location);
        //Both LoadByte and SaveByte must be set by a memory management system before most CPU instructions can function.
        public SaveByteDelegate SaveByte;
        public SaveByteDelegate LoadByte;

        ////////////////////
        /// TESTING AREA ///
        ////////////////////
        
    }
}
