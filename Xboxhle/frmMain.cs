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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xboxhle
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            // Background worker Event Handler
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            int interval = 0;
            frmApp AppFrm = new frmApp();
            while (this.Visible == true)
            {
                if (interval == 10000000)
                {
                    if (this.Visible == true)
                    {
                        AppFrm.Show();
                        break;
                    }
                }
                interval++;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            Graphics displayGraphics = e.Graphics;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            xboxhle.xbox.video.video.frameBuffer_Draw(e);
            xboxhle.xbox.video.video.debugInfo(e);
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
                openFileDialog1.ShowDialog();
            }
        }
        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private  void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Load Xbox Executable File
            xboxhle.xbe.load_pXbe(openFileDialog1.FileName);
            
            // Print Xbox Executable Header
            //xboxhle.output.pXbe();
            if (xboxhle.Properties.Settings.Default.isXBEActive == true) xboxhle.xbe.print_pXbe();

            TextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as TextBox;
            
            if (!(xboxhle.xbe.cert_title_name == null))
            {
                t.AppendText("Starting " + xboxhle.xbe.cert_title_name.ToString() + " Emulation..." + "\r\n");
                this.Text = xboxhle.xbe.cert_title_name.ToString() + " – " + Application.ProductName;
            }
            else
            {
                t.AppendText("Starting " + "Undefined Emulation..." + "\r\n");
                this.Text = "Undefined" + " – " + Application.ProductName;
            }

            Application.OpenForms["frmApp"].Text = Path.GetFileName(xboxhle.xbe.pXbe.Name + " – " + Application.ProductName + " Application Log");

            // Initialize Thread
            if (!(bw.IsBusy))
            {
                xbox.emu.emuIsRunning = true;
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            xboxhle.xbox.i386.parse.addr32.pc = xboxhle.xbe.SubtractOffset(xboxhle.xbe.entry_points, xboxhle.xbe.base_addr);

            while (xbox.emu.emuIsRunning == true)
            {
                if (!(bw.CancellationPending == true))
                {
                    // Interpret and report instructions that are executed.
                    int OpCode = xboxhle.xbox.emu.execute_x86();
                    
                    if (xboxhle.Properties.Settings.Default.isI386Active == true)
                    {
                        // Report Progress
                        bw.WorkerReportsProgress = true;
                        
                        // Report executed instructions to the applications' profiler.
                        bw.ReportProgress(OpCode);

                        // Delay Thread for 50 miliseconds
                        System.Threading.Thread.Sleep(50);
                    }
                    else if (xboxhle.Properties.Settings.Default.isI386Active == false) 
                    {
                        // Disable progress reporting
                        bw.WorkerReportsProgress = false;
                        
                        // Delay Thread for 0 miliseconds
                        System.Threading.Thread.Sleep(0);
                    }

                    // Cancel thread if emuIsRunning = False
                    if (xbox.emu.emuIsRunning == false) bw.CancelAsync();

                    // Invalidate form once per-cycle.
                    this.Invalidate();   
                }
                else {
                    e.Cancel = true;
                    break;
                }
            }        
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) 
        {
            // Send API Logging to Application log.
            xboxhle.output.x86(xboxhle.xbox.i386.parse.addr32.pc, xboxhle.xbox.i386.parse.addr32.eip);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) 
        {          
            this.Text = Application.ProductName;
            xboxhle.xbox.emu.reset_x86();
            this.Invalidate();
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            if (xboxhle.xbox.emu.Pause == true) {
                xboxhle.xbox.emu.Pause = false;
                menuItem6.Checked = false;
            } else if (xboxhle.xbox.emu.Pause == false) {
                xboxhle.xbox.emu.Pause = true;
                menuItem6.Checked = true;
            }
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            xboxhle.xbox.emu.hReset = true;
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            xbox.emu.emuIsRunning = false;
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            //frmAbout AboutFrm = new frmAbout();
            //AboutFrm.ShowDialog();
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/Gabriel-Maldonado/XboxHLE");
        }   
    }
}
