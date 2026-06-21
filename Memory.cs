using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVM_RISC_V
{
    public static class Memory
    {
        public static uint RamCount = 1024 * 1024;
        public static byte[] Ram = new byte[(int)RamCount];
        public static void ResetRam()
            =>Ram=new byte[RamCount];
        public static uint ReadUInt32(uint addr)
        {
            if (addr + 4 > Ram.Length) throw new Exception("Memory read out of range");
            return BitConverter.ToUInt32(Ram, (int)addr);
        }
        public static void WriteUInt32(uint addr, uint value)
        {
            if (addr + 4 > Ram.Length) throw new Exception("Memory write out of range");
            var bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, Ram, addr, 4);
        }

        // 类似实现 ReadByte, ReadUInt16, WriteByte, WriteUInt16, WriteUInt64 等
    }
}
