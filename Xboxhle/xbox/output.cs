using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
namespace xboxhle
{
   static class output
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        public static void ScrollToBottom()
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            SendMessage(t.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
        }
       public static void print(int pc, int eip, string mnemonic) 
       {
           RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
           
           string str = "   " + (eip << 0).ToString("X") + "h";
           char padding = ' ';
           int eip_length = eip.ToString().Length;
           int length_adjust = eip_length + (25 - eip_length);

           t.AppendText(xboxhle.xbe.getSectionName(pc) + ":" + pc.ToString("X8"), Color.DarkSlateBlue, Color.Transparent);
           t.AppendText(str.PadRight(length_adjust, padding), Color.DarkSlateGray, Color.Transparent);
           t.AppendText(mnemonic + "\n", Color.MediumBlue, Color.Transparent);
           t.SelectionStart = t.Text.Length;
           ScrollToBottom();
       }

       public static void pXbe()
       {

       }


        public static void x86(int pc, int eip) {
            switch (eip << 0 & 0xFF)
            {
                case 0x01:
                    print(pc, eip, "add ecx, ebx");
                    break;
                case 0x05:
                    print(pc, eip, "add eax, " + ((eip & 0x0000FF00) >> 8).ToString("X") + "h");
                    break;
                case 0x20:
                    print(pc, eip, "and byte ptr [eax+esi*8]");
                    break;
                case 0x25: // and
                    if (((eip & 0x0000FF00) >> 8) == 0xFF)
                    {
                        print(pc, eip, "and eax, " + ((eip & 0x0000FF00) >> 8).ToString("X") + "h"); // 0xFF
                    }
                    else if (((eip & 0x00FF0000) >> 16) == 0xFF)
                    {
                        print(pc, eip, "and eax, " + ((eip & 0x00FF0000) >> 8).ToString("X") + "h"); // 0xFF00
                    }
                    else if (((eip & 0xFF000000) >> 24) == 0xFF)
                    {
                        print(pc, eip, "and eax, " + ((eip & 0xFF000000) >> 8).ToString("X") + "h"); // 0xFF0000
                    }
                    break;
                case 0x39: // new
                    if (((eip & 0x0000FF00) >> 8) == 0xCB) // new
                    {
                        print(pc, eip, "cmp ebx, ecx");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0x0B) // new
                    {
                        print(pc, eip, "cmp [ebx], ecx");
                    }
                    break; 
                case 0x48:
                    print(pc, eip, "dec eax");
                    break;
                case 0x50:
                    print(pc, eip, "push eax");
                    break;
                case 0x55:
                    print(pc, eip, "push ebp");
                    break;
                case 0x5D: // Pop ebp
                    print(pc, eip, "pop ebp");
                    break;
                case 0x66:
                    print(pc, eip, "OPSIZE");
                    break;
                case 0x69:
                    print(pc, eip, "imul ecx, " + ((eip & 0xFFFF0000) >> 16).ToString("X") + "h");
                    break;
                case 0x6A: // new 
                    print(pc, eip, "Push dword " + ((eip & 0x0000FF00) >> 8).ToString("X") + "h");
                    break;
                case 0x75:
                    print(pc, eip, "jnz loc_" + xboxhle.i386.reg32_pc.ToString("X") + "h");
                    break;
                case 0x7C: // new
                    if (((eip & 0x0000FF00) >> 8) == 0xF5) // new
                    {
                        print(pc, eip, "jl short loc_" + xboxhle.i386.reg32_pc.ToString("X") + "h");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0xFC) // new
                    {
                        print(pc, eip, "jl short loc_" + xboxhle.i386.reg32_pc.ToString("X") + "h");
                    }
                    break;
                case 0x81:
                    if (((eip & 0x0000FF00) >> 8) == 0xC1)
                    {
                        print(pc, eip, "add ecx, " + ((eip & 0xFFFF0000) >> 16).ToString("X") + "h");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0xC4)
                    {
                        print(pc, eip, "add esp, " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h");
                    }
                    break;
                case 0x83:
                    if (((eip & 0x0000FF00) >> 8) == 0xC0)
                    {
                        print(pc, eip, "add eax, " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0xC3) // new
                    {
                        print(pc, eip, "add ebx, " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0xC4)
                    {
                        print(pc, eip, "add esp, " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h");
                    }
                    break;
                case 0x88:
                    if (Convert.ToString((eip & 0xFFFFFFFF)).Length == Convert.ToString(0xFFFFFFFF).Length)
                    {
                        print(pc, eip, "mov [ebx + ecx * " + ((eip & 0x0000FF00) >> 8).ToString("X") + "h" + "], al");
                    }
                    else {
                        print(pc, eip, "mov [ebx + ecx * " + ((eip & 0x00000F00) >> 8).ToString("X") + "h" + " + " + ((eip & 0xFF000000) >> 24).ToString("X") + "h" + "], al");
                    }
                    break;
                case 0x89:
                    print(pc, eip, "mov ebp, esp");
                    break;
                case 0x8B:
                    if (((eip & 0x0000FF00) >> 8) == 0x0B) // new
                    {
                        print(pc, eip, "mov ecx, dword [ebx]");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0x1D)
                    {
                        print(pc, eip, "mov ebx, ds:" + ((eip & 0xFFFF0000) >> 16).ToString("X") + "h");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0x45)
                    {
                        print(pc, eip, "mov eax, dword ptr [ebp + " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h" + "]");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0x4D)
                    {
                        print(pc, eip, "mov ecx, dword ptr [ebp + " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h" + "]");
                    }
                    else if (((eip & 0x0000FF00) >> 8) == 0x5D)
                    {
                        print(pc, eip, "mov ebx, dword ptr [ebp + " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h" + "]");
                    }
                    break;
                case 0x90:
                    print(pc, eip, "nop");
                    break;
                case 0xA1:
                    print(pc, eip, "mov eax, ds:" + ((eip & 0x00FFFF00) >> 8).ToString("X") + "h");
                    break;
                case 0xA3:
                    print(pc, eip, "mov ds:" + ((eip & 0x00FFFF00) >> 8).ToString("X") + "h" + ", eax");
                    break;
                case 0xB0:
                    print(pc, eip, "mov al");
                    break;
                case 0xB8:
                    print(pc, eip, "mov eax");
                    break;
                case 0xB9: // new
                    print(pc, eip, "mov ecx, " + ((eip & 0xFFFFFF00) >> 8).ToString("X") + "h"); 
                    break;
                case 0xBA:
                    print(pc, eip, "mov dx");
                    break;
                case 0xBB: // new
                    print(pc, eip, "mov ebx, " + ((eip & 0xFFFFFF00) >> 8).ToString("X") + "h"); 
                    break;
                case 0xC1: 
                    if (((eip & 0x0000FF00) >> 8) == 0xE8) 
                    {
                        print(pc, eip, "shr eax, " + ((eip & 0x00FF0000) >> 16).ToString("X") + "h");
                    }
                    break;
                case 0xC2:
                    print(pc, eip, "retn " + ((eip & 0x00FF0000) >> 16).ToString("X"));
                    break;
                case 0xC6: // new
                    print(pc, eip, "mov byte ptr [ebx+" + ((eip & 0x00FF0000) >> 16).ToString("X") + "], " + ((eip & 0xFF000000) >> 24).ToString("X") + "h");
                    break;
                case 0xE8:
                    print(pc, eip, "call sub_" + xboxhle.i386.reg32_pc.ToString("X"));
                    break;
                case 0xE9:
                    print(pc, eip, "jmp loc_" + xboxhle.i386.reg32_pc.ToString("X"));
                    break;
                case 0xEB:
                    if ((((eip & 0x0000FF00) >> 8)) == 0x58)
                    {
                        print(pc, eip, "jmp short loc_" + xboxhle.i386.reg32_pc.ToString("X"));
                    }
                    break;
                case 0xED:
                    print(pc, eip, "in ax, dx");
                    break;
                case 0xEE:
                    print(pc, eip, "out dx, al");
                    break;
                case 0xFF:
                    if ((((eip & 0x0000FF00) >> 8)) == 0x15)
                    {
                        print(pc, eip, "call [" + xboxhle.tables.kernel_Thunk_table(xboxhle.i386.seg16_ds) + "]");
                    }
                    else if ((((eip & 0x0000FF00) >> 8)) == 0x35)
                    {
                        print(pc, eip, "push dword ptr ds:" + ((eip & 0xFFFF0000) >> 16).ToString("X") + "h");
                    }
                    break;
                default:
                    frmMain.emuIsRunning = false;
                    print(pc, eip, "Illegal Operation\r\nEmulation Halted");
                    break;
            }
        }

        public static void kernelThunk(int index) { 
        
        }
    }
}
