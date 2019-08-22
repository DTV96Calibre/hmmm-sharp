using System;
using System.Diagnostics;
using DTV;
using static DTV.Instruction;

namespace Bench
{
    class Program
    {
        static void Main(string[] args)
        {
            DecodeTest();
            var hmmm = new Hmmm();
            hmmm.AddN(0, 1);
            hmmm.AddN(1, 255);
            hmmm.Add(0, 0, 1);
            hmmm.Mul(0, 0, 1);
            hmmm.Mul(0, 0, 1);
            hmmm.Write(0); // NOTE: Will always print 0 because r0 is always 0, even when you try to set it to something else. 
            hmmm.Halt();
        }
        
        public static void DecodeTest(){
            var h = new Hmmm();
            Debug.Assert(HALT == h.Decode((ushort)0b0000_0000_0000_0000));
            var nopInstructionDecode = h.Decode((ushort)0b0110_0000_0000_0000);
            Debug.Assert(NOP == nopInstructionDecode || COPY == nopInstructionDecode);
            Debug.Assert(READ == h.Decode((ushort)0b0000_0000_0000_0001));
            Debug.Assert(WRITE == h.Decode((ushort)0b0000_0000_0000_0010));
            Debug.Assert(SETN == h.Decode((ushort)0b0001_0000_0000_0000));
            Debug.Assert(LOADR == h.Decode((ushort)0b0100_0000_0000_0000));
            Debug.Assert(STORER == h.Decode((ushort)0b0100_0000_0000_0001));
            Debug.Assert(POPR == h.Decode((ushort)0b0100_0000_0000_0010));
            Debug.Assert(PUSHR == h.Decode((ushort)0b0100_0000_0000_0011));
            Debug.Assert(LOADN == h.Decode((ushort)0b0010_0000_0000_0011));
            Debug.Assert(STOREN == h.Decode((ushort)0b0011_0000_0000_0011));
            Debug.Assert(ADDN == h.Decode((ushort)0b0101_0000_0000_0011));
            Debug.Assert(COPY == h.Decode((ushort)0b0110_0000_0001_0000)); // 0b0110_0000_0000_0000 might be identified as a nop, so instead change one of the copy registers to something other than r0 (which is necessarily 0)
            Debug.Assert(NEG == h.Decode((ushort)0b0111_0000_0000_0011));
            Debug.Assert(ADD == h.Decode((ushort)0b0110_0000_0000_0011));
            Debug.Assert(SUB == h.Decode((ushort)0b0111_0000_0001_0011)); // 0b0111_XXXX_0000_YYYY might be identified as a neg, so change rY to soemthing other than r0
            Debug.Assert(MUL == h.Decode((ushort)0b1000_0000_0000_0000));
            Debug.Assert(DIV == h.Decode((ushort)0b1001_0000_0000_0011));
            Debug.Assert(MOD == h.Decode((ushort)0b1010_0000_0000_0011));
            Debug.Assert(JUMP == h.Decode((ushort)0b0000_0000_0000_0011));
            Debug.Assert(JUMPN == h.Decode((ushort)0b1011_0000_0000_0000));
            Debug.Assert(JEQZ == h.Decode((ushort)0b1100_0000_0000_0011));
            Debug.Assert(JNEZ == h.Decode((ushort)0b1101_0000_0000_0011));
            Debug.Assert(JGTZ == h.Decode((ushort)0b1110_0000_0000_0011));
            Debug.Assert(JLTZ == h.Decode((ushort)0b1111_0000_0000_0000));
            Debug.Assert(CALL == h.Decode((ushort)0b1011_0001_0000_0000)); // 0b1011_0000_####_#### might be identified as a jumpn, so change rX to something other than r0
            // jumpn and call are equivalent: in jumpn, the return address is not stored (specifically it goes to r0). CALL r0 N is equivalent to JUMPN N
        }
    }
}
