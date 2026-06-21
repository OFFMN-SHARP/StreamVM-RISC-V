using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVM_RISC_V
{
    public static class CPUControl
    {
        public static bool Halted=true;
        public static void StartCPU()
        {
            Halted=false;
            while (!Halted)
            {
                uint inst = Memory.ReadUInt32(Registers.Instance.PC);
                Registers.Instance.PC += 4;
                CodeDictionary.Execute(inst);
            }
        }
    }
}
