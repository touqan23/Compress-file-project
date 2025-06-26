using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FileCompressor.CompressionAlgorithms;

namespace FileCompressor
{
    public partial class CompressForm : Form
    {
        private List<string> selectedFiles = new List<string>();
        string savePath = "";
        private CancellationTokenSource cancellationTokenSource;
        private ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);
        private bool isPaused = false;

        public CompressForm()
        {
            InitializeComponent();
        }

        private void CompressForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in ofd.FileNames)
                {
                    selectedFiles.Add(file);
                    fileListBox.Items.Add(file);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            if (fileListBox.SelectedItem != null)
            {
                selectedFiles.Remove(fileListBox.SelectedItem.ToString());
                fileListBox.Items.Remove(fileListBox.SelectedItem);
            }
        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "اختر مجلد لحفظ الملفات المضغوطة ";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;

                    // Assume the first file in list box is the source
                    if (fileListBox.Items.Count > 0)
                    {
                        // Get file name without extension
                        string fileName = Path.GetFileNameWithoutExtension(fileListBox.Items[0].ToString());

                        // Create a new folder with the file name inside the selected path
                        string targetFolderPath = Path.Combine(selectedPath, fileName);
                        savePath = targetFolderPath;

                        if (!Directory.Exists(targetFolderPath))
                        {
                            Directory.CreateDirectory(targetFolderPath);
                        }

                        // Now you can store this path and use it for saving later

                        MessageBox.Show("سيتم حفظ الملفات في:\n" + targetFolderPath, "تم الاختيار", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("يرجى تحديد ملف أولاً من القائمة", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void algoCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void btnCompress_Click(object sender, EventArgs e)
        {
            if (selectedFiles.Count == 0)
            {
                MessageBox.Show("يرجى اختيار ملفات أولاً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(savePath))
            {
                MessageBox.Show("يرجى اختيار مكان الحفظ أولاً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (algoCombo.SelectedItem == null)
            {
                MessageBox.Show("يرجى اختيار خوارزمية الضغط.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (password != confirmPassword)
            {
                MessageBox.Show("كلمتا المرور غير متطابقتين!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnCompress.Enabled = false;
            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Blocks;
            //lblStatus.Text = "الحالة: جاري الضغط...";

            List<FileToCompress> filesToCompress = selectedFiles.Select(filePath => new FileToCompress
            {
                FullPath = filePath,
                RelativePath = Path.GetFileName(filePath) // أو احسب المسار النسبي حسب ما يلزمك
            }).ToList();

            CompressionAlgorithms.Algorithm algo = algoCombo.SelectedItem.ToString() == "Huffman"
                ? CompressionAlgorithms.Algorithm.Huffman
                : CompressionAlgorithms.Algorithm.ShannonFano;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            pauseEvent.Set();

            var progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
                //lblStatus.Text = $"الحالة: جاري الضغط... {value}%";
            });

            try
            {
                var results = await Task.Run(() =>
                    CompressionAlgorithms.CompressFiles(filesToCompress, savePath, algo, progress, password, pauseEvent, token)
                );

                double avgRatio = results.Average(r => r.CompressionRatio);
                lblRatio.Text = $"نسبة الضغط: {avgRatio:F2}%";
                progressBar.Value = 100;
                //lblStatus.Text = "تم الضغط بنجاح ✅";

                MessageBox.Show("✅ تم ضغط جميع الملفات بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Close();
            }
            catch (OperationCanceledException)
            {
                //lblStatus.Text = "تم إلغاء عملية الضغط ❌";
                progressBar.Value = 0;
                MessageBox.Show("❌ تم إلغاء عملية الضغط.", "ملغي", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                //lblStatus.Text = "حدث خطأ أثناء الضغط ❌";
                progressBar.Value = 0;
                MessageBox.Show("حدث خطأ أثناء الضغط:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCompress.Enabled = true;
            }
        }


        private void btnCompareAlgorithms_Click(object sender, EventArgs e)
        {
            if (selectedFiles.Count != 1)
            {
                MessageBox.Show("يرجى اختيار ملف واحد فقط للمقارنة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFile = selectedFiles[0];
            ComparisonForm comparisonForm = new ComparisonForm(selectedFile);
            comparisonForm.Show();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                pauseEvent.Reset(); // إيقاف مؤقت
                isPaused = true;
                MessageBox.Show("⏸️ تم إيقاف الضغط مؤقتًا.");
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                pauseEvent.Set(); // استئناف
                isPaused = false;
                MessageBox.Show("▶️ تم استئناف الضغط.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                btnCompress.Enabled = true;
                pauseEvent.Set(); // تأكد من فك التوقف في حال كان موقوف
                Application.Exit();
            }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "اختر مجلد لإضافته مع جميع محتوياته";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolder = folderDialog.SelectedPath;

                    // اجلب كل الملفات داخل المجلد والمجلدات الفرعية
                    var allFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.AllDirectories);

                    foreach (var file in allFiles)
                    {
                        if (!selectedFiles.Contains(file))
                        {
                            selectedFiles.Add(file);
                            fileListBox.Items.Add(file);
                        }
                    }

                    MessageBox.Show($"تمت إضافة {allFiles.Length} ملفًا من المجلد:\n{selectedFolder}", "تم الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}

