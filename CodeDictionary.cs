using System;
using System.Collections.Generic;

namespace StreamVM_RISC_V
{
    public static class CodeDictionary
    {
        private static readonly Dictionary<byte, Dictionary<(byte funct3, byte funct7), Action<uint>>> _table = new();
        public static void Add(byte opcode, byte funct3, byte funct7, Action<uint> handler)
        {
            if (!_table.TryGetValue(opcode, out var subDict))
            {
                subDict = new Dictionary<(byte, byte), Action<uint>>();
                _table[opcode] = subDict;
            }
            subDict[(funct3, funct7)] = handler;
        }

        public static void Execute(uint instruction)
        {
            byte opcode = (byte)(instruction & 0x7F);
            if (!_table.TryGetValue(opcode, out var subDict))
                throw new InvalidOperationException($"未知 opcode: 0x{opcode:X2}");

            byte funct3 = (byte)((instruction >> 12) & 0x7);
            byte funct7 = (byte)((instruction >> 25) & 0x7F);

            if (!subDict.TryGetValue((funct3, funct7), out var handler))
                throw new InvalidOperationException($"未实现的指令: opcode=0x{opcode:X2}, funct3=0x{funct3:X1}, funct7=0x{funct7:X2}");

            handler(instruction);
        }
    }
}