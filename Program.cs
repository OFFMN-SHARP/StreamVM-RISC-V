namespace StreamVM_RISC_V
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Outputer.VMOutputPipeName = "stdout"; 
                Outputer.VMInputPipeName = "stdin";
            }else
            {
                Outputer.VMOutputPipeName= args[0];
                Outputer.VMInputPipeName = args[1];
            }
            if (args.Length > 2)
            {
                string[] VMARG = args.Skip(2).ToArray();
            }
        }
    }
}
