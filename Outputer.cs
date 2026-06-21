using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace StreamVM_RISC_V
{
    public static class Outputer
    {/*[mst(1)]//设置输出模式1=VGA  0=Txt
[s_size(200,300)]//设置屏幕大小，第一个是宽，第二个是长
[0,0,#FFFFFFF]//第一个0是左距离，第二个是顶距离
[0,0,‘h’]//同上，但是这个是模式0的输出，并且txt模式只能一个一个char的输出
[rst]//清屏
[ln,0->1,0->0,#FFFFFFF]//VGA批量绘画，和前面的一样，但是支持：px/ln/sq/sp
[dw]//刷新缓冲区
[stp]//已停机
[ero,"error:undefine"]//vm错误*/
        public static ModeSet DisplayMode = new();
        public static ScreenSize DisplaySize=new();
        public static CharSeter DisplayChar=new();
        public static StringSeter DisplayString=new();
        public static Draw DisplayDraw=new();
        public static DrawPX DisplayPX=new();
        public static TipError VMError=new();
        public static List<string> MessageSequence =new();
        public static void VirtualMachineBackendEscapeSequenceBuilder(int VirtualMachineBackendEscapeSequenceConstructorSignalScheduler)
        {
            StringBuilder DisplayMessage = new StringBuilder();
            switch (VirtualMachineBackendEscapeSequenceConstructorSignalScheduler)
            {
                case 0:
                    DisplayMessage.Append($"[mst({DisplayMode.Mode})]");
                    break;
                case 1:
                    DisplayMessage.Append($"[s_size({DisplaySize.Width},{DisplaySize.Height})]");
                    break;
                case 2:
                    DisplayMessage.Append($"[{DisplayChar.PositionLeft},{DisplayChar.PositionTop},{DisplayChar.Text},{DisplayChar.TextColor},{DisplayChar.BackgroundColor}]");
                    break;
                case 3:
                    DisplayMessage.Append($"[{DisplayString.PositionLeft},{DisplayString.PositionTop},{DisplayString.Text},{DisplayString.TextColor},{DisplayString.BackgroundColor}]");
                    break; 
                case 4:
                    DisplayMessage.Append("[rst]");
                    break;
                case 5:
                    string mode = "";
                    switch (DisplayDraw.Mode)
                    {
                        case 0:
                            mode = "LN";
                            break;
                        case 1:
                            mode = "SP";
                            break;
                        case 2:
                            mode = "SQ";
                            break;
                    }
                    DisplayMessage.Append($"[{mode.ToLower()},{DisplayDraw.PositionLeft}->{DisplayDraw.ToPositionLeft},{DisplayDraw.PositionTop}->{DisplayDraw.ToPositionTop},{DisplayDraw.Color}]");
                    break;
                case 6:
                    DisplayMessage.Append($"[px,{DisplayPX.PositionLeft},{DisplayPX.PositionTop},{DisplayPX.Color}]");
                    break;
                case 7:
                    DisplayMessage.Append("[dw]");
                    break;
                case 8:
                    DisplayMessage.Append("[stp]");
                    break;
                case 9:
                    DisplayMessage.Append($"[ero,\"{VMError.ErrorType}\",\"{VMError.Message}\"]");
                    break;
            }
            MessageSequence.Add(DisplayMessage.ToString());
        }
    }
    public class ModeSet
    {
        public int Mode {  get; set; }
    }
    public class ScreenSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
    public class CharSeter
    {
        public int PositionLeft { get; set; }
        public int PositionTop { get; set; }
        public char Text {  get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    public class StringSeter
    {
        public int PositionLeft { get; set; }
        public int PositionTop { get; set; }
        public string Text { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    public class Draw
    {
        public int PositionLeft { get; set; }
        public int PositionTop { get; set; }
        public int ToPositionLeft {  get; set; }
        public int ToPositionTop { get; set; }
        public int Mode { get; set; }//LN=0 SP=1 SQ=2
        public bool Fill { get; set; }
        public string Color { get; set; }
    }
    public class DrawPX
    {
        public int PositionLeft { get; set; }
        public int PositionTop { get; set; }
        public string Color { get; set; }
    }
    public class TipError
    {
        public string ErrorType { get; set; }
        public string Message { get; set; }
    }
}
