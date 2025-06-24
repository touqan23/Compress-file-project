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
            public byte? Symbol;
            public int Frequency;
            public Node Left, Right;
        }

        public static Dictionary<byte, string> BuildCodes(Dictionary<byte, int> frequencies)
        {
            var nodes = new List<Node>(frequencies.Select(kv => new Node { Symbol = kv.Key, Frequency = kv.Value }));

            while (nodes.Count > 1)
            {
                var ordered = nodes.OrderBy(n => n.Frequency).ToList();
                var left = ordered[0];
                var right = ordered[1];

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

            var codes = new Dictionary<byte, string>();
            Traverse(nodes[0], "", codes);
            return codes;
        }

        private static void Traverse(Node node, string code, Dictionary<byte, string> codes)
        {
            if (node.Symbol.HasValue)
                codes[node.Symbol.Value] = code;
            else
            {
                Traverse(node.Left, code + "0", codes);
                Traverse(node.Right, code + "1", codes);
            }
        }
    }
}
