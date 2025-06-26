using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileCompressor
{
    public partial class DecompressForm : Form
    {

        private CancellationTokenSource cancellationTokenSource;

        public DecompressForm()
        {
            InitializeComponent();
        }

        private void DecompressForm_Load(object sender, EventArgs e)
        {
        }



        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Compressed Files (*.cmp)|*.cmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtCompressedFilePath.Text = ofd.FileName;
            }
        }

        private void txtCompressedFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "اختر مجلد لحفظ الملفات بعد فك الضغط";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtSavePath.Text = fbd.SelectedPath;
                }
            }
        }

        private async void btnDecompress_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCompressedFilePath.Text))
            {
                MessageBox.Show("يرجى اختيار الملف المضغوط.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtSavePath.Text))
            {
                MessageBox.Show("يرجى اختيار مجلد الاستخراج.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            

            string password = txtPassword.Text;
            string compressedFile = txtCompressedFilePath.Text;
            string outputFolder = txtSavePath.Text;

            

            btnDecompress.Enabled = false;
            btnCancel.Enabled = true;
            lblStatus.Text = "الحالة: جاري فك الضغط...";
            progressBar.Style = ProgressBarStyle.Marquee;
            var progress = new Progress<int>(value =>
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = value;
                lblStatus.Text = $"الحالة: جاري فك الضغط... {value}%";
            });

            cancellationTokenSource = new CancellationTokenSource();

            try
            {
               
                await CompressionAlgorithms.DecompressFile(
     compressedFile,
     outputFolder,
     password,
     cancellationTokenSource.Token,
     progress
 );

                lblStatus.Text = "الحالة: تم فك الضغط بنجاح ✅";
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 100;

                MessageBox.Show("✅ تم فك ضغط الملف بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Close();
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = "الحالة: تم إلغاء العملية ❌";
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                MessageBox.Show("❌ تم إلغاء عملية فك الضغط.", "ملغي", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "الحالة: فشل في فك الضغط ❌";
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                MessageBox.Show("❌ حدث خطأ أثناء فك الضغط:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDecompress.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private void grpSecurity_Enter(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                lblStatus.Text = "جاري الإلغاء...";
                Application.Exit();
            }
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "اختر مجلد";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtCompressedFilePath.Text = folderDialog.SelectedPath;
                }
            }
        }
    }
}
