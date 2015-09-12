// © Copyright 2015  Gabriel Maldonado.  All Rights Reserved.
// Linkedin: https://www.linkedin.com/pub/gabriel-maldonado/63/457/866

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xboxhle
{
    public partial class frmMain : Form
    {
        public static bool emuIsRunning = false;

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
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            Graphics displayGraphics = e.Graphics;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            xboxhle.video.vmem_read(e);
            xboxhle.video.debugInfo(e);
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
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Load Xbox Executable File
            xboxhle.xbe.load_pXbe(openFileDialog1.FileName);
            
            // Print Xbox Executable Header
            //xboxhle.output.pXbe();
            xboxhle.xbe.print_pXbe();

            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            
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
                emuIsRunning = true;
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            xboxhle.i386.reg32_pc = xboxhle.xbe.SubtractOffset(xboxhle.xbe.entry_points, xboxhle.xbe.base_addr);

            while (emuIsRunning == true)
            {
                if (!(bw.CancellationPending == true))
                {
                    // Interpret and report instructions that are executed.
                    int OpCode = xboxhle.i386.x86();
                    
                    // Report executed instructions to the applications' profiler.
                    //bw.ReportProgress(OpCode);

                    // Cancel thread if emuIsRunning = False
                    if (emuIsRunning == false) bw.CancelAsync();

                    // Invalidate form once per-cycle.
                    this.Invalidate();

                    // Delay Thread for 100 miliseconds
                    System.Threading.Thread.Sleep(100);
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
            xboxhle.output.x86(xboxhle.i386.reg32_pc, xboxhle.i386.reg32_eip);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) 
        {
            
            this.Text = Application.ProductName;
        }   
    }
}
