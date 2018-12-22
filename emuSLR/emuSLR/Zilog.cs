﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    //All emulated Zilog functions are here instead of in Main.cs, to keep me sane.
    public partial class Main
    {
        ////////////////////////////////
        /// EMULATED ZILOG FUNCTIONS ///
        ////////////////////////////////

        //NOP (No Operation Performed)
        public void NOP()
        {
            //One NOP is 1 CPU tick.
            Clock.Tick();
        }

        //LD BC, nn
        //16 bit immediate load into BC.
        public void LDBCnn(byte a, byte b)
        {
            reg.A = a;
            reg.B = b;
            Clock.Tick(2);
        }

        //LD (BC) A
        //Save register A to the location in memory pointed to by BC (16b).
        public void LDBCa()
        {
            ushort loc = Utils.ConcatBytes(reg.B, reg.C);
            SaveByte(reg.A, loc);
            Clock.Tick(3);
        }

        //INC BC
        //Increment the total 16 bit value of BC.
        public void INCBC()
        {
            ushort bc = (ushort)(Utils.ConcatBytes(reg.B, reg.C)+1);
            Utils.Splitx16(bc, ref reg.B, ref reg.C);
            Clock.Tick(4);
        }

        //INC B
        //Increment the value of register B.
        public void INCB()
        {
            reg.B++;
            Clock.Tick(1);
        }

        //DEC B
        //Decrement the value of register B.
        public void DECB()
        {
            reg.B--;
            Clock.Tick(1);
        }

        //LD B, n
        //Load 8 bit immediate value into B.
        public void LDBn(byte n)
        {
            reg.B = n;
            Clock.Tick(1);
        }

        //RLC A
        //Rotate all bits in A to the left, putting the MSB in the Carry flag,
        //and the value of the Carry flag into the LSB.
        public void RLCA()
        {
            byte MSB = (byte)((reg.A >> 8) & 0xFFu);
            bool LSB = reg.flags.CFlag;
            reg.flags.CFlag = Convert.ToBoolean(MSB);

            //Shifting bits, correcting last bit.
            reg.A = (byte)(reg.A << 1);
            BitArray bits = Utils.ByteToArray(reg.A);
            bits[0] = LSB;

            //Saving.
            reg.A = Utils.ConvertToByte(bits);
        }

    }
}