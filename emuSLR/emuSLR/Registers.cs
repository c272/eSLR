using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    public class Registers
    {
        //Defining open CPU registers.
        public ushort PC; //Program counter.
        public ushort SP; //Stack pointer.
        public ushort A, B, C, D, E, H, L; //General purpose registers.
        public RegisterFlags flags = new RegisterFlags(); //Flags (ZF, OF, HCF, CF).
    }
}
