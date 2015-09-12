// © copyright 2015  Gabriel Maldonado.  All Rights Reserved.
// Linkedin: https://www.linkedin.com/pub/gabriel-maldonado/63/457/866

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace xboxhle
{
    public partial class frmApp : Form
    {

        public frmApp()
        {
            InitializeComponent();
        }

        public string ThisText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        private void frmApp_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " Application Log";
            int interval = 0;
            frmMain MainFrm = new frmMain();
            while (this.Visible == true)
            {
                if (interval == 1)
                {
                    if (this.Visible == true)
                    {
                        MainFrm.Show();
                        break;
                    }
                }
                interval++;
            }
            
        }
    }
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color fcolor, Color bcolor)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = fcolor;
            box.SelectionBackColor = bcolor;
            
            box.AppendText(text);
        }
    }
}
