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
    public partial class ComparisonForm : Form
    {
        private string filePath;
        public ComparisonForm(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnRunComparison_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear();
            var huffmanResult = CompressionAlgorithms.TestCompression(filePath, Path.GetTempPath(), CompressionAlgorithms.Algorithm.Huffman);
            var shannonResult = CompressionAlgorithms.TestCompression(filePath, Path.GetTempPath(), CompressionAlgorithms.Algorithm.ShannonFano);

            resultsListBox.Items.Add(" Huffman:");
            resultsListBox.Items.Add($"- الحجم قبل: {huffmanResult.OriginalSize} بايت");
            resultsListBox.Items.Add($"- الحجم بعد: {huffmanResult.CompressedSize} بايت");
            resultsListBox.Items.Add($"- نسبة الضغط: {huffmanResult.CompressionRatio:F2}%");
            resultsListBox.Items.Add($"- الزمن: {huffmanResult.ExecutionTimeMs} مللي ثانية");
            resultsListBox.Items.Add("");

            resultsListBox.Items.Add(" Shannon-Fano:");
            resultsListBox.Items.Add($"- الحجم قبل: {shannonResult.OriginalSize} بايت");
            resultsListBox.Items.Add($"- الحجم بعد: {shannonResult.CompressedSize} بايت");
            resultsListBox.Items.Add($"- نسبة الضغط: {shannonResult.CompressionRatio:F2}%");
            resultsListBox.Items.Add($"- الزمن: {shannonResult.ExecutionTimeMs} مللي ثانية");
        }
    }
}
