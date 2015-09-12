using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime;
namespace xboxhle
{
    class i386
    {
        public static int op1, op2, op3, op4;

        // stack registers
        public static Int16[] word = new Int16[0x100];
        public static Int32[] dword = new Int32[0x00FFFFFF];

        public static UInt32[] framebuffer = new UInt32[0x00FFFFFF];
        //https://retdec.com/decompilation-run/

        public static byte reg8_ah, reg8_al, reg8_bh, reg8_bl, reg8_ch, reg8_cl, reg8_dh, reg8_dl;
        public static Int16 reg16_ax, reg16_bx, reg16_cx, reg16_dx;
        public static Int32 reg32_eax, reg32_ebx, reg32_ecx, reg32_edx;
        public static Int32 reg32_eip, reg32_pc;

        // Flag Registers
        public static int carry_flag, parity_flag, aux_flag, zero_flag, sign_flag, trace, interrupt_flag, dir_flag, overflow_flag;

        // Segment Registers
        public static int seg16_cs, seg16_ds, seg16_es, seg16_ss; // Current Segment, Defined Segment, Extra Segment, Stack Segment.

        //Registers
        public static int reg16_si, reg16_di, reg16_bp, reg16_sp; // Source Index, Destination Index, Base Pointer, Stack Pointer registers
        public static Int32 reg32_esi, reg32_edi, reg32_ebp, reg32_esp;

        // Opcodes
        public static Int32 value, offset;
        public static int y;
        public static int x;
        public static int color;
        public static int call32_addr;

        public static int x86()
        {
            //return nonxdk_bgrflood_demo();
            return nonxdk_drawPixel_demo();
        }
        public static int nonxdk_image_demo()
        {
            return 0;
        }


        public static int nonxdk_bgrflood_demo() {
            op2 = ((xboxhle.xbe.mem[reg32_pc + 3] << 8) | (xboxhle.xbe.mem[reg32_pc + 2] << 0));
            op1 = ((xboxhle.xbe.mem[reg32_pc + 1] << 8) | (xboxhle.xbe.mem[reg32_pc + 0] << 0));

            reg32_eip = ((op2 << 16) | (op1 << 0));

                switch ((reg32_eip << 0) & 0xFF)
                {
                    case 0xBB: // mov ebx, address
                        //reg32_ebx = (Int32)(((reg32_eip & 0xFFFFFF00)));
                        
                        reg32_ebx = (Int32)((reg32_eip & 0x00FFFF00) >> 8);
                        reg32_pc += 5;
                        break;
                    case 0xB9: // mov ecx, address
                        reg32_ecx = (Int32)((reg32_eip & 0xFFFF0000) >> 8);
                        framebuffer = new UInt32[reg32_ecx & 0x0FFFFFFF << 0];
                        reg32_pc += 5;
                        break;
                    case 0xC6: // mov byte ptr [ebx + 1], 0xFFh;
                        framebuffer[reg32_ebx + 1] = (UInt32)((reg32_eip & 0xFF000000) >> 16);                        
                        reg32_pc += 4;
                        break;
                    case 0x83: // add ebx, 4;
                        if (((reg32_eip & 0x0000FF00) >> 8) == 0xC3) // new
                        {
                            reg32_ebx = reg32_ebx + ((reg32_eip & 0x00FF0000) >> 16);
                        }
                        reg32_pc += 3;
                        break;
                    case 0x39: // cmp ebx, ecx;
                        if (((reg32_eip & 0x0000FF00) >> 8) == 0xCB)
                        {
                            if (reg32_ebx == reg32_ecx)
                            {
                                // do something
                                frmMain.emuIsRunning = false;
                            }
                            reg32_pc += 2;
                        }
                        else if (((reg32_eip & 0x0000FF00) >> 8) == 0x0B)
                        {
                            if (reg32_ebx == reg32_ecx)
                            {
                                frmMain.emuIsRunning = false;
                            }
                            reg32_pc += 2;
                        }
                        break;
                    case 0x7C: // jl short loc_1200A
                        if (((reg32_eip & 0x0000FF00) >> 8) == 0xF5)
                        {
                            int loc = (Int32)(((reg32_eip & 0x0000FF00) >> 8) + 0xFFFFFF00);
                            reg32_pc = (reg32_pc + loc) + 2;

                            //reg32_pc += 2;
                        }
                        else if (((reg32_eip & 0x0000FF00) >> 8) == 0xFC)
                        {
                            int loc = (Int32)(((reg32_eip & 0x0000FF00) >> 8) + 0xFFFFFF00);
                            reg32_pc = (reg32_pc + loc) + 2;
                            //reg32_pc += 2;
                        }
                        break;
                    case 0x8B: // ebx, ds: 11000h
                        if (((reg32_eip & 0x0000FF00) >> 8) == 0x0B)
                        {
                            reg32_ecx = xboxhle.xbe.mem[reg32_ebx];
                            reg32_pc += 2;
                        }
                        else if (((reg32_eip & 0x0000FF00) >> 8) == 0x1D)
                        {
                            seg16_ds = (int)((reg32_eip & 0xFFFF0000) >> 16);
                            reg32_ebx = (xboxhle.xbe.mem[seg16_ds + 3] << 24 | xboxhle.xbe.mem[seg16_ds + 2] << 16 | xboxhle.xbe.mem[seg16_ds + 1] << 8 | xboxhle.xbe.mem[seg16_ds + 0] << 0); // get framebuffer location
                            reg32_pc += 6;
                        }
                        break;
                    case 0x81: // add ecx, 1388h
                        if (((reg32_eip & 0x0000FF00) >> 8) == 0xC1)
                        {
                            reg32_ecx = reg32_ecx + (int)((reg32_eip & 0xFFFF0000) >> 16);
                            reg32_pc += 6;
                        }
                        break;
                    case 0x6A: // PUSH 2
                        reg32_pc += 2;
                        break;
                    case 0xFF: // push dword ptr ds:11004h
                        if ((((reg32_eip & 0x0000FF00) >> 8)) == 0x15)
                        {
                            seg16_ds = xboxhle.xbe.mem[((reg32_eip & 0xFFFF0000) >> 16)];
                            dword[reg32_esp] = seg16_ds;
                            reg32_esp = reg32_esp + 1;
                            xboxhle.tables.kernel_Thunk_table(seg16_ds);
                            reg32_pc += 6;
                        }
                        break;
                    case 0xEB:
                        frmMain.emuIsRunning = false;
                        break;
                    default:
                        frmMain.emuIsRunning = false;
                        break;
                }

                return reg32_eip;
        }

        public static int nonxdk_drawPixel_demo()
        {

            op2 = ((xboxhle.xbe.mem[reg32_pc + 3] << 8) | (xboxhle.xbe.mem[reg32_pc + 2] >> 0));
            op1 = ((xboxhle.xbe.mem[reg32_pc + 1] << 8) | (xboxhle.xbe.mem[reg32_pc + 0] >> 0));

            reg32_eip = ((op2 << 16) | (op1 << 0));

            switch ((reg32_eip << 0) & 0xFF)
            {
                case 0xE9: // jump
                    if ((((reg32_eip & 0x0000FF00) >> 8)) == 0x58)
                    {
                        reg32_pc = (int)(0x1100 + ((reg32_eip & 0x0000FF00) >> 8) + 5);
                    }
                    break;
                case 0x55: // push
                    if ((((reg32_eip & 0x0000FF00) >> 8)) == 0x89)
                    {
                        //dword[reg32_esp] = xboxhle.xbe.mem[reg32_ebp];
                        //reg32_esp = reg32_esp + 1;
                        reg32_pc += 1;
                    }
                    break;
                case 0x89:
                    if ((((reg32_eip & 0x0000FF00) >> 8)) == 0xE5)
                    {
                        reg32_ebp = reg32_esp;
                        reg32_pc += 2;
                    }
                    break;
                case 0x8B:
                    if (((reg32_eip & 0x0000FF00) >> 8) == 0x1D)
                    {
                        seg16_ds = (int)((reg32_eip & 0xFFFF0000) >> 16);
                        reg32_ebx = (xboxhle.xbe.mem[seg16_ds + 3] << 24 | xboxhle.xbe.mem[seg16_ds + 2] << 16 | xboxhle.xbe.mem[seg16_ds + 1] << 8 | xboxhle.xbe.mem[seg16_ds + 0] << 0); // get framebuffer location
                        //framebuffer = new UInt32[reg32_ebx & 0x0FFFFFFF << 0];
                        reg32_pc += 6;
                    }
                    else if (((reg32_eip & 0x0000FF00) >> 8) == 0x45)
                    {
                        offset = ((reg32_eip & 0x00FF0000) >> 16);
                        color = reg32_eax = dword[/*reg32_ebp + offset*/ 0]; // needs checking moves color to eax
                        reg32_pc += 3;
                    }
                    else if (((reg32_eip & 0x0000FF00) >> 8) == 0x4D)
                    {

                        offset = ((reg32_eip & 0x00FF0000) >> 16);
                        reg32_ecx = dword[/*reg32_ebp + offset*/ 1]; // needs checking moves y pos to ecx
                        reg32_pc += 3;
                    }
                    else if (((reg32_eip & 0x0000FF00) >> 8) == 0x5D)
                    {
                        offset = ((reg32_eip & 0x00FF0000) >> 16);
                        reg32_ebx = dword[/*reg32_ebp + offset*/ 2]; // needs checking moves x pos to ebx
                        reg32_pc += 3;
                    }
                    break;
                case 0x69:
                    if ((((reg32_eip & 0x0000FF00) >> 8)) == 0xC9)
                    {
                        value = reg32_ecx * (int)((reg32_eip & 0x0FFFF000) >> 16);
                        reg32_ecx = value;
                        reg32_pc += 6;
                    }
                    break;
                case 0x01:
                    // add ecx, ebx
                    reg32_ecx = reg32_ecx + reg32_ebx;
                    reg32_pc += 2;
                    break;
                case 0x25:
                    if (((reg32_eip & 0x0000FF00) >> 8) == 0xFF)
                    {
                        value = (int)((reg32_eip & 0x0000FF00) >> 8); // 0xFF
                        reg32_eax = reg32_eax & value;
                        reg32_pc += 5;
                    }
                    else if (((reg32_eip & 0x00FF0000) >> 16) == 0xFF)
                    {
                        value = (int)((reg32_eip & 0x00FF0000) >> 8); // 0xFF00
                        reg32_eax = reg32_eax & value;
                        reg32_pc += 5;
                    }
                    else if (((reg32_eip & 0xFF000000) >> 24) == 0xFF)
                    {
                        value = (int)((reg32_eip & 0xFF000000) >> 8); // 0xFF0000
                        reg32_eax = reg32_eax & value;
                        reg32_pc += 5;
                    }
                    break;
                case 0x88:
                    if (((reg32_eip & 0xFF000000) >> 24) == 0x8B)
                    {
                        framebuffer[(reg32_ebx + reg32_ecx * ((reg32_eip & 0x0000FF00) >> 8)) & 0x00FFFFFF] = (UInt32)(reg32_eax << 0) & 0x000000FF;
                        reg32_pc += 3;
                    }
                    else if (((reg32_eip & 0xFF000000) >> 24) == 0x01)
                    {
                        framebuffer[(reg32_ebx + reg32_ecx * 4 + ((reg32_eip & 0xFF000000) >> 24)) & 0x00FFFFFF] = (UInt32)(reg32_eax << 8) & 0x0000FF00;
                        reg32_pc += 4;
                    }
                    else if (((reg32_eip & 0xFF000000) >> 24) == 0x02)
                    {
                        framebuffer[(reg32_ebx + reg32_ecx * 4 + ((reg32_eip & 0xFF000000) >> 24)) & 0x00FFFFFF] = (UInt32)(reg32_eax << 16) & 0x00FF0000;
                        reg32_pc += 4;
                    }
                    break;
                case 0xC1:
                    if (((reg32_eip & 0x0000FF00) >> 8) == 0xE8)
                    {
                        value = ((reg32_eip & 0x00FF0000) >> 16);
                        reg32_eax = reg32_eax >> value;
                        reg32_pc += 3;
                    }
                    break;
                case 0x5D:
                    reg32_pc += 1;
                    break;
                case 0xC2: // retn
                    reg32_esp = reg32_esp - 1;
                    reg32_pc = (dword[reg32_esp] + 5);
                    break;
                case 0xFF: // push dword ptr ds:value
                    if ((((reg32_eip & 0x0000FF00) >> 8)) == 0x35)
                    {
                        seg16_ds = xboxhle.xbe.mem[(Int32)(((reg32_eip & 0xFFFF0000) >> 16)) + 2] << 16 | xboxhle.xbe.mem[(Int32)(((reg32_eip & 0xFFFF0000) >> 16)) + 1] << 8 | xboxhle.xbe.mem[(Int32)(((reg32_eip & 0xFFFF0000) >> 16)) + 0] << 0;
                        dword[reg32_esp] = seg16_ds;
                        reg32_esp = reg32_esp + 1;
                        reg32_pc += 6;
                    }
                    break;
                case 0xE8:
                    // call sub
                    if ((((reg32_eip & 0x0000FF00) >> 8)) == 0x95)
                    {
                        dword[reg32_esp] = (reg32_pc & 0xFFFF);
                        reg32_esp = reg32_esp + 1;
                        call32_addr = (Int32)((reg32_eip & 0xFFFFFFFF00) >> 8);
                        reg32_pc = reg32_pc - Math.Abs(call32_addr) + 5;
                        call32_addr = reg32_pc;//
                    }
                    break;
                case 0x81:
                    // add esp, valh
                    value = ((reg32_eip & 0x00FF0000) >> 16);
                    reg32_esp = (reg32_esp + value);
                    reg32_pc += 6;
                    break;
                case 0xA1:
                    seg16_ds = xboxhle.xbe.mem[((reg32_eip & 0x00FFFF00) >> 8)];
                    reg32_eax = seg16_ds;
                    reg32_pc += 5;
                    //frmMain.emuIsRunning = false;
                    break;
                case 0x05:
                    value = ((reg32_eip & 0x0000FF00) >> 8);
                    reg32_eax = reg32_eax + value;
                    reg32_pc += 5;
                    break;
                case 0xA3:
                    // mov ds, eax
                    seg16_ds = ((reg32_eip & 0x00FFFF00) >> 8);
                    if (reg32_eax <= 0xFF)
                    {
                        seg16_ds = ((reg32_eip & 0x00FFFF00) >> 8);
                        xboxhle.xbe.mem[seg16_ds] = (byte)reg32_eax;
                    }
                    reg32_pc += 5;
                    break;
                case 0x48:
                    // dec eax
                    reg32_eax = reg32_eax - 1;
                    reg32_pc += 1;
                    break;
                case 0x75: // jnz
                    if (((reg32_eip & 0x0000FF00) >> 8) == 0xC7)
                    {
                        if (reg32_eax != 0)
                        {
                            reg32_esp = 0; // adjustment hack
                            offset = (Int32)(((reg32_eip & 0x0000FF00) >> 8) + 0xFFFFFF00);
                            reg32_pc = (reg32_pc + offset) + 2;
                        }
                        else
                        {
                            frmMain.emuIsRunning = false;
                            reg32_pc += 2;
                        }
                    }
                    break;
                case 0xEB:
                    if ((((reg32_eip & 0x0000FF00) >> 8)) == 0x58)
                    {
                        reg32_pc = (short)(0x1100 + ((reg32_eip & 0x0000FF00) >> 8) + 2);
                    }
                    break;
                default:
                    frmMain.emuIsRunning = false;
                    break;
            }
            return reg32_eip;
        }
    }
}
