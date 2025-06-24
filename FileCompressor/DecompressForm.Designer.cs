namespace FileCompressor
{
    partial class DecompressForm
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
            txtCompressedFilePath = new TextBox();
            btnBrowseFile = new Button();
            btnSavePath = new Button();
            txtSavePath = new TextBox();
            grpSecurity = new GroupBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            btnDecompress = new Button();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            algoCombo = new ComboBox();
            label1 = new Label();
            btnCancel = new Button();
            grpSecurity.SuspendLayout();
            SuspendLayout();
            // 
            // txtCompressedFilePath
            // 
            txtCompressedFilePath.Location = new Point(43, 83);
            txtCompressedFilePath.Name = "txtCompressedFilePath";
            txtCompressedFilePath.ReadOnly = true;
            txtCompressedFilePath.Size = new Size(543, 31);
            txtCompressedFilePath.TabIndex = 1;
            txtCompressedFilePath.TextChanged += txtCompressedFilePath_TextChanged;
            // 
            // btnBrowseFile
            // 
            btnBrowseFile.Location = new Point(227, 43);
            btnBrowseFile.Name = "btnBrowseFile";
            btnBrowseFile.Size = new Size(173, 34);
            btnBrowseFile.TabIndex = 2;
            btnBrowseFile.Text = "الملف المضغوط";
            btnBrowseFile.UseVisualStyleBackColor = true;
            btnBrowseFile.Click += btnBrowseFile_Click;
            // 
            // btnSavePath
            // 
            btnSavePath.Location = new Point(227, 148);
            btnSavePath.Name = "btnSavePath";
            btnSavePath.Size = new Size(173, 34);
            btnSavePath.TabIndex = 4;
            btnSavePath.Text = "اختيار مكان الحفظ";
            btnSavePath.UseVisualStyleBackColor = true;
            btnSavePath.Click += btnSavePath_Click;
            // 
            // txtSavePath
            // 
            txtSavePath.Location = new Point(43, 188);
            txtSavePath.Name = "txtSavePath";
            txtSavePath.ReadOnly = true;
            txtSavePath.Size = new Size(543, 31);
            txtSavePath.TabIndex = 3;
            // 
            // grpSecurity
            // 
            grpSecurity.BackColor = Color.Transparent;
            grpSecurity.Controls.Add(txtPassword);
            grpSecurity.Controls.Add(lblPassword);
            grpSecurity.Location = new Point(92, 310);
            grpSecurity.Name = "grpSecurity";
            grpSecurity.Size = new Size(443, 84);
            grpSecurity.TabIndex = 10;
            grpSecurity.TabStop = false;
            grpSecurity.Text = "إعدادات الأمان";
            grpSecurity.Enter += grpSecurity_Enter;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(17, 39);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(262, 31);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
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
            // btnDecompress
            // 
            btnDecompress.AutoSize = true;
            btnDecompress.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDecompress.Location = new Point(305, 423);
            btnDecompress.Name = "btnDecompress";
            btnDecompress.Size = new Size(173, 42);
            btnDecompress.TabIndex = 11;
            btnDecompress.Text = "فك الضغط";
            btnDecompress.UseVisualStyleBackColor = true;
            btnDecompress.Click += btnDecompress_Click;
            // 
            // progressBar
            // 
            progressBar.BackColor = Color.AliceBlue;
            progressBar.Location = new Point(74, 471);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(443, 27);
            progressBar.TabIndex = 12;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(380, 525);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(206, 25);
            lblStatus.TabIndex = 13;
            lblStatus.Text = "الحالة : العملية لم تبدأ بعد";
            // 
            // algoCombo
            // 
            algoCombo.FormattingEnabled = true;
            algoCombo.Items.AddRange(new object[] { "Huffman", "Shannon-Fano" });
            algoCombo.Location = new Point(104, 237);
            algoCombo.Name = "algoCombo";
            algoCombo.Size = new Size(182, 33);
            algoCombo.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(332, 245);
            label1.Name = "label1";
            label1.Size = new Size(204, 25);
            label1.TabIndex = 14;
            label1.Text = "اختر خوارزمية فك الضغط ";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            btnCancel.ForeColor = Color.Firebrick;
            btnCancel.Location = new Point(109, 423);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(173, 42);
            btnCancel.TabIndex = 16;
            btnCancel.Text = "الغاء الضغط";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // DecompressForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SeaShell;
            ClientSize = new Size(644, 574);
            Controls.Add(btnCancel);
            Controls.Add(algoCombo);
            Controls.Add(label1);
            Controls.Add(lblStatus);
            Controls.Add(progressBar);
            Controls.Add(btnDecompress);
            Controls.Add(grpSecurity);
            Controls.Add(btnSavePath);
            Controls.Add(txtSavePath);
            Controls.Add(btnBrowseFile);
            Controls.Add(txtCompressedFilePath);
            Name = "DecompressForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DecompressForm";
            Load += DecompressForm_Load;
            grpSecurity.ResumeLayout(false);
            grpSecurity.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtCompressedFilePath;
        private Button btnBrowseFile;
        private Button btnSavePath;
        private TextBox txtSavePath;
        private GroupBox grpSecurity;
        private TextBox txtPassword;
        private Label lblPassword;
        private Button btnDecompress;
        private ProgressBar progressBar;
        private Label lblStatus;
        private ComboBox algoCombo;
        private Label label1;
        private Button btnCancel;
    }
}