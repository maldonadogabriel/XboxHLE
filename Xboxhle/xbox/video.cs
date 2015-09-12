// © Copyright 2015  Gabriel Maldonado.  All Rights Reserved.
// Linkedin: https://www.linkedin.com/pub/gabriel-maldonado/63/457/866

using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace xboxhle
{
    public class video
    {
        public static void vmem_read(PaintEventArgs e)
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
                        if (i386.framebuffer.Length != 0) { *v++ = i386.framebuffer[x + y * 640 * 4]; }
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
                "reg32_eax:" + xboxhle.i386.reg32_eax.ToString("X") + " " + "reg32_ebx:" + xboxhle.i386.reg32_ebx.ToString("X") + "\n" +
                "reg32_ecx:" + xboxhle.i386.reg32_ecx.ToString("X") + " " + "reg32_edx:" + xboxhle.i386.reg32_edx.ToString("X") +
                "\n\n" + "src/des" +
                "\n" + "reg32_esi:" + xboxhle.i386.reg32_esi.ToString("X") + " " + "reg32_edi:" + xboxhle.i386.reg32_edi.ToString("X") +
                "\n\n" + "pointers" +
                "\n" + "reg32_ebp:" + xboxhle.i386.reg32_ebp.ToString("X") + " " + "reg32_esp:" + xboxhle.i386.reg32_esp.ToString("X") +
                "\n\n" + "segments" +
                "\n" + "seg16_cs:" + xboxhle.i386.seg16_cs.ToString("X") + " " + "seg16_ds:" + xboxhle.i386.seg16_ds.ToString("X") +
                "\n" + "seg16_es:" + xboxhle.i386.seg16_es.ToString("X") + " " + "seg16_ss:" + xboxhle.i386.seg16_ss.ToString("X");

            // Create font and brush.
            Font drawFont = new Font("Consolas", 8);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            // Create point for upper-left corner of drawing.
            PointF drawPoint = new PointF(0, 0);

            // Draw string to screen.
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }
    }
}
