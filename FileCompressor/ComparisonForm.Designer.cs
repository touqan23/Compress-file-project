namespace FileCompressor
{
    partial class ComparisonForm
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
            label2 = new Label();
            resultsListBox = new ListBox();
            btnRunComparison = new Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.BackColor = SystemColors.ButtonHighlight;
            label2.Font = new Font("Bell MT", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(-5, 22);
            label2.Name = "label2";
            label2.Size = new Size(807, 38);
            label2.TabIndex = 1;
            label2.Text = "نتائج مقارنة الخوارزميتين ";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // resultsListBox
            // 
            resultsListBox.FormattingEnabled = true;
            resultsListBox.ItemHeight = 25;
            resultsListBox.Location = new Point(136, 102);
            resultsListBox.Name = "resultsListBox";
            resultsListBox.Size = new Size(519, 254);
            resultsListBox.TabIndex = 2;
            // 
            // btnRunComparison
            // 
            btnRunComparison.Location = new Point(288, 385);
            btnRunComparison.Name = "btnRunComparison";
            btnRunComparison.Size = new Size(203, 34);
            btnRunComparison.TabIndex = 3;
            btnRunComparison.Text = "ابدأ المقارنة";
            btnRunComparison.UseVisualStyleBackColor = true;
            btnRunComparison.Click += btnRunComparison_Click;
            // 
            // ComparisonForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PeachPuff;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRunComparison);
            Controls.Add(resultsListBox);
            Controls.Add(label2);
            Name = "ComparisonForm";
            Text = "ComparisonForm";
            ResumeLayout(false);
        }

        #endregion
        private Label label2;
        private ListBox resultsListBox;
        private Button btnRunComparison;
    }
}