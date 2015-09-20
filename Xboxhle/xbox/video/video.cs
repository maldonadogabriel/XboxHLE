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

using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace xboxhle.xbox.video
{
    public class video
    {
        public struct frame
        {
            public UInt32 framebuffer;
        }

        public static frame[] arr = new frame[0x00FFFFFF];

        public static void frameBuffer_Draw(PaintEventArgs e)
        {
            Graphics displayGraphics = e.Graphics;
            Bitmap tmpBmp = new Bitmap(640, 480);

            BitmapData Locked = tmpBmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            unsafe
            {
                UInt32* v = (UInt32*)Locked.Scan0;

                for (uint y = 0; y < 480; ++y)
                {
                    for (uint x = 0; x < 640; ++x)
                    {
                        if (xboxhle.xbox.i386.parse.arr.Length != 0) {
                            *v++ = arr[x + y * 640 * 4].framebuffer;
                         }
                    }
                }
            }
            
            tmpBmp.UnlockBits(Locked);
            displayGraphics.DrawImageUnscaled(tmpBmp, new Rectangle(0, 0, 640, 480)); 
        }
        public static void debugInfo(PaintEventArgs e)
        {
            // Create string to draw.
            String drawString =
                "registers \n" +
                "reg32_eax:" + xboxhle.xbox.i386.parse.reg32.eax.ToString("X") + " " + "reg32_ebx:" + xboxhle.xbox.i386.parse.reg32.ebx.ToString("X") + "\n" +
                "reg32_ecx:" + xboxhle.xbox.i386.parse.reg32.ecx.ToString("X") + " " + "reg32_edx:" + xboxhle.xbox.i386.parse.reg32.edx.ToString("X") +
                "\n\n" + "src/des" +
                "\n" + "esi:" + xboxhle.xbox.i386.parse.eIdx.esi.ToString("X") + " " + "edi:" + xboxhle.xbox.i386.parse.eIdx.edi.ToString("X") +
                "\n\n" + "pointers" +
                "\n" + "ebp:" + xboxhle.xbox.i386.parse.ePtr.ebp.ToString("X") + " " + "esp:" + xboxhle.xbox.i386.parse.ePtr.esp.ToString("X") +
                "\n\n" + "segments" +
                "\n" + "seg16_cs:" + xboxhle.xbox.i386.parse.seg16.cs.ToString("X") + " " + "seg16_ds:" + xboxhle.xbox.i386.parse.seg16.ds.ToString("X") +
                "\n" + "seg16_es:" + xboxhle.xbox.i386.parse.seg16.es.ToString("X") + " " + "seg16_ss:" + xboxhle.xbox.i386.parse.seg16.ss.ToString("X");

            // Create font and brush.
            Font drawFont = new Font("Consolas", 8);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            // Create point for upper-left corner of drawing.
            PointF drawPoint = new PointF(0, 0);

            // Draw string to screen.
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }

        public static void frameBuffer_Clear() {
            for (int y = 0; y < 480; y++)
            {
                for (int x = 0; x < 640; x++)
                {
                    arr[x + y * 640 * 4].framebuffer = 0;
                }
            }
        }
    }
}
