using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    public partial class eSLR
    {
        //Creating the registers.
        public Registers reg = new Registers();
        //Tick counter.
        public uint ticks = 0;
        //Creating and setting processor state.
        State.ProcessorStates state = State.ProcessorStates.NORMAL;

        //Setting the two "SaveByte" and "LoadByte" delegates.
        //All locations in the Sharp's internal memory are 16 bit.
        public delegate void SaveByteDelegate(byte toSave, ushort location);
        public delegate void Savex16Delegate(ushort toSave, ushort location);
        public delegate byte LoadByteDelegate(ushort location);
        public delegate ushort Loadx16Delegate(ushort location);

        //Both LoadByte and SaveByte must be set by a memory management system before most CPU instructions can function.
        public SaveByteDelegate SaveByte;
        public LoadByteDelegate LoadByte;
        public Savex16Delegate Savex16;
        public Loadx16Delegate Loadx16;

        ////////////////////
        /// TESTING AREA ///
        ////////////////////
        
    }
}
