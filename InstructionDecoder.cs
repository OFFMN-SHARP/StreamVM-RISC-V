using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVM_RISC_V
{
    public static class InstructionDecoder
    {
        public static uint FetchInstruction(byte[] mem,uint pc)
        {
            if (pc + 4 > mem.Length)
                throw new Exception("Instruction fetch out of bounds.");
            uint result = BitConverter.ToUInt32(mem, (int)pc);
            return result;
        }
        private const uint OPCODE_MASK = 0x7F;
        private const uint RD_MASK = 0x1F;
        private const uint FUNCT3_MASK = 0x7;
        private const uint RS1_MASK = 0x1F;
        private const uint RS2_MASK = 0x1F;
        private const uint FUNCT7_MASK = 0x7F;
        public static void DecodeInstruction(uint inst)
        {
            uint opcode = inst & OPCODE_MASK;
            uint rd = (inst >> 7) & RD_MASK;
            uint funct3 = (inst >> 12) & FUNCT3_MASK;
            uint rs1 = (inst >> 15) & RS1_MASK;
            uint rs2 = (inst >> 20) & RS2_MASK;
            uint funct7 = (inst >> 25) & FUNCT7_MASK;

        }
    }
}
