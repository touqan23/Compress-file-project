using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressor
{
    internal class Huffman
    {
        class Node
        {
            public byte? Symbol;//الرمز
            public int Frequency;//عدد مرات ظهور الرمز يعني احتماله
            public Node Left, Right;
        }

        public static Dictionary<byte, string> BuildCodes(Dictionary<byte, int> frequencies)
        {
            var nodes = new List<Node>(frequencies.Select(kv => new Node { Symbol = kv.Key, Frequency = kv.Value }));
            //بكرر هي الحلقة حتى اخلص من كل العقد وابني الشجرة كاملة
            while (nodes.Count > 1)
            {
                //عم رتب العقد من الاصغر للاكبر
                var ordered = nodes.OrderBy(n => n.Frequency).ToList();
                var left = ordered[0];
                var right = ordered[1];
                //ع انشأ عقدة جديدة الي هي مجموع العقدتين
                var parent = new Node
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };

                nodes.Remove(left);
                nodes.Remove(right);
                nodes.Add(parent);
            }
            //عم مر على الشجرة بدأ من الجذر واعرف شو بايتات لكل رمز 
            var codes = new Dictionary<byte, string>();
            Traverse(nodes[0], "", codes);
            return codes;
        }

        private static void Traverse(Node node, string code, Dictionary<byte, string> codes)
        {
            //اذا وصلت عند ورقة بسجل شو قيمة الرمز
            if (node.Symbol.HasValue)
                codes[node.Symbol.Value] = code;
            //والا كل شي يسار العقدة هو صفر كل شي يمينها هو واحد
            else
            {
                Traverse(node.Left, code + "0", codes);
                Traverse(node.Right, code + "1", codes);
            }
        }
    }
}
