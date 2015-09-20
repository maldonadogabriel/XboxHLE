/*
Copyright (C) 2015  Gabriel Maldonado

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or 
any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>

* Linkedin: https://www.linkedin.com/pub/gabriel-maldonado/63/457/866
* Github: https://github.com/Gabriel-Maldonado/XboxHLE
*/

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xboxhle.xbox.i386
{
    public class parse
    {
        public struct temp { 
            public static int addr, offset, value;
        }

        public struct stack { 
            public UInt32 dword, word;
        }

        public struct addr32 { 
            public static int op1, op2, op3, op4;
            public static Int32 eip, pc;
        }

        public struct reg32
        {
            public static Int32 eax, ebx, ecx, edx;          
        }
        
        public struct seg16 
        { 
            public static int cs, ds, es, ss; // left as type int for compatibility reasons.
        }

        public struct reg16 
        {
            public static Int16 ax, bx, cx, dx;
        }

        public struct reg8
        {
            public static byte ah, al, bh, bl, ch, cl, dh, dl;
        }

        public struct eIdx
        {
            public static Int32 esi, edi;        
        }

        public struct ePtr 
        {
            public static Int32 ebp, esp;
        }

        public struct eFlag 
        { 
            public static Int32 carry, parity, aux, zero, sign, trap, interrupt_enabled, direction, io_privledge, nested_task, resume, virtual_8086, alignment_check, virtual_interrupt, virtual_interrupt_pending, id;
        }

        public static stack[] arr = new stack[0x00FFFFFF];

        public static int translate()
        {
            //return nonxdk_bgrflood_demo();
            return nonxdk_drawPixel_demo();
        }

        public static int nonxdk_image_demo()
        {
            return 0;
        }

        public static int nonxdk_bgrflood_demo()
        {
            addr32.op2 = ((xboxhle.xbe.mem[addr32.pc + 3] << 8) | (xboxhle.xbe.mem[addr32.pc + 2] << 0));
            addr32.op1 = ((xboxhle.xbe.mem[addr32.pc + 1] << 8) | (xboxhle.xbe.mem[addr32.pc + 0] << 0));

            addr32.eip = ((addr32.op2 << 16) | (addr32.op1 << 0));

            switch ((addr32.eip << 0) & 0xFF)
            {
                case 0xBB: // mov ebx, address
                    reg32.ebx = (Int32)((addr32.eip & 0x00FFFF00) >> 8);
                    addr32.pc += 5;
                    break;
                case 0xB9: // mov ecx, address
                    reg32.ecx = (Int32)((addr32.eip & 0xFFFF0000) >> 8);
                    addr32.pc += 5;
                    break;
                case 0xC6: // mov byte ptr [ebx + 1], 0xFFh;
                    xboxhle.xbox.video.video.arr[reg32.ebx + 1].framebuffer = (UInt32)((addr32.eip & 0xFF000000) >> 16);
                    addr32.pc += 4;
                    break;
                case 0x83: // add ebx, 4;
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0xC3)
                    {
                        reg32.ebx = reg32.ebx + ((addr32.eip & 0x00FF0000) >> 16);
                    }
                    addr32.pc += 3;
                    break;
                case 0x39: // cmp ebx, ecx;
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0xCB)
                    {
                        if (reg32.ebx == reg32.ecx)
                        {
                            // do something
                            xbox.emu.emuIsRunning = false;
                        }
                        addr32.pc += 2;
                    }
                    else if (((addr32.eip & 0x0000FF00) >> 8) == 0x0B)
                    {
                        if (reg32.ebx == reg32.ecx)
                        {
                            xbox.emu.emuIsRunning = false;
                        }
                        addr32.pc += 2;
                    }
                    break;
                case 0x7C: // jl short loc_1200A
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0xF5)
                    {
                        int loc = (Int32)(((addr32.eip & 0x0000FF00) >> 8) + 0xFFFFFF00);
                        addr32.pc = (addr32.pc + loc) + 2;
                    }
                    else if (((addr32.eip & 0x0000FF00) >> 8) == 0xFC)
                    {
                        int loc = (Int32)(((addr32.eip & 0x0000FF00) >> 8) + 0xFFFFFF00);
                        addr32.pc = (addr32.pc + loc) + 2;
                    }
                    break;
                case 0x8B: // ebx, ds: 11000h
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0x0B)
                    {
                        reg32.ecx = xboxhle.xbe.mem[reg32.ebx];
                        addr32.pc += 2;
                    }
                    else if (((addr32.eip & 0x0000FF00) >> 8) == 0x1D)
                    {
                        seg16.ds = (UInt16)((addr32.eip & 0xFFFF0000) >> 16);
                        reg32.ebx = (xboxhle.xbe.mem[seg16.ds + 3] << 24 | xboxhle.xbe.mem[seg16.ds + 2] << 16 | xboxhle.xbe.mem[seg16.ds + 1] << 8 | xboxhle.xbe.mem[seg16.ds + 0] << 0); // get framebuffer location
                        addr32.pc += 6;
                    }
                    break;
                case 0x81: // add ecx, 1388h
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0xC1)
                    {
                        reg32.ecx = reg32.ecx + (int)((addr32.eip & 0xFFFF0000) >> 16);
                        addr32.pc += 6;
                    }
                    break;
                case 0x6A: // PUSH 2
                    addr32.pc += 2;
                    break;
                case 0xFF: // push dword ptr ds:11004h
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0x15)
                    {
                        seg16.ds = xboxhle.xbe.mem[((addr32.eip & 0xFFFF0000) >> 16)];
                        arr[ePtr.esp].dword = (UInt32)seg16.ds;
                        ePtr.esp = ePtr.esp + 1;
                        xboxhle.table.kernel_Thunk_table(seg16.ds);
                        addr32.pc += 6;
                    }
                    break;
                case 0xEB:
                    xbox.emu.emuIsRunning = false;
                    break;
                default:
                    xbox.emu.emuIsRunning = false;
                    break;
            }

            return addr32.eip;
        }

        public static int nonxdk_drawPixel_demo()
        {
            addr32.op2 = ((xboxhle.xbe.mem[addr32.pc + 3] << 8) | (xboxhle.xbe.mem[addr32.pc + 2] >> 0));
            addr32.op1 = ((xboxhle.xbe.mem[addr32.pc + 1] << 8) | (xboxhle.xbe.mem[addr32.pc + 0] >> 0));

            addr32.eip = ((addr32.op2 << 16) | (addr32.op1 << 0));

            switch ((addr32.eip << 0) & 0xFF)
            {
                case 0xE9: // jump
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0x58)
                    {
                        addr32.pc = (int)(0x1100 + ((addr32.eip & 0x0000FF00) >> 8) + 5);
                    }
                    break;
                case 0x55: // push
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0x89)
                    {
                        //dword[esp] = xboxhle.xbe.mem[ebp];
                        //esp = esp + 1;
                        addr32.pc += 1;
                    }
                    break;
                case 0x89: // mov ebp, esp
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0xE5)
                    {
                        ePtr.ebp = ePtr.esp;
                        addr32.pc += 2;
                    }
                    break;
                case 0x8B:
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0x1D) // mov ebx, ds:1105
                    {
                        // Oddly, this sample retrieves a 32 bit-size pixel from a 16 bit segment, I needed to adjust the segments' type to int for this to work.
                        seg16.ds = (int)((addr32.eip & 0xFFFF0000) >> 16);
                        reg32.ebx = (xboxhle.xbe.mem[seg16.ds + 3] << 24 | xboxhle.xbe.mem[seg16.ds + 2] << 16 | xboxhle.xbe.mem[seg16.ds + 1] << 8 | xboxhle.xbe.mem[seg16.ds + 0] << 0); // get framebuffer location
                        addr32.pc += 6;
                    }
                    else if (((addr32.eip & 0x0000FF00) >> 8) == 0x45) // mov eax, [ebp+8h]
                    {
                        temp.offset = ((addr32.eip & 0x00FF0000) >> 16);
                        reg32.eax = (int)arr[/*ebp + temp.offset*/ 0].dword; // Adjusted needs checking moves color to eax
                        addr32.pc += 3;
                    }
                    else if (((addr32.eip & 0x0000FF00) >> 8) == 0x4D) // mov ecx, [ebp+Ch]
                    {

                        temp.offset = ((addr32.eip & 0x00FF0000) >> 16);
                        reg32.ecx = (int)arr[/*ebp + temp.offset*/ 1].dword; // Adjusted needs checking moves y pos to ecx
                        addr32.pc += 3;
                    }
                    else if (((addr32.eip & 0x0000FF00) >> 8) == 0x5D) // mov ebx, [ebp+8h]
                    {
                        temp.offset = ((addr32.eip & 0x00FF0000) >> 16);
                        reg32.ebx = (int)arr[/*ebp + offset*/ 2].dword; // Adjusted needs checking moves x pos to ebx
                        addr32.pc += 3;
                    }
                    break;
                case 0x69:
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0xC9)
                    {
                        temp.value = reg32.ecx * (int)((addr32.eip & 0x0FFFF000) >> 16);
                        reg32.ecx = temp.value;
                        addr32.pc += 6;
                    }
                    break;
                case 0x01:
                    // add ecx, ebx
                    reg32.ecx = reg32.ecx + reg32.ebx;
                    addr32.pc += 2;
                    break;
                case 0x25:
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0xFF)
                    {
                        temp.value = (int)((addr32.eip & 0x0000FF00) >> 8); // 0xFF
                        reg32.eax = reg32.eax & temp.value;
                        addr32.pc += 5;
                    }
                    else if (((addr32.eip & 0x00FF0000) >> 16) == 0xFF)
                    {
                        temp.value = (int)((addr32.eip & 0x00FF0000) >> 8); // 0xFF00
                        reg32.eax = reg32.eax & temp.value;
                        addr32.pc += 5;
                    }
                    else if (((addr32.eip & 0xFF000000) >> 24) == 0xFF)
                    {
                        temp.value = (int)((addr32.eip & 0xFF000000) >> 8); // 0xFF0000
                        reg32.eax = reg32.eax & temp.value;
                        addr32.pc += 5;
                    }
                    break;
                case 0x88:
                    if (((addr32.eip & 0xFF000000) >> 24) == 0x8B)
                    {
                        xboxhle.xbox.video.video.arr[(reg32.ebx + reg32.ecx * ((addr32.eip & 0x00000F00) >> 8)) & 0x00FFFFFF].framebuffer = (UInt32)(reg32.eax << 0) & 0x000000FF;
                        addr32.pc += 3;
                    }
                    else if (((addr32.eip & 0xFF000000) >> 24) == 0x01)
                    {
                        xboxhle.xbox.video.video.arr[(reg32.ebx + reg32.ecx * ((addr32.eip & 0x00000F00) >> 8) + ((addr32.eip & 0xFF000000) >> 24)) & 0x00FFFFFF].framebuffer = (UInt32)(reg32.eax << 8) & 0x0000FF00;
                        addr32.pc += 4;
                    }
                    else if (((addr32.eip & 0xFF000000) >> 24) == 0x02)
                    {
                        xboxhle.xbox.video.video.arr[(reg32.ebx + reg32.ecx * ((addr32.eip & 0x00000F00) >> 8) + ((addr32.eip & 0xFF000000) >> 24)) & 0x00FFFFFF].framebuffer = (UInt32)(reg32.eax << 16) & 0x00FF0000;
                        addr32.pc += 4;
                    }
                    break;
                case 0xC1:
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0xE8)
                    {
                        temp.value = ((addr32.eip & 0x00FF0000) >> 16);
                        reg32.eax = reg32.eax >> temp.value;
                        addr32.pc += 3;
                    }
                    break;
                case 0x5D: // pop ebp
                    addr32.pc += 1;
                    break;
                case 0xC2: // retn
                    ePtr.esp = ePtr.esp - 1;
                    addr32.pc = (int)(arr[ePtr.esp].dword + 5);
                    break;
                case 0xFF: // push dword ptr ds:value
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0x35)
                    {
                        seg16.ds = (int)((xboxhle.xbe.mem[(Int32)(((addr32.eip & 0xFFFF0000) >> 16)) + 2] << 16 | xboxhle.xbe.mem[(Int32)(((addr32.eip & 0xFFFF0000) >> 16)) + 1] << 8 | xboxhle.xbe.mem[(Int32)(((addr32.eip & 0xFFFF0000) >> 16)) + 0] << 0));
                        arr[ePtr.esp].dword = (UInt32)seg16.ds;
                        ePtr.esp = ePtr.esp + 1;
                        addr32.pc += 6;
                    }
                    break;
                case 0xE8:
                    // call sub
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0x95)
                    {
                        arr[ePtr.esp].dword = (UInt32)(addr32.pc & 0xFFFF);
                        ePtr.esp = ePtr.esp + 1;
                        temp.addr = (Int32)((addr32.eip & 0xFFFFFFFF00) >> 8);
                        addr32.pc = addr32.pc - Math.Abs(temp.addr) + 5;
                    }
                    break;
                case 0x81:
                    // add esp, valh
                    temp.value = ((addr32.eip & 0x00FF0000) >> 16);
                    ePtr.esp = ePtr.esp + temp.value;
                    addr32.pc += 6;
                    break;
                case 0xA1:
                    seg16.ds = xboxhle.xbe.mem[((addr32.eip & 0x00FFFF00) >> 8)]; // needs checking
                    reg32.eax = seg16.ds;
                    addr32.pc += 5;
                    break;
                case 0x05:
                    temp.value = ((addr32.eip & 0x0000FF00) >> 8);
                    reg32.eax = reg32.eax + temp.value;
                    addr32.pc += 5;
                    break;
                case 0xA3:
                    // mov ds, eax
                    seg16.ds = (UInt16)((addr32.eip & 0x00FFFF00) >> 8);
                    if (reg32.eax <= 0xFF)
                    {
                        seg16.ds = (UInt16)((addr32.eip & 0x00FFFF00) >> 8);
                        xboxhle.xbe.mem[seg16.ds] = (byte)reg32.eax; // needs checking
                    }
                    addr32.pc += 5;
                    break;
                case 0x48:
                    // dec eax
                    reg32.eax = reg32.eax - 1;
                    addr32.pc += 1;
                    break;
                case 0x75: // jnz
                    if (((addr32.eip & 0x0000FF00) >> 8) == 0xC7)
                    {
                        if (reg32.eax != 0)
                        {
                            ePtr.esp = 0; // adjustment hack
                            temp.offset = (Int32)(((addr32.eip & 0x0000FF00) >> 8) + 0xFFFFFF00);
                            addr32.pc = (addr32.pc + temp.offset) + 2;
                        }
                        else
                        {
                            addr32.pc += 2;
                        }
                    }
                    break;
                case 0xEB:
                    if ((((addr32.eip & 0x0000FF00) >> 8)) == 0xFE)
                    {
                        temp.offset = (Int32)(((addr32.eip & 0x0000FF00) >> 8) + 0xFFFFFF00);
                        addr32.pc = (addr32.pc + temp.offset) + 2;
                    }
                    break;
                default:
                    xbox.emu.emuIsRunning = false;
                    break;
            }
            return addr32.eip;
        }
    }
}
