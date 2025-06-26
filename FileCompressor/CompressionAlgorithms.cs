using System;
using System.Collections.Concurrent;
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
            public string FullPath { get; set; }
            public string RelativePath { get; set; }
            public bool IsDirectory { get; set; }  // ➕ أضف هذا إذا غير موجود
        }

        // تعديل دالة الضغط لتدعم ضغط الملفات مع المسار النسبي
        public static List<CompressionResult> CompressFiles(
       List<FileToCompress> files, string saveDirectory, Algorithm algorithm,
       IProgress<int> progress, string password,
       ManualResetEventSlim pauseEvent, CancellationToken token)
        {
            var results = new ConcurrentBag<CompressionResult>();
            var compressedEntries = new ConcurrentBag<byte[]>();

            var allFileInfos = new List<(string fullPath, string relativePath)>();
            foreach (var file in files)
            {
                if (file.IsDirectory)
                {
                    var innerFiles = Directory.GetFiles(file.FullPath, "*", SearchOption.AllDirectories);
                    foreach (var path in innerFiles)
                    {
                        string relPath = Path.Combine(file.RelativePath,
                            Path.GetRelativePath(file.FullPath, path)).Replace('\\', '/');
                        allFileInfos.Add((path, relPath));
                    }
                }
                else
                {
                    allFileInfos.Add((file.FullPath, file.RelativePath.Replace('\\', '/')));
                }
            }

            int totalFiles = allFileInfos.Count;
            int processedFiles = 0;

            Parallel.ForEach(allFileInfos, new ParallelOptions { CancellationToken = token }, fileInfo =>
            {
                token.ThrowIfCancellationRequested();
                pauseEvent.Wait();

                string fullPath = fileInfo.fullPath;
                string relativePath = fileInfo.relativePath;

                byte[] fileData = File.ReadAllBytes(fullPath);

                var entryData = CompressFileEntry(fileData, relativePath, algorithm, password, out double ratio);
                compressedEntries.Add(entryData);

                results.Add(new CompressionResult
                {
                    AlgorithmName = algorithm.ToString(),
                    OriginalSize = fileData.Length,
                    CompressedData = entryData,
                    CompressionRatio = ratio
                });

                int progressPercentage = Interlocked.Increment(ref processedFiles) * 100 / totalFiles;
                progress?.Report(progressPercentage);
            });

            string archiveName;

            if (files.Count == 1)
            {
                var file = files[0];
                if (file.IsDirectory)
                    archiveName = Path.GetFileName(file.FullPath.TrimEnd(Path.DirectorySeparatorChar)) + ".cmp";
                else
                    archiveName = Path.GetFileNameWithoutExtension(file.FullPath) + ".cmp";
            }
            else
            {
                archiveName = $"Archive_{DateTime.Now:yyyyMMdd_HHmmss}.cmp";
            }

            string archivePath = Path.Combine(saveDirectory, archiveName);

            using (MemoryStream finalArchive = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(finalArchive))
            {
                writer.Write(compressedEntries.Count);

                foreach (var entry in compressedEntries)
                {
                    writer.Write(entry.Length);
                    writer.Write(entry);
                }

                File.WriteAllBytes(archivePath, finalArchive.ToArray());
            }

            progress?.Report(100);

            return results.ToList();
        }


        private static byte[] CompressFileEntry(byte[] fileData, string relativePath, Algorithm algorithm, string password, out double ratio)
        {
            Dictionary<byte, int> frequencies = CountFrequencies(fileData);
            Dictionary<byte, string> codes = (algorithm == Algorithm.Huffman)
                ? Huffman.BuildCodes(frequencies)
                : ShannonFano.BuildCodes(frequencies);

            byte[] compressedData = Encode(fileData, codes);

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)(!string.IsNullOrEmpty(password) ? 0x01 : 0x00));

                byte[] contentBytes;
                using (MemoryStream contentStream = new MemoryStream())
                using (BinaryWriter contentWriter = new BinaryWriter(contentStream))
                {
                    contentWriter.Write(relativePath);
                    contentWriter.Write(codes.Count);
                    foreach (var kvp in codes)
                    {
                        contentWriter.Write(kvp.Key);
                        contentWriter.Write(kvp.Value);
                    }

                    contentWriter.Write(compressedData.Length);
                    contentWriter.Write(compressedData);

                    contentBytes = contentStream.ToArray();
                }

                if (!string.IsNullOrEmpty(password))
                {
                    contentBytes = Encrypt(contentBytes, password);
                }

                writer.Write(contentBytes.Length);
                writer.Write(contentBytes);

                ratio = (1 - (double)compressedData.Length / fileData.Length) * 100.0;
                return ms.ToArray();
            }
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



        public static async Task DecompressFile(string compressedFilePath, string outputDirectory, 
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

                using (MemoryStream ms = new MemoryStream(rawData))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    int fileCount;
                    try
                    {
                        fileCount = reader.ReadInt32(); // 📦 كم ملف داخل الأرشيف
                        if (fileCount <= 0 || fileCount > 100000) // حماية من البيانات التالفة
                            throw new InvalidDataException("❌ عدد الملفات غير منطقي.");
                    }
                    catch
                    {
                        throw new InvalidOperationException("❌ الملف تالف أو ليس ملف أرشيف صالح.");
                    }

                    for (int i = 0; i < fileCount; i++)
                    {
                        token.ThrowIfCancellationRequested();

                        int entryLength = reader.ReadInt32();
                        if (entryLength <= 0 || entryLength > rawData.Length)
                            throw new InvalidDataException("❌ حجم أحد الإدخالات غير منطقي.");

                        byte[] entryData = reader.ReadBytes(entryLength);

                        using (MemoryStream entryStream = new MemoryStream(entryData))
                        using (BinaryReader entryReader = new BinaryReader(entryStream))
                        {
                            byte flag = entryReader.ReadByte();
                            bool isEncrypted = flag == 0x01;

                            int contentLength = entryReader.ReadInt32();
                            if (contentLength <= 0 || contentLength > entryLength - 5)
                                throw new InvalidDataException("❌ تنسيق الإدخال غير صالح.");

                            byte[] contentData = entryReader.ReadBytes(contentLength);

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
                                for (int j = 0; j < codeCount; j++)
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

                                progress?.Report((int)((i + 1) * 100.0 / fileCount));
                            }
                        }
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