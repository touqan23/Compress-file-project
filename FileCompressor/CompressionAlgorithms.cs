using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FileCompressor
{
    internal class CompressionAlgorithms
    {
        public enum Algorithm
        {
            Huffman,
            ShannonFano
        }

        public class CompressionResult
        {
            public string AlgorithmName;
            public long OriginalSize;
            public byte[] CompressedData;
            public double CompressionRatio;
            public long ExecutionTimeMs;
            public long CompressedSize => CompressedData?.Length ?? 0;
        }
        public class FileToCompress
        {
            public string FullPath { get; set; }         // المسار الكامل الحقيقي
            public string RelativePath { get; set; }     // المسار النسبي داخل الأرشيف
        }
        // تعديل دالة الضغط لتدعم ضغط الملفات مع المسار النسبي
        public static List<CompressionResult> CompressFiles(
            List<FileToCompress> files, string saveDirectory, Algorithm algorithm,
            Action<string> reportProgress, string password,
            ManualResetEventSlim pauseEvent, CancellationToken token)
        {
            var results = new List<CompressionResult>();

            foreach (var file in files)
            {
                token.ThrowIfCancellationRequested();
                pauseEvent.Wait();

                byte[] fileData = File.ReadAllBytes(file.FullPath);
                string relativePath = file.RelativePath.Replace('\\', '/');


                Dictionary<byte, int> frequencies = CountFrequencies(fileData);
                Dictionary<byte, string> codes = (algorithm == Algorithm.Huffman)
                    ? Huffman.BuildCodes(frequencies)
                    : ShannonFano.BuildCodes(frequencies);

                byte[] compressedData = Encode(fileData, codes);

                byte[] finalData;

                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    // 1. Flag: هل الملف مشفر؟
                    writer.Write((byte)(!string.IsNullOrEmpty(password) ? 0x01 : 0x00));

                    // 2. بناء المحتوى الذي سيتم تشفيره أو نسخه مباشرة
                    byte[] contentBytes;
                    using (MemoryStream contentStream = new MemoryStream())
                    using (BinaryWriter contentWriter = new BinaryWriter(contentStream))
                    {
                        contentWriter.Write(relativePath);             // المسار النسبي
                        contentWriter.Write(codes.Count);              // عدد الرموز
                        foreach (var kvp in codes)
                        {
                            contentWriter.Write(kvp.Key);              // المفتاح (byte)
                            contentWriter.Write(kvp.Value);            // الكود (سلسلة bits)
                        }

                        contentWriter.Write(compressedData.Length);    // طول البيانات المضغوطة
                        contentWriter.Write(compressedData);           // البيانات المضغوطة فعليًا

                        contentBytes = contentStream.ToArray();
                    }

                    // 3. التشفير (إذا طلب المستخدم كلمة مرور)
                    if (!string.IsNullOrEmpty(password))
                    {
                        contentBytes = Encrypt(contentBytes, password);
                    }

                    // 4. كتابة طول البيانات النهائية + المحتوى
                    writer.Write(contentBytes.Length); // هذا السطر ضروري لفك الضغط الصحيح
                    writer.Write(contentBytes);        // البيانات (مشفرة أو لا)

                    finalData = ms.ToArray();
                }


                string compressedFileName = Path.GetFileNameWithoutExtension(file.FullPath) + ".cmp";
                string compressedFilePath = Path.Combine(saveDirectory, compressedFileName);

                File.WriteAllBytes(compressedFilePath, finalData);

                double ratio = (1 - (double)compressedData.Length / fileData.Length) * 100.0;
                results.Add(new CompressionResult
                {
                    AlgorithmName = algorithm.ToString(),
                    OriginalSize = fileData.Length,
                    CompressedData = compressedData,
                    CompressionRatio = ratio
                });

                reportProgress?.Invoke($"✅ {relativePath} مضغوط بنسبة {ratio:F2}%");
            }

            return results;
        }

        private static string GetCommonBasePath(List<string> paths)
        {
            if (paths == null || paths.Count == 0)
                return string.Empty;

            string[] separated = paths
                .Select(p => Path.GetFullPath(p))
                .OrderBy(p => p.Length)
                .ToArray();

            string first = separated[0];
            string commonPath = first;

            foreach (string path in separated)
            {
                while (!path.StartsWith(commonPath, StringComparison.OrdinalIgnoreCase))
                {
                    commonPath = Path.GetDirectoryName(commonPath);
                    if (string.IsNullOrEmpty(commonPath)) break;
                }
            }

            return commonPath ?? "";
        }


        public static CompressionResult TestCompression(string filePath, string outputDir, Algorithm algorithm)
        {
            byte[] originalBytes = File.ReadAllBytes(filePath);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // بناء جدول التكرار
            Dictionary<byte, int> frequencies = CountFrequencies(originalBytes);

            // اختيار الخوارزمية
            Dictionary<byte, string> codes = (algorithm == Algorithm.Huffman)
                ? Huffman.BuildCodes(frequencies)
                : ShannonFano.BuildCodes(frequencies);

            // التشفير
            byte[] compressedData = Encode(originalBytes, codes);

            stopwatch.Stop();

            // حفظ الملف مؤقتًا لمجرد التجربة
            string fileName = Path.GetFileName(filePath);
            string compressedPath = Path.Combine(outputDir, fileName + "." + algorithm.ToString().ToLower() + ".cmp");
            SaveCompressedFile(compressedPath, compressedData, codes);

            return new CompressionResult
            {
                AlgorithmName = algorithm.ToString(),
                OriginalSize = originalBytes.Length,
                CompressedData = compressedData,
                CompressionRatio = (1 - (double)compressedData.Length / originalBytes.Length) * 100.0,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        private static void SaveCompressedFile(string path, byte[] data, Dictionary<byte, string> codes)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(path)))
            {
                writer.Write(codes.Count);
                foreach (var kvp in codes)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }
                writer.Write(data.Length);
                writer.Write(data);
            }
        }



        public static async Task DecompressFile(string compressedFilePath, string outputDirectory, Algorithm algorithm,
       string password = null, CancellationToken token = default, IProgress<int> progress = null)
        {
            await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();

                if (!File.Exists(compressedFilePath))
                    throw new FileNotFoundException("الملف المضغوط غير موجود.", compressedFilePath);

                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);

                progress?.Report(5);

                byte[] rawData = File.ReadAllBytes(compressedFilePath);
                token.ThrowIfCancellationRequested();

                bool isEncrypted;
               
                try
                {
                    using (var checkStream = new MemoryStream(rawData))
                    using (var checkReader = new BinaryReader(checkStream))
                    {
                        isEncrypted = checkReader.ReadByte() == 0x01;
                    }
                }
                catch
                {
                    throw new InvalidOperationException("❌ الملف تالف أو ليس ملف مضغوط صالح.");
                }

                progress?.Report(10);

                if (isEncrypted && string.IsNullOrEmpty(password))
                    throw new InvalidOperationException("❌ الملف مشفّر، الرجاء إدخال كلمة المرور.");

               

                token.ThrowIfCancellationRequested();
                using (MemoryStream ms = new MemoryStream(rawData))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    byte flag = reader.ReadByte();
                     isEncrypted = flag == 0x01;

                    int contentLength = reader.ReadInt32();

                    // حماية من القيم غير المنطقية
                    if (contentLength < 0 || contentLength > rawData.Length - 5) // -5 لأننا قرأنا 1 (flag) + 4 (int)
                        throw new InvalidDataException("❌ تنسيق الملف غير صالح: حجم البيانات غير منطقي.");

                    byte[] contentData = reader.ReadBytes(contentLength);

                    if (isEncrypted)
                    {
                        if (string.IsNullOrEmpty(password))
                            throw new InvalidOperationException("❌ الملف مشفّر، الرجاء إدخال كلمة المرور.");

                        try
                        {
                            contentData = Decrypt(contentData, password);
                        }
                        catch
                        {
                            throw new InvalidOperationException("❌ فشل في فك التشفير، تأكد من صحة كلمة المرور.");
                        }
                    }

                    using (MemoryStream contentStream = new MemoryStream(contentData))
                    using (BinaryReader contentReader = new BinaryReader(contentStream))
                    {
                        string relativePath = contentReader.ReadString();
                        int codeCount = contentReader.ReadInt32();

                        var codes = new Dictionary<byte, string>();
                        for (int i = 0; i < codeCount; i++)
                        {
                            byte key = contentReader.ReadByte();
                            string value = contentReader.ReadString();
                            codes[key] = value;
                        }

                        int dataLength = contentReader.ReadInt32();
                        byte[] compressedData = contentReader.ReadBytes(dataLength);

                        byte[] decompressedData = Decode(compressedData, codes);

                        string outputFilePath = Path.Combine(outputDirectory, relativePath.Replace('/', Path.DirectorySeparatorChar));
                        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                        File.WriteAllBytes(outputFilePath, decompressedData);
                    }
                progress?.Report(100);
                }
            });
        }



        private static Dictionary<byte, int> CountFrequencies(byte[] data)
        {
            var freq = new Dictionary<byte, int>();
            foreach (var b in data)
            {
                if (!freq.ContainsKey(b)) freq[b] = 0;
                freq[b]++;
            }
            return freq;
        }

        private static byte[] Encode(byte[] data, Dictionary<byte, string> codes)
        {
            StringBuilder encodedBits = new StringBuilder();
            foreach (byte b in data)
                encodedBits.Append(codes[b]);

            int byteCount = (encodedBits.Length + 7) / 8;
            byte[] result = new byte[byteCount];

            for (int i = 0; i < encodedBits.Length; i++)
            {
                if (encodedBits[i] == '1')
                    result[i / 8] |= (byte)(1 << (7 - i % 8));
            }
            return result;
        }

        private static byte[] Decode(byte[] compressedData, Dictionary<byte, string> codes)
        {
            var reverseCodes = codes.ToDictionary(kv => kv.Value, kv => kv.Key);

            StringBuilder bitString = new StringBuilder();
            foreach (byte b in compressedData)
            {
                for (int i = 7; i >= 0; i--)
                    bitString.Append(((b >> i) & 1) == 1 ? '1' : '0');
            }

            List<byte> originalBytes = new List<byte>();
            StringBuilder temp = new StringBuilder();

            foreach (char bit in bitString.ToString())
            {
                temp.Append(bit);
                if (reverseCodes.ContainsKey(temp.ToString()))
                {
                    originalBytes.Add(reverseCodes[temp.ToString()]);
                    temp.Clear();
                }
            }

            return originalBytes.ToArray();
        }

        public static byte[] Encrypt(byte[] data, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] salt = Encoding.UTF8.GetBytes("your_salt_1234");
                var key = new Rfc2898DeriveBytes(password, salt, 10000);

                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] data, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] salt = Encoding.UTF8.GetBytes("your_salt_1234");
                var key = new Rfc2898DeriveBytes(password, salt, 10000);

                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
}