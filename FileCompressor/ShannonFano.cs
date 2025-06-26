using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressor
{
    internal class ShannonFano
    {
        public static Dictionary<byte, string> BuildCodes(Dictionary<byte, int> frequencies)
        {
            //نرتب الرموز تنازليا
            var symbols = frequencies.OrderByDescending(kv => kv.Value).ToList();
            var codes = new Dictionary<byte, string>();
            Build(symbols, codes, "");
            return codes;
        }

        private static void Build(List<KeyValuePair<byte, int>> symbols, Dictionary<byte, string> codes, string prefix)
        {
             /*
             التابع عودي فهاد هو الشرط تبع التوقف اني اوصل لمجموعة
             فيها عنصر واحد يعني خلصت كل التقسيمات
             */
            if (symbols.Count == 1)
            {
                codes[symbols[0].Key] = prefix.Length > 0 ? prefix : "0";
                return;
            }

            int total = symbols.Sum(kv => kv.Value);
            int half = total / 2;
            int sum = 0;
            int split = 0;
            //ايجاد النقطة التي تقسم المجموعات لقسمين متساووين
            for (int i = 0; i < symbols.Count; i++)
            {
                sum += symbols[i].Value;
                if (sum >= half)
                {
                    split = i + 1;
                    break;
                }
            }
            //استدعاء عودي للمجموعتين الي انقسمو الاولى بتاخد صفر والتانية واحد 
            Build(symbols.GetRange(0, split), codes, prefix + "0");
            Build(symbols.GetRange(split, symbols.Count - split), codes, prefix + "1");
        }
    }
}

