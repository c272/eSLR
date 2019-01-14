using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    //All emulated Zilog functions are here instead of in Main.cs, to keep me sane.
    public partial class eSLR
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
            Utils.Splitx16(n, ref reg.B, ref reg.C);
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
            reg.flags.NFlag = false;
            Clock.Tick(4);
        }

        //INC B
        //Increment the value of register B.
        public void INCB()
        {
            reg.B++;
            reg.flags.NFlag = false;
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

            //Resetting H and N flags.
            reg.H = 0x0;
            reg.flags.NFlag = false;

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

        //LD A, (BC)
        //Loads data into A from address given by BC.
        public void LDABC()
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
            reg.flags.NFlag = false;
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
        public void LDCn(byte n)
        {
            reg.C = n;
            Clock.Tick(1);
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

            //Resetting H and N flags.
            reg.H = 0x0;
            reg.flags.NFlag = false;

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
            Utils.Splitx16(n, ref reg.D, ref reg.E);
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
            reg.flags.NFlag = false;
            Clock.Tick(2);
        }

        //INC D
        //Increments register D.
        public void INCD()
        {
            reg.D++;
            reg.flags.NFlag = false;
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
        public void LDDn(byte n)
        {
            reg.D = n;
            Clock.Tick(1);
        }

        //RL A
        //Rotate A left.
        public void RLA()
        {
            reg.A = (byte)(reg.A << 1);

            //Resetting H and N flags.
            reg.H = 0x0;
            reg.flags.NFlag = false;

            Clock.Tick(1);
        }

        //JR n
        //Relative jump (adds to current PC), unconditional, to n.
        public void JRn(byte n)
        {
            reg.PC += n;
            Clock.Tick(1);
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
            Clock.Tick(1);
        }

        //INC E
        //Increments register E.
        public void INCE()
        {
            reg.E++;
            reg.flags.NFlag = false;
            Clock.Tick(1);
        }

        //DEC E
        //Decrements register E.
        public void DECE()
        {
            reg.E--;
            Clock.Tick(1);
        }

        //LD E, n
        //Loads 8-bit immediate n into E.
        public void LDEn(byte n)
        {
            reg.E = n;
            Clock.Tick(1);
        }

        //RR A
        //Rotate A right.
        public void RRA()
        {
            reg.A = (byte)(reg.A >> 1);

            //Resetting H and N flags.
            reg.H = 0x0;
            reg.flags.NFlag = false;
            
            Clock.Tick(1);
        }
        
        //JR NZ, n
        //Relative jump by signed immediate if the Zero flag is off. (last result not zero)
        public void JRNZn(byte n)
        {
            if (reg.flags.ZeroFlag==false)
            {
                reg.PC += n;
            }
            Clock.Tick(2);
        }

        //LD HL, nn
        //16-bit immediate load into HL.
        public void LDHLnn(ushort n)
        {
            Utils.Splitx16(n, ref reg.H, ref reg.L);
            Clock.Tick(1);
        }

        //LDI (HL), A
        //Save A to address pointed by HL, increment HL.
        public void LDIHLA()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            SaveByte(reg.A, HL);
            HL++;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(3);
        }

        //INC HL
        //Increment 16-bit HL.
        public void INCHL()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            HL++;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            reg.flags.NFlag = false;
            Clock.Tick(2);
        }

        //INC H
        //Increment 8bit register H.
        public void INCH()
        {
            reg.H++;
            reg.flags.NFlag = false;
            Clock.Tick(1);
        }

        //DEC H
        public void DECH()
        {
            reg.H--;
            Clock.Tick(1);
        }

        //LD H, n
        //Load 8bit immediate into H.
        public void LDHn(byte n)
        {
            reg.H = n;
            Clock.Tick(1);
        }

        //DAA
        //Corrects addition results for BCD (instructions below, todo.)
        public void DAA()
        {
            //if the least significant four bits of A contain a non-BCD digit (i. e. it is greater than 9) or the H flag is set, 
            //then $06 is added to the register. Then the four most significant bits are checked. If this more significant digit 
            //also happens to be greater than 9 or the C flag is set, then $60 is added.
        }

        //JR Z, n
        //Relative jump by signed immediate if the last result was zero.
        public void JRZn(byte n)
        {
            if (reg.flags.ZeroFlag)
            {
                reg.PC += n;
            }
            Clock.Tick(2);
        }
        
        //ADD HL [x].
        //Adds a value to HL.
        public void ADDHL(ushort val)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            HL += val;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(2);
        }

        //LDI A, (HL)
        //Load A from address pointed to by HL, and then increment HL.
        public void LDIAHL()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            reg.A = LoadByte(HL);
            HL++;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(2);
        }

        //DEC HL
        //Decrement 16-bit HL.
        public void DECHL()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            HL--;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(1);
        }

        //INC L
        //Increment 8bit register L.
        public void INCL()
        {
            reg.L++;
            reg.flags.NFlag = false;
            Clock.Tick(1);
        }

        //DEC L
        //Decrement 8bit register L.
        public void DECL()
        {
            reg.L--;
            Clock.Tick(1);
        }

        //LD L, n
        //8bit immediate load into L.
        public void LDLn(byte n)
        {
            reg.L = n;
            Clock.Tick(2);
        }

        //CPL
        //Complement logical NOT on A. (Basically just XOR with 0xFF)
        public void CPL()
        {
            reg.A = (byte)(reg.A ^ 0xFF);
            Clock.Tick(1);
        }

        //JR NC, n
        //Relative jump by immediate if carry flag off.
        public void JRNCn(byte n)
        {
            if (reg.flags.CFlag==true)
            {
                reg.PC += n;
            }
            Clock.Tick(2);
        }

        //LD SP, nn
        //Load 16bit immediate into SP.
        public void LDSPnn(ushort n)
        {
            reg.SP = n;
            Clock.Tick(1);
        }

        //LDD (HL), A
        //Save A to address pointed by HL, decrement HL.
        public void LDDHLA()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            SaveByte(reg.A, HL);
            HL--;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(3);
        }

        //INC SP
        //Increment stack pointer.
        public void INCSP()
        {
            reg.SP++;
            reg.flags.NFlag = false;
            Clock.Tick(1);
        }

        //INC (HL)
        //Increment value pointed to by HL.
        public void INCatHL()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            byte n = LoadByte(HL);
            n++;
            SaveByte(n, HL);
            reg.flags.NFlag = false;
            Clock.Tick(2);
        }

        //DEC (HL)
        //Decrement value pointed to by HL.
        public void DECatHL()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            byte n = LoadByte(HL);
            n--;
            SaveByte(n, HL);
            Clock.Tick(2);
        }

        //LD (HL) n
        //Load 8bit immediate into address at HL.
        public void LDHLn(byte n)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            SaveByte(n, HL);
            Clock.Tick(2);
        }

        //SCF
        //Set Carry Flag.
        public void SCF()
        {
            //This operation also clears H and N.
            reg.flags.CFlag = true;
            reg.flags.NFlag = false;
            reg.H = 0x0;
            Clock.Tick(2);
        }

        //JR C, n
        //Relative jump to immediate if carry is set.
        public void JRCn(byte n)
        {
            if (reg.flags.CFlag)
            {
                reg.PC += n;
            }
            Clock.Tick(2);
        }

        //ADD HL SP
        //Add 16bit SP to HL.
        public void ADDHLSP()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            HL += reg.SP;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(2);
        }

        //LDD A, (HL)
        //Load A from address pointed to by HL, then decrement HL.
        public void LDDAHL()
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            reg.A = LoadByte(HL);
            HL--;
            Utils.Splitx16(HL, ref reg.H, ref reg.L);
            Clock.Tick(3);
        }

        //DEC SP
        //Decrement 16bit SP.
        public void DECSP()
        {
            reg.SP--;
            Clock.Tick(1);
        }

        //INC A
        //Increment A.
        public void INCA()
        {
            reg.A++;
            Clock.Tick(1);
        }

        //DEC A
        //Decrement A.
        public void DECA()
        {
            reg.A--;
            Clock.Tick(1);
        }

        //LD A, n
        //Load 8bit immediate into A.
        public void LDAn(byte n)
        {
            reg.A = n;
            Clock.Tick(1);
        }

        //CCF
        //Clear carry flag.
        public void CCF()
        {
            reg.flags.CFlag = false;
            Clock.Tick(1);
        }

        //LD X, X
        //Copy 8bit register to 8bit register.
        public void LDXX(ref byte reg, ref byte regtocopy)
        {
            reg = regtocopy;
            Clock.Tick(1);
        }

        //LD X, (HL)
        //Copy value pointed by HL to given register..
        public void LDXHL(ref byte register)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            register = LoadByte(HL);
            Clock.Tick(2);
        }

        //LD (HL) X
        //Copy given register to address pointed by HL.
        public void LDHLX(ref byte register)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            SaveByte(register, HL);
            Clock.Tick(1);
        }

        //HALT
        //Halts the processor.
        public void HALT()
        {
            state = State.ProcessorStates.HALTED;
            Clock.Tick(1);
        }

        //ADD X X
        //Add given register to other given register.
        public void ADDXX(ref byte reg, ref byte regtoadd)
        {
            reg += regtoadd;
            Clock.Tick(1);
        }

        //ADD X, HL
        //Adds value pointed to by HL to given register.
        public void ADDXHL(ref byte reg1)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            reg1 += LoadByte(HL);
            Clock.Tick(2);
        }

        //ADC X, X
        //Adds the two registers, and the carry flag, and sums to the first register.
        public void ADCXX(ref byte reg1, ref byte reg2)
        {
            //TEST THIS!
            reg1 += (byte)(reg2 + int.Parse(reg.flags.CFlag.ToString()));
            Clock.Tick(3);
        }

        //ADC X, (HL)
        //Adds value pointed to by HL to given register, as well as carry flag.
        public void ADCXHL(ref byte reg1)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            byte reg2 = LoadByte(HL);
            //TEST THIS!
            reg1 += (byte)(reg2 + int.Parse(reg.flags.CFlag.ToString()));
            Clock.Tick(3);
        }

        //SUB X, X
        //Subtracts one register from another.
        public void SUBXX(ref byte reg1, ref byte reg2)
        {
            reg1 -= reg2;
            Clock.Tick(1);
        }

        //SUB X, (HL)
        //Subtracts value pointed to by HL from given register.
        public void SUBXHL(ref byte reg1)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            byte reg2 = LoadByte(HL);
            reg1 -= reg2;
            Clock.Tick(2);
        }

        //SBC X, X
        //Subtract register and carry flag from another register.
        public void SBCXX(ref byte reg1, ref byte reg2)
        {
            //TEST THIS!
            reg1 -= (byte)(reg2 - int.Parse(reg.flags.CFlag.ToString()));
            Clock.Tick(2);
        }

        //SBC X, (HL)
        //Subtract HL and carry flag from given register.
        public void SCBXHL(ref byte reg1)
        {
            ushort HL = Utils.ConcatBytes(reg.H, reg.L);
            byte reg2 = LoadByte(HL);
            //TEST THIS!
            reg1 -= (byte)(reg2 - int.Parse(reg.flags.CFlag.ToString()));
            Clock.Tick(3);
        }

        //AND X
        //Compares given register to A using Logical AND.
        public void ANDX(ref byte reg1)
        {
            if (reg.A!=0x0 && reg1!=0x0)
            {
                reg.A = 1;
            } else
            {
                reg.A = 0;
            }
            Clock.Tick(2);
        }

        //
    }
}
