using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVM_RISC_V
{
    public class Registers
    {
        private ulong[] _x = new ulong[32];
        private uint _pc = 0x80000000;

        // 索引器访问通用寄存器
        public ulong this[int index]
        {
            get => index == 0 ? 0 : _x[index];
            set { if (index != 0) _x[index] = value; }
        }

        // PC 作为属性
        public uint PC
        {
            get => _pc;
            set => _pc = value;
        }

        // 静态单例（方便全局访问）
        public static Registers Instance { get; } = new Registers();
    }
}
