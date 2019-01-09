using System;
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
        public void LDBCnn(ushort n)
        {
            ushort BC = Loadx16(n);
            Utils.Splitx16(BC, ref reg.B, ref reg.C);
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

            //Incrementing ticks.
            Clock.Tick(6);
        }

        //LD (nn) SP
        //Save SP into a given 16bit address.
        public void LDnnSP(ushort n)
        {
            //Saving the x16 in memory at location n and n+1.
            byte a=0x0, b=0x0;
            Utils.Splitx16(reg.SP, ref a, ref b);
            Savex16(a, n);
            Clock.Tick(2);
        }

        //ADD HL, BC
        //Adds BC to HL and saves.
        public void ADDHLBC()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            ushort BC = Utils.ConcatBytes(reg.B, reg.C);
            HL += BC;

            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(4);
        }

        //LD A, (BC)
        //Loads data into A from address given by BC.
        public void LDAbc()
        {
            ushort BC = Utils.ConcatBytes(reg.B, reg.C);
            reg.A = LoadByte(BC);
            Clock.Tick(2);
        }

        //DEC BC
        //Decrements the 16-bit whole of BC and saves.
        public void DECBC()
        {
            ushort BC = Utils.ConcatBytes(reg.B, reg.C);
            BC--;
            Utils.Splitx16(BC, ref reg.B, ref reg.C);
            Clock.Tick(3);
        }

        //INC C
        //Increments register C.
        public void INCC()
        {
            reg.C++;
            Clock.Tick(1);
        }

        //DEC C
        //Decrements register C.
        public void DECC()
        {
            reg.C--;
            Clock.Tick(1);
        }

        //LD C, n
        //Load 8-bit immediate into C.
        public void LDCn(ushort n)
        {
            reg.C = LoadByte(n);
        }

        //RRC A
        //Rotate A right with carry flag.
        public void RRCA()
        {
            byte LSB = (byte)((reg.A >> 8) & 0xFFu);
            bool MSB = reg.flags.CFlag;
            reg.flags.CFlag = Convert.ToBoolean(LSB);

            //Shifting bits, correcting first bit.
            reg.A = (byte)(reg.A >> 1);
            BitArray bits = Utils.ByteToArray(reg.A);
            bits[7] = MSB;

            //Saving.
            reg.A = Utils.ConvertToByte(bits);

            //Incrementing ticks.
            Clock.Tick(6);
        }

        //STOP
        //Stop processor.
        public void STOP()
        {
            //Terminate processor, send termination signal.
            state = State.ProcessorStates.TERMINATED;
            Clock.Tick(1);
        }

        //LD DE, nn
        //16-bit immediate load into BC.
        public void LDDEnn(ushort n)
        {
            ushort DE = Loadx16(n);
            Utils.Splitx16(DE, ref reg.D, ref reg.E);
            Clock.Tick(2);
        }

        //LD (DE) A
        //Save A to address pointed to by DE.
        public void LDDEA()
        {
            ushort DE = Utils.ConcatBytes(reg.D, reg.E);
            SaveByte(reg.A, DE);
            Clock.Tick(2);
        }

        //INC DE
        //Increments registers DE as a whole.
        public void INCDE()
        {
            ushort DE = Utils.ConcatBytes(reg.D, reg.E);
            DE++;
            Utils.Splitx16(DE, ref reg.D, ref reg.E);
            Clock.Tick(2);
        }

        //INC D
        //Increments register D.
        public void INCD()
        {
            reg.D++;
            Clock.Tick(1);
        }

        //DEC D
        //Decrements register D.
        public void DECD()
        {
            reg.D--;
            Clock.Tick(1);
        }

        //LD D, n
        //Load 8-bit immediate into D.
        public void LDDn(ushort n)
        {
            reg.D = LoadByte(n);
            Clock.Tick(2);
        }

        //RL A
        //Rotate A left.
        public void RLA()
        {
            reg.A = (byte)(reg.A << 1);
            Clock.Tick(1);
        }

        //JR n
        //Relative jump (adds to current PC), unconditional, to n.
        public void JRn(byte n)
        {
            reg.PC += n;
            Clock.Tick(1);
        }

        //ADD HL, DE
        //Adds DE to HL as whole 16-bit nums.
        public void ADDHLDE()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            ushort DE = Utils.ConcatBytes(reg.D, reg.E);
            HL += DE;
            Clock.Tick(2);
        }

        //LD A, (DE)
        public void LDADE()
        {
            ushort DE = Utils.ConcatBytes(reg.D, reg.E);
            reg.A = LoadByte(DE);
            Clock.Tick(2);
        }

        //DEC DE
        //Decrement 16-bit DE.
        public void DECDE()
        {
            ushort DE = Utils.ConcatBytes(reg.D, reg.E);
            Utils.Splitx16(DE, ref reg.D, ref reg.E);
        }


    }
}
