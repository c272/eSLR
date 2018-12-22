using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emuSLR
{
    //Class containing basic bitwise utility functions.
    public class Utils
    {
        //Combine 2 8-bit bytes into one 16 bit ushort.
        public static ushort ConcatBytes(byte a, byte b)
        {
            return (ushort)((ushort)(a << 4) | b);
        }

        //Splits a 16 bit ushort into two 8-bit bytes, taken by reference.
        public static void Splitx16(ushort u, ref byte a, ref byte b)
        {
            a = (byte)(u >> 8);
            b = (byte)(u);
        }

        //Gets the byte representations of the given ushort/byte.
        public static string ByteToString(byte a)
        {
            return Convert.ToString(a, 2).PadLeft(8, '0');
        }
        public static string x16ToString(ushort a)
        {
            return Convert.ToString(a, 2).PadLeft(16, '0');
        }

        //Prints the byte representations of the given ushort/byte.
        public static void PrintByte(byte a)
        {
            Console.WriteLine(ByteToString(a));
        }
        public static void Printx16(ushort a)
        {
            Console.WriteLine(x16ToString(a));
        }

        //Converts a BitArray into a single byte.
        public static byte ConvertToByte(BitArray bits, bool reverse=false)
        {
            //BitArrays sometimes need to be reversed to avoid LIFO errors.
            if (reverse)
            {
                bits = new BitArray(bits.Cast<bool>().Reverse().ToArray());
            }

            if (bits.Count != 8)
            {
                throw new ArgumentException("Illegal number of bits in BitArray.");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        //Returns a BitArray containing the bits inside the given byte.
        public static BitArray ByteToArray(byte a)
        {
            return new BitArray(new byte[] { a });
        }
    }
}
