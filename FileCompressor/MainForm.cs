using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileCompressor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the main form
            CompressForm compressForm = new CompressForm();
            compressForm.FormClosed += (s, args) => this.Show(); // Re-show main when compress form is closed
            compressForm.Show();
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the main form
            DecompressForm decompressForm = new DecompressForm();
            decompressForm.FormClosed += (s, args) => this.Show(); // Re-show main when compress form is closed
            decompressForm.Show();
        }

      
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
