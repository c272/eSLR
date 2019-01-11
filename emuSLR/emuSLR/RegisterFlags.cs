using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    //Class containing all flags in the Sharp register.
    public class RegisterFlags
    {
        //Bit array, contains all flags.
        public BitArray bits = new BitArray(4);
        
        //Setting initial bits to zero.
        public RegisterFlags()
        {
            bits.SetAll(false);
        }

        //Public accessor for the "ZeroFlag".
        public bool ZeroFlag
        {
            get
            {
                //Grab the first index in bit array (ZF)
                return bits.Get(0);
            }
            set
            {
                //Set the first index, based on true/false.
                bits.Set(0, value);
            }
        }

        //Public accessor for the negative flag.
        public bool NFlag
        {
            get
            {
                //Grab the second index in bit array (OF)
                return bits.Get(1);
            }
            set
            {
                //Set the second index.
                bits.Set(1, value);
            }
        }

        //Public accessor for the HalfCarryFlag.
        public bool HCFlag
        {
            get
            {
                //Get the third index in bit array (HCF)
                return bits.Get(2);
            }
            set
            {
                //Set the third index.
                bits.Set(2, value);
            }
        }

        //Public accessor for the CarryFlag.
        public bool CFlag
        {
            get
            {
                //Get the fourth index in bit array (CF)
                return bits.Get(3);
            }
            set
            {
                //Set the fourth index.
                bits.Set(3, value);
            }
        }
    }
}
