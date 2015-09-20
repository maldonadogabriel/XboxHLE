// © Copyright 2015  Gabriel Maldonado.  All Rights Reserved.
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

        private void frmApp_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " Application Log";
            
            // Retrieves saved bit for verifying the state of our sources in the application log. 
            //menuItem2.Checked = xboxhle.Properties.Settings.Default.isXBEActive; // For legacy code reasons, this must be disabled. Until some rewrites are met for the xbe loader.
            menuItem3.Checked = xboxhle.Properties.Settings.Default.isI386Active;           
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (menuItem2.Checked == true)
            {
                menuItem2.Checked = false;
                xboxhle.Properties.Settings.Default.isXBEActive = false;
            }
            else if (menuItem2.Checked == false)
            {
                menuItem2.Checked = true;
                xboxhle.Properties.Settings.Default.isXBEActive = true;
            }
            xboxhle.Properties.Settings.Default.Save();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (menuItem3.Checked == true)
            {
                menuItem3.Checked = false;
                xboxhle.Properties.Settings.Default.isI386Active = false;
            }
            else if (menuItem3.Checked == false)
            {
                menuItem3.Checked = true;
                xboxhle.Properties.Settings.Default.isI386Active = true;
            }
            xboxhle.Properties.Settings.Default.Save();
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            Application.OpenForms["frmMain"].Close();
        }
    }
}
