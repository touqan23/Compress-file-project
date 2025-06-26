namespace FileCompressor
{
    partial class CompressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            fileListBox = new ListBox();
            btnAddFiles = new Button();
            btnRemove = new Button();
            label1 = new Label();
            algoCombo = new ComboBox();
            btnSavePath = new Button();
            btnCompress = new Button();
            progressBar = new ProgressBar();
            lblRatio = new Label();
            grpSecurity = new GroupBox();
            txtConfirmPassword = new TextBox();
            txtPassword = new TextBox();
            lblConfirmPassword = new Label();
            lblPassword = new Label();
            btnResume = new Button();
            btnCancel = new Button();
            btnCompareAlgorithms = new Button();
            btnPause = new Button();
            btnAddFolder = new Button();
            grpSecurity.SuspendLayout();
            SuspendLayout();
            // 
            // fileListBox
            // 
            fileListBox.FormattingEnabled = true;
            fileListBox.ItemHeight = 25;
            fileListBox.Location = new Point(55, 46);
            fileListBox.Name = "fileListBox";
            fileListBox.Size = new Size(443, 104);
            fileListBox.TabIndex = 0;
            // 
            // btnAddFiles
            // 
            btnAddFiles.BackColor = SystemColors.ControlLightLight;
            btnAddFiles.Location = new Point(224, 177);
            btnAddFiles.Name = "btnAddFiles";
            btnAddFiles.Size = new Size(120, 35);
            btnAddFiles.TabIndex = 1;
            btnAddFiles.Text = "إضافة ملفات";
            btnAddFiles.UseVisualStyleBackColor = false;
            btnAddFiles.Click += btnAddFiles_Click;
            // 
            // btnRemove
            // 
            btnRemove.BackColor = SystemColors.ControlLightLight;
            btnRemove.Font = new Font("Segoe UI", 9F);
            btnRemove.Location = new Point(350, 177);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(120, 35);
            btnRemove.TabIndex = 2;
            btnRemove.Text = "إزالة المحدد";
            btnRemove.TextAlign = ContentAlignment.TopCenter;
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(283, 255);
            label1.Name = "label1";
            label1.Size = new Size(175, 25);
            label1.TabIndex = 3;
            label1.Text = "اختر خوارزمية الضغط ";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // algoCombo
            // 
            algoCombo.FormattingEnabled = true;
            algoCombo.Items.AddRange(new object[] { "Huffman", "Shannon-Fano" });
            algoCombo.Location = new Point(55, 247);
            algoCombo.Name = "algoCombo";
            algoCombo.Size = new Size(182, 33);
            algoCombo.TabIndex = 4;
            algoCombo.SelectedIndexChanged += algoCombo_SelectedIndexChanged;
            // 
            // btnSavePath
            // 
            btnSavePath.Location = new Point(55, 315);
            btnSavePath.Name = "btnSavePath";
            btnSavePath.Size = new Size(443, 34);
            btnSavePath.TabIndex = 5;
            btnSavePath.Text = "اختيار مكان الحفظ";
            btnSavePath.UseVisualStyleBackColor = true;
            btnSavePath.Click += btnSavePath_Click;
            // 
            // btnCompress
            // 
            btnCompress.Location = new Point(294, 545);
            btnCompress.Name = "btnCompress";
            btnCompress.Size = new Size(138, 34);
            btnCompress.TabIndex = 6;
            btnCompress.Text = "بدء الضغط";
            btnCompress.UseVisualStyleBackColor = true;
            btnCompress.Click += btnCompress_Click;
            // 
            // progressBar
            // 
            progressBar.BackColor = Color.AliceBlue;
            progressBar.Location = new Point(57, 651);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(443, 27);
            progressBar.TabIndex = 7;
            // 
            // lblRatio
            // 
            lblRatio.AutoSize = true;
            lblRatio.BackColor = Color.Transparent;
            lblRatio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRatio.Location = new Point(361, 692);
            lblRatio.Name = "lblRatio";
            lblRatio.Size = new Size(139, 25);
            lblRatio.TabIndex = 8;
            lblRatio.Text = "نسبة الضغط %  \r\n";
            // 
            // grpSecurity
            // 
            grpSecurity.BackColor = Color.Transparent;
            grpSecurity.Controls.Add(txtConfirmPassword);
            grpSecurity.Controls.Add(txtPassword);
            grpSecurity.Controls.Add(lblConfirmPassword);
            grpSecurity.Controls.Add(lblPassword);
            grpSecurity.Location = new Point(55, 384);
            grpSecurity.Name = "grpSecurity";
            grpSecurity.Size = new Size(443, 144);
            grpSecurity.TabIndex = 9;
            grpSecurity.TabStop = false;
            grpSecurity.Text = "إعدادات الأمان";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(17, 92);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(262, 31);
            txtConfirmPassword.TabIndex = 3;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(17, 39);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(262, 31);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConfirmPassword.Location = new Point(285, 92);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(137, 25);
            lblConfirmPassword.TabIndex = 1;
            lblConfirmPassword.Text = "تأكيد كلمة المرور";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPassword.Location = new Point(306, 39);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(97, 25);
            lblPassword.TabIndex = 0;
            lblPassword.Text = "كلمة المرور";
            // 
            // btnResume
            // 
            btnResume.ForeColor = Color.OliveDrab;
            btnResume.Location = new Point(199, 598);
            btnResume.Name = "btnResume";
            btnResume.Size = new Size(155, 34);
            btnResume.TabIndex = 10;
            btnResume.Text = "استئناف الضغط";
            btnResume.UseVisualStyleBackColor = true;
            btnResume.Click += btnResume_Click;
            // 
            // btnCancel
            // 
            btnCancel.ForeColor = Color.Firebrick;
            btnCancel.Location = new Point(55, 598);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(138, 34);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "الغاء الضغط";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnCompareAlgorithms
            // 
            btnCompareAlgorithms.BackColor = SystemColors.ControlLightLight;
            btnCompareAlgorithms.Location = new Point(115, 545);
            btnCompareAlgorithms.Name = "btnCompareAlgorithms";
            btnCompareAlgorithms.Size = new Size(172, 34);
            btnCompareAlgorithms.TabIndex = 12;
            btnCompareAlgorithms.Text = "مقارنة الخوارزميات";
            btnCompareAlgorithms.UseVisualStyleBackColor = false;
            btnCompareAlgorithms.Click += btnCompareAlgorithms_Click;
            // 
            // btnPause
            // 
            btnPause.ForeColor = Color.OrangeRed;
            btnPause.Location = new Point(360, 598);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(138, 34);
            btnPause.TabIndex = 13;
            btnPause.Text = "ايقاف الضغط";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnAddFolder
            // 
            btnAddFolder.BackColor = SystemColors.ControlLightLight;
            btnAddFolder.Location = new Point(82, 177);
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Size = new Size(136, 35);
            btnAddFolder.TabIndex = 14;
            btnAddFolder.Text = "إضافة مجلدات";
            btnAddFolder.UseVisualStyleBackColor = false;
            btnAddFolder.Click += btnAddFolder_Click;
            // 
            // CompressForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.SeaShell;
            ClientSize = new Size(578, 744);
            Controls.Add(btnAddFolder);
            Controls.Add(btnPause);
            Controls.Add(btnCompareAlgorithms);
            Controls.Add(btnCancel);
            Controls.Add(btnResume);
            Controls.Add(grpSecurity);
            Controls.Add(lblRatio);
            Controls.Add(progressBar);
            Controls.Add(btnCompress);
            Controls.Add(btnSavePath);
            Controls.Add(algoCombo);
            Controls.Add(label1);
            Controls.Add(btnRemove);
            Controls.Add(btnAddFiles);
            Controls.Add(fileListBox);
            Name = "CompressForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CompressForm";
            Load += CompressForm_Load;
            grpSecurity.ResumeLayout(false);
            grpSecurity.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox fileListBox;
        private Button btnAddFiles;
        private Button btnRemove;
        private Label label1;
        private ComboBox algoCombo;
        private Button btnSavePath;
        private Button btnCompress;
        private ProgressBar progressBar;
        private Label lblRatio;
        private GroupBox grpSecurity;
        private TextBox txtConfirmPassword;
        private TextBox txtPassword;
        private Label lblConfirmPassword;
        private Label lblPassword;
        private Button btnResume;
        private Button btnCancel;
        private Button btnCompareAlgorithms;
        private Button btnPause;
        private Button btnAddFolder;
    }
}