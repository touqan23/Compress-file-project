namespace FileCompressor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            label1 = new Label();
            btnCompress = new Button();
            btnDecompress = new Button();
            btnExit = new Button();
            timer = new PictureBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)timer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = SystemColors.ButtonHighlight;
            label1.Font = new Font("Bauhaus 93", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(-61, 52);
            label1.Name = "label1";
            label1.Size = new Size(807, 38);
            label1.TabIndex = 0;
            label1.Text = "مرحبًا بك في برنامج ضغط الملفات";
            label1.TextAlign = ContentAlignment.TopCenter;
            label1.Click += label1_Click;
            // 
            // btnCompress
            // 
            btnCompress.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCompress.Location = new Point(152, 131);
            btnCompress.Name = "btnCompress";
            btnCompress.Size = new Size(366, 69);
            btnCompress.TabIndex = 1;
            btnCompress.Text = "ضغط ملفات";
            btnCompress.UseVisualStyleBackColor = true;
            btnCompress.Click += btnCompress_Click;
            // 
            // btnDecompress
            // 
            btnDecompress.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDecompress.Location = new Point(152, 250);
            btnDecompress.Name = "btnDecompress";
            btnDecompress.Size = new Size(366, 69);
            btnDecompress.TabIndex = 2;
            btnDecompress.Text = "فك ضغط الملفات";
            btnDecompress.UseVisualStyleBackColor = true;
            btnDecompress.Click += btnDecompress_Click;
            // 
            // btnExit
            // 
            btnExit.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(240, 384);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(185, 47);
            btnExit.TabIndex = 4;
            btnExit.Text = "خروج";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // timer
            // 
            timer.BackColor = SystemColors.ButtonHighlight;
            timer.Image = (Image)resources.GetObject("timer.Image");
            timer.Location = new Point(439, 142);
            timer.Name = "timer";
            timer.Size = new Size(61, 47);
            timer.SizeMode = PictureBoxSizeMode.Zoom;
            timer.TabIndex = 11;
            timer.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ButtonHighlight;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(439, 260);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(61, 47);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PeachPuff;
            ClientSize = new Size(720, 486);
            Controls.Add(pictureBox1);
            Controls.Add(timer);
            Controls.Add(btnExit);
            Controls.Add(btnDecompress);
            Controls.Add(btnCompress);
            Controls.Add(label1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)timer).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Button btnCompress;
        private Button btnDecompress;
        private Button btnExit;
        private PictureBox timer;
        private PictureBox pictureBox1;
    }
}