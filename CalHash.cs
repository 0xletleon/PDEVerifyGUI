﻿//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PDEVerifyGUI {
//    internal class CalHash {

//        /// <summary>
//        /// 单个文件计算
//        /// </summary>
//        /// <param name="rootDirPath"></param>
//        /// <param name="filePath"></param>
//        public static void CalFile(string rootDirPath, string filePath) {
//            // 检查文件是否存在
//            if (!File.Exists(filePath)) {
//                Debug.WriteLine("The file does not exist: " + filePath);
//                return;
//            }

//            // 计算相对于根目录的路径
//            string relativePath = Path.GetRelativePath(rootDirPath, filePath);

//            // 替换路径分隔符从 \ 到 /
//            string unixStylePath = relativePath.Replace('\\', '/');

//            string unixStylePathRemoveCache = "";

//            // 检查 Unix 风格的路径中是否包含 "/.cache"
//            if (unixStylePath.Contains(".cache")) {
//                // 去除 "/.cache" 字符串
//                unixStylePathRemoveCache = unixStylePath.Replace(".cache", "");
//            }

//            // 计算文件路径哈希值
//            uint filePathHash = CalculateFilePathHash("/" + unixStylePathRemoveCache);

//            // 读取文件内容并计算哈希值
//            byte[] thisFilerBytes = File.ReadAllBytes(filePath);
//            uint fileContentHash = CalculateFileContentHash(filePathHash, thisFilerBytes);

//            // 打印结果
//            Debug.WriteLine($"File Path: {filePath}");
//            Debug.WriteLine($"File Path Hash: {filePathHash:X8}");
//            Debug.WriteLine($"File Content Hash: {fileContentHash:X8}");

//            // 读取thisFilerBytes 0x10之后的4个字节
//            // 从第 0x10 (16) 字节之后取 4 个字节
//            byte[] fourBytes = new byte[4];
//            Array.Copy(thisFilerBytes, 0x10, fourBytes, 0, 4);
//            uint thisFileHash = BitConverter.ToUInt32(fourBytes, 0);
//            Debug.WriteLine($"This File Hash: {thisFileHash:X8}");

//            if (thisFileHash != fileContentHash) {
//                Debug.WriteLine("File Hash Error!");

//                // 移动旧文件
//                string newFileName = Path.GetFileNameWithoutExtension(filePath) + Path.GetExtension(filePath) + ".old";
//                string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);

//                try {
//                    // 重命名文件，如果目标文件已存在，则覆盖它
//                    File.Move(filePath, newFilePath);
//                    Debug.WriteLine($"File renamed to: {newFilePath}");
//                } catch (IOException ioEx) {
//                    // 处理可能发生的 IO 异常，例如文件不存在
//                    Debug.WriteLine($"Error renaming file {filePath}: {ioEx.Message}");
//                } catch (Exception ex) {
//                    // 处理其他可能的异常
//                    Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
//                }

//                // 将 fourBytes 写入 thisFilerBytes 0x10 处
//                Array.Copy(BitConverter.GetBytes(fileContentHash), 0, thisFilerBytes, 0x10, 4);
//                // 将 thisFilerBytes 写入到 file.FullName
//                File.WriteAllBytes(filePath, thisFilerBytes);
//                Debug.WriteLine(filePathHash + "修复完成!");

//            }
//        }

//        /// <summary>
//        /// 计算目录下的所有文件路径哈希值与crcKey
//        /// </summary>
//        /// <param name="DirPath"></param>
//        public static void CalDir(string DirPath) {
//            // 遍历所有文件
//            // 检查目录路径是否有效
//            if (!Directory.Exists(DirPath)) {
//                Debug.WriteLine("The directory does not exist: " + DirPath);
//                return;
//            }

//            // 创建DirectoryInfo实例
//            DirectoryInfo di = new(DirPath);

//            // 获取目录下的所有文件（包括子目录中的文件）
//            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);

//            // 遍历所有文件
//            foreach (FileInfo file in files) {
//                Debug.WriteLine(file.FullName);
//                if (!file.Name.Contains(".cache")) {
//                    Debug.WriteLine("不包含.cache: " + file.Name);
//                    continue;
//                }

//                // 计算相对于根目录的路径
//                string relativePath = Path.GetRelativePath(DirPath, file.FullName);

//                // 替换路径分隔符从 \ 到 /
//                string unixStylePath = relativePath.Replace('\\', '/');

//                string unixStylePathRemoveCache = "";

//                // 检查 Unix 风格的路径中是否包含 "/.cache"
//                if (unixStylePath.Contains(".cache")) {
//                    // 去除 "/.cache" 字符串
//                    unixStylePathRemoveCache = unixStylePath.Replace(".cache", "");
//                }

//                // 输出结果
//                Debug.WriteLine(unixStylePathRemoveCache);

//                // 计算并输出文件路径和内容的哈希值
//                uint filePathHash = CalculateFilePathHash("/" + unixStylePathRemoveCache);
//                Debug.WriteLine($"File Path Hash: {filePathHash:X8}");

//                // 读取文件内容
//                // 读取文件的全部内容到字节数组
//                byte[] thisFilerBytes = File.ReadAllBytes(file.FullName);

//                uint fileContentHash = CalculateFileContentHash(filePathHash, thisFilerBytes);
//                Debug.WriteLine($"File Content Hash: {fileContentHash:X8}");

//                // 读取thisFilerBytes 0x10之后的4个字节
//                // 从第 0x10 (16) 字节之后取 4 个字节
//                byte[] fourBytes = new byte[4];
//                Array.Copy(thisFilerBytes, 0x10, fourBytes, 0, 4);
//                uint thisFileHash = BitConverter.ToUInt32(fourBytes, 0);
//                Debug.WriteLine($"This File Hash: {thisFileHash:X8}");

//                if (thisFileHash != fileContentHash) {
//                    Debug.WriteLine("File Hash Error!");

//                    // 移动旧文件
//                    string newFileName = Path.GetFileNameWithoutExtension(file.FullName) + Path.GetExtension(file.FullName) + ".old";
//                    string newFilePath = Path.Combine(Path.GetDirectoryName(file.FullName), newFileName);

//                    try {
//                        // 重命名文件，如果目标文件已存在，则覆盖它
//                        File.Move(file.FullName, newFilePath);
//                        Debug.WriteLine($"File renamed to: {newFilePath}");
//                    } catch (IOException ioEx) {
//                        // 处理可能发生的 IO 异常，例如文件不存在
//                        Debug.WriteLine($"Error renaming file {file.FullName}: {ioEx.Message}");
//                    } catch (Exception ex) {
//                        // 处理其他可能的异常
//                        Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
//                    }

//                    // 将 fourBytes 写入 thisFilerBytes 0x10 处
//                    Array.Copy(BitConverter.GetBytes(fileContentHash), 0, thisFilerBytes, 0x10, 4);
//                    // 将 thisFilerBytes 写入到 file.FullName
//                    File.WriteAllBytes(file.FullName, thisFilerBytes);
//                    Debug.WriteLine(filePathHash + "修复完成!");

//                }
//            }
//        }

//        /// <summary>
//        /// 计算文件路径哈希值与crcKey
//        /// </summary>
//        /// <param name="filePath">相对文件路径</param>
//        /// <returns>文件路径哈希值</returns>
//        static uint CalculateFilePathHash(string filePath) {
//            // crcKey字节数组
//            byte[] crcKey = Encoding.UTF8.GetBytes("crc_key");
//            // crcValue ("crc_key" -> 0xC4999A60)
//            uint crcValue = 0x0;
//            // 计算crcKey
//            foreach (byte crcByteValue in crcKey) {
//                crcValue = ProcessByte(crcByteValue, crcValue);
//            }

//            // 相对路径字节数组
//            byte[] filePathBytes = Encoding.UTF8.GetBytes(filePath);
//            // crcValue的初始值
//            uint hashValue = crcValue;
//            // 计算
//            foreach (byte FileByteValue in filePathBytes) {
//                hashValue = ProcessByte(FileByteValue, hashValue);
//            }

//            return hashValue;
//        }

//        /// <summary>
//        /// 计算文件内容哈希值
//        /// </summary>
//        /// <param name="pathHash">文件路径哈希值</param>
//        /// <param name="fileBytes">文件字节数组</param>
//        /// <returns>文件内容哈希值</returns>
//        static uint CalculateFileContentHash(uint pathHash, byte[] fileBytes) {
//            // 使用文件路径哈希作为初始值
//            uint hashValue = pathHash;
//            // 从0x18开始的数据为待计算的数据（也就是6F/6D的位置）
//            byte[] fileData = new byte[fileBytes.Length - 0x18];
//            // 复制数据
//            Array.Copy(fileBytes, 0x18, fileData, 0, fileBytes.Length - 0x18);
//            // 计算
//            foreach (uint byteValue in fileData) {
//                hashValue = ProcessByte(byteValue, hashValue);
//            }
//            return hashValue;
//        }

//        /// <summary>
//        /// 处理字节
//        /// </summary>
//        /// <param name="byteValue">字节值</param>
//        /// <param name="hashValue">文件路径哈希值</param>
//        /// <returns></returns>
//        static uint ProcessByte(uint byteValue, uint hashValue) {
//            byteValue ^= hashValue;
//            byteValue &= 0xFF;
//            hashValue >>= 8;

//            // 使用查找表计算哈希
//            byte[] tableBytes = new byte[4];
//            Array.Copy(LookupTable, byteValue * 4, tableBytes, 0, 4);
//            hashValue ^= BitConverter.ToUInt32(tableBytes, 0);

//            return hashValue;
//        }

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PDEVerifyGUI {
    internal class CalHash {
        /// <summary>
        /// 异步计算单个文件的哈希值并进行验证
        /// </summary>
        /// <param name="rootDirPath">根目录路径</param>
        /// <param name="filePath">文件路径</param>
        public static async Task CalFileAsync(string rootDirPath, string filePath) {
            // 检查文件是否存在
            if (!File.Exists(filePath)) {
                Debug.WriteLine($"The file does not exist: {filePath}");
                return;
            }

            // 计算相对路径并转换为Unix风格
            string relativePath = Path.GetRelativePath(rootDirPath, filePath);
            string unixStylePath = relativePath.Replace('\\', '/');
            string unixStylePathRemoveCache = unixStylePath.Replace(".cache", "");

            // 计算文件路径哈希
            uint filePathHash = CalculateFilePathHash("/" + unixStylePathRemoveCache);

            try {
                // 异步读取文件内容
                await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.Asynchronous);
                Memory<byte> fileBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(fileBytes);

                // 计算文件内容哈希
                uint fileContentHash = CalculateFileContentHash(filePathHash, fileBytes.Span);

                // 输出调试信息
                Debug.WriteLine($"File Path: {filePath}");
                Debug.WriteLine($"File Path Hash: {filePathHash:X8}");
                Debug.WriteLine($"File Content Hash: {fileContentHash:X8}");

                // 读取文件中存储的哈希值
                uint thisFileHash = BinaryPrimitives.ReadUInt32LittleEndian(fileBytes.Span.Slice(0x10, 4));
                Debug.WriteLine($"This File Hash: {thisFileHash:X8}");

                // 如果哈希值不匹配，进行处理
                if (thisFileHash != fileContentHash) {
                    await HandleFileHashMismatchAsync(filePath, fileStream, fileBytes, fileContentHash);
                }
            } catch (IOException ex) {
                Debug.WriteLine($"IO Error occurred while processing file {filePath}: {ex.Message}");
                // 可以在这里添加更多的错误处理逻辑
            } catch (Exception ex) {
                Debug.WriteLine($"An unexpected error occurred while processing file {filePath}: {ex.Message}");
                // 可以在这里添加更多的错误处理逻辑
            }
        }

        /// <summary>
        /// 异步计算目录下所有文件的哈希值
        /// </summary>
        public static async Task CalDirAsync(string dirPath) {
            // 检查目录是否存在
            if (!Directory.Exists(dirPath)) {
                Debug.WriteLine($"The directory does not exist: {dirPath}");
                return;
            }

            // 获取目录下所有文件
            var files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);

            // 并行处理所有文件
            await Parallel.ForEachAsync(files, async (file, token) => {
                if (!file.Contains(".cache"))
                    return;
                await CalFileAsync(dirPath, file);
            });
        }

        /// <summary>
        /// 处理文件哈希不匹配的情况
        /// </summary>
        private static async Task HandleFileHashMismatchAsync(string filePath, FileStream fileStream, Memory<byte> fileBytes, uint fileContentHash) {
            Debug.WriteLine("File Hash Error!");

            // 生成新的文件名（添加.old后缀）
            string newFilePath = Path.Join(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}{Path.GetExtension(filePath)}.old");

            try {
                // 关闭文件流
                await fileStream.DisposeAsync();

                // 重命名原文件
                File.Move(filePath, newFilePath, true);
                Debug.WriteLine($"File renamed to: {newFilePath}");

                // 更新文件内容中的哈希值
                BinaryPrimitives.WriteUInt32LittleEndian(fileBytes.Span.Slice(0x10, 4), fileContentHash);

                // 将更新后的内容写回文件
                await File.WriteAllBytesAsync(filePath, fileBytes.ToArray());
                Debug.WriteLine($"{nameof(fileContentHash)} 修复完成!");
            } catch (IOException ioEx) {
                Debug.WriteLine($"IO Error handling file {filePath}: {ioEx.Message}");
                // 可以在这里添加重试逻辑或其他错误处理
            } catch (Exception ex) {
                Debug.WriteLine($"Error handling file {filePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// 计算文件路径的哈希值
        /// </summary>
        private static uint CalculateFilePathHash(string filePath) {
            Span<byte> crcKey = Encoding.UTF8.GetBytes("crc_key");
            uint crcValue = 0x0;
            foreach (byte crcByteValue in crcKey) {
                crcValue = ProcessByte(crcByteValue, crcValue);
            }

            Span<byte> filePathBytes = Encoding.UTF8.GetBytes(filePath);
            uint hashValue = crcValue;
            foreach (byte fileByteValue in filePathBytes) {
                hashValue = ProcessByte(fileByteValue, hashValue);
            }

            return hashValue;
        }

        /// <summary>
        /// 计算文件内容的哈希值
        /// </summary>
        private static uint CalculateFileContentHash(uint pathHash, ReadOnlySpan<byte> fileBytes) {
            uint hashValue = pathHash;
            ReadOnlySpan<byte> fileData = fileBytes[0x18..];
            foreach (byte byteValue in fileData) {
                hashValue = ProcessByte(byteValue, hashValue);
            }
            return hashValue;
        }

        /// <summary>
        /// 处理单个字节的哈希计算
        /// </summary>
        private static uint ProcessByte(uint byteValue, uint hashValue) {
            byteValue ^= hashValue;
            byteValue &= 0xFF;
            hashValue >>= 8;

            // 使用查找表计算哈希
            byte[] tableBytes = new byte[4];
            Array.Copy(LookupTable, byteValue * 4, tableBytes, 0, 4);
            hashValue ^= BitConverter.ToUInt32(tableBytes, 0);

            return hashValue;
        }

        /// <summary>
        /// 哈希计算使用的查找表
        /// </summary>
        private static readonly byte[] LookupTable = [0x0, 0x00, 0x00, 0x00, 0x96, 0x30, 0x07, 0x77, 0x2C, 0x61, 0x0E, 0xEE, 0xBA, 0x51, 0x09, 0x99, 0x19, 0xC4, 0x6D, 0x07, 0x8F, 0xF4, 0x6A, 0x70, 0x35, 0xA5, 0x63, 0xE9, 0xA3, 0x95, 0x64, 0x9E, 0x32, 0x88, 0xDB, 0x0E, 0xA4, 0xB8, 0xDC, 0x79, 0x1E, 0xE9, 0xD5, 0xE0, 0x88, 0xD9, 0xD2, 0x97, 0x2B, 0x4C, 0xB6, 0x09, 0xBD, 0x7C, 0xB1, 0x7E, 0x07, 0x2D, 0xB8, 0xE7, 0x91, 0x1D, 0xBF, 0x90, 0x64, 0x10, 0xB7, 0x1D, 0xF2, 0x20, 0xB0, 0x6A, 0x48, 0x71, 0xB9, 0xF3, 0xDE, 0x41, 0xBE, 0x84, 0x7D, 0xD4, 0xDA, 0x1A, 0xEB, 0xE4, 0xDD, 0x6D, 0x51, 0xB5, 0xD4, 0xF4, 0xC7, 0x85, 0xD3, 0x83, 0x56, 0x98, 0x6C, 0x13, 0xC0, 0xA8, 0x6B, 0x64, 0x7A, 0xF9, 0x62, 0xFD, 0xEC, 0xC9, 0x65, 0x8A, 0x4F, 0x5C, 0x01, 0x14, 0xD9, 0x6C, 0x06, 0x63, 0x63, 0x3D, 0x0F, 0xFA, 0xF5, 0x0D, 0x08, 0x8D, 0xC8, 0x20, 0x6E, 0x3B, 0x5E, 0x10, 0x69, 0x4C, 0xE4, 0x41, 0x60, 0xD5, 0x72, 0x71, 0x67, 0xA2, 0xD1, 0xE4, 0x03, 0x3C, 0x47, 0xD4, 0x04, 0x4B, 0xFD, 0x85, 0x0D, 0xD2, 0x6B, 0xB5, 0x0A, 0xA5, 0xFA, 0xA8, 0xB5, 0x35, 0x6C, 0x98, 0xB2, 0x42, 0xD6, 0xC9, 0xBB, 0xDB, 0x40, 0xF9, 0xBC, 0xAC, 0xE3, 0x6C, 0xD8, 0x32, 0x75, 0x5C, 0xDF, 0x45, 0xCF, 0x0D, 0xD6, 0xDC, 0x59, 0x3D, 0xD1, 0xAB, 0xAC, 0x30, 0xD9, 0x26, 0x3A, 0x00, 0xDE, 0x51, 0x80, 0x51, 0xD7, 0xC8, 0x16, 0x61, 0xD0, 0xBF, 0xB5, 0xF4, 0xB4, 0x21, 0x23, 0xC4, 0xB3, 0x56, 0x99, 0x95, 0xBA, 0xCF, 0x0F, 0xA5, 0xBD, 0xB8, 0x9E, 0xB8, 0x02, 0x28, 0x08, 0x88, 0x05, 0x5F, 0xB2, 0xD9, 0x0C, 0xC6, 0x24, 0xE9, 0x0B, 0xB1, 0x87, 0x7C, 0x6F, 0x2F, 0x11, 0x4C, 0x68, 0x58, 0xAB, 0x1D, 0x61, 0xC1, 0x3D, 0x2D, 0x66, 0xB6, 0x90, 0x41, 0xDC, 0x76, 0x06, 0x71, 0xDB, 0x01, 0xBC, 0x20, 0xD2, 0x98, 0x2A, 0x10, 0xD5, 0xEF, 0x89, 0x85, 0xB1, 0x71, 0x1F, 0xB5, 0xB6, 0x06, 0xA5, 0xE4, 0xBF, 0x9F, 0x33, 0xD4, 0xB8, 0xE8, 0xA2, 0xC9, 0x07, 0x78, 0x34, 0xF9, 0x00, 0x0F, 0x8E, 0xA8, 0x09, 0x96, 0x18, 0x98, 0x0E, 0xE1, 0xBB, 0x0D, 0x6A, 0x7F, 0x2D, 0x3D, 0x6D, 0x08, 0x97, 0x6C, 0x64, 0x91, 0x01, 0x5C, 0x63, 0xE6, 0xF4, 0x51, 0x6B, 0x6B, 0x62, 0x61, 0x6C, 0x1C, 0xD8, 0x30, 0x65, 0x85, 0x4E, 0x00, 0x62, 0xF2, 0xED, 0x95, 0x06, 0x6C, 0x7B, 0xA5, 0x01, 0x1B, 0xC1, 0xF4, 0x08, 0x82, 0x57, 0xC4, 0x0F, 0xF5, 0xC6, 0xD9, 0xB0, 0x65, 0x50, 0xE9, 0xB7, 0x12, 0xEA, 0xB8, 0xBE, 0x8B, 0x7C, 0x88, 0xB9, 0xFC, 0xDF, 0x1D, 0xDD, 0x62, 0x49, 0x2D, 0xDA, 0x15, 0xF3, 0x7C, 0xD3, 0x8C, 0x65, 0x4C, 0xD4, 0xFB, 0x58, 0x61, 0xB2, 0x4D, 0xCE, 0x51, 0xB5, 0x3A, 0x74, 0x00, 0xBC, 0xA3, 0xE2, 0x30, 0xBB, 0xD4, 0x41, 0xA5, 0xDF, 0x4A, 0xD7, 0x95, 0xD8, 0x3D, 0x6D, 0xC4, 0xD1, 0xA4, 0xFB, 0xF4, 0xD6, 0xD3, 0x6A, 0xE9, 0x69, 0x43, 0xFC, 0xD9, 0x6E, 0x34, 0x46, 0x88, 0x67, 0xAD, 0xD0, 0xB8, 0x60, 0xDA, 0x73, 0x2D, 0x04, 0x44, 0xE5, 0x1D, 0x03, 0x33, 0x5F, 0x4C, 0x0A, 0xAA, 0xC9, 0x7C, 0x0D, 0xDD, 0x3C, 0x71, 0x05, 0x50, 0xAA, 0x41, 0x02, 0x27, 0x10, 0x10, 0x0B, 0xBE, 0x86, 0x20, 0x0C, 0xC9, 0x25, 0xB5, 0x68, 0x57, 0xB3, 0x85, 0x6F, 0x20, 0x09, 0xD4, 0x66, 0xB9, 0x9F, 0xE4, 0x61, 0xCE, 0x0E, 0xF9, 0xDE, 0x5E, 0x98, 0xC9, 0xD9, 0x29, 0x22, 0x98, 0xD0, 0xB0, 0xB4, 0xA8, 0xD7, 0xC7, 0x17, 0x3D, 0xB3, 0x59, 0x81, 0x0D, 0xB4, 0x2E, 0x3B, 0x5C, 0xBD, 0xB7, 0xAD, 0x6C, 0xBA, 0xC0, 0x20, 0x83, 0xB8, 0xED, 0xB6, 0xB3, 0xBF, 0x9A, 0x0C, 0xE2, 0xB6, 0x03, 0x9A, 0xD2, 0xB1, 0x74, 0x39, 0x47, 0xD5, 0xEA, 0xAF, 0x77, 0xD2, 0x9D, 0x15, 0x26, 0xDB, 0x04, 0x83, 0x16, 0xDC, 0x73, 0x12, 0x0B, 0x63, 0xE3, 0x84, 0x3B, 0x64, 0x94, 0x3E, 0x6A, 0x6D, 0x0D, 0xA8, 0x5A, 0x6A, 0x7A, 0x0B, 0xCF, 0x0E, 0xE4, 0x9D, 0xFF, 0x09, 0x93, 0x27, 0xAE, 0x00, 0x0A, 0xB1, 0x9E, 0x07, 0x7D, 0x44, 0x93, 0x0F, 0xF0, 0xD2, 0xA3, 0x08, 0x87, 0x68, 0xF2, 0x01, 0x1E, 0xFE, 0xC2, 0x06, 0x69, 0x5D, 0x57, 0x62, 0xF7, 0xCB, 0x67, 0x65, 0x80, 0x71, 0x36, 0x6C, 0x19, 0xE7, 0x06, 0x6B, 0x6E, 0x76, 0x1B, 0xD4, 0xFE, 0xE0, 0x2B, 0xD3, 0x89, 0x5A, 0x7A, 0xDA, 0x10, 0xCC, 0x4A, 0xDD, 0x67, 0x6F, 0xDF, 0xB9, 0xF9, 0xF9, 0xEF, 0xBE, 0x8E, 0x43, 0xBE, 0xB7, 0x17, 0xD5, 0x8E, 0xB0, 0x60, 0xE8, 0xA3, 0xD6, 0xD6, 0x7E, 0x93, 0xD1, 0xA1, 0xC4, 0xC2, 0xD8, 0x38, 0x52, 0xF2, 0xDF, 0x4F, 0xF1, 0x67, 0xBB, 0xD1, 0x67, 0x57, 0xBC, 0xA6, 0xDD, 0x06, 0xB5, 0x3F, 0x4B, 0x36, 0xB2, 0x48, 0xDA, 0x2B, 0x0D, 0xD8, 0x4C, 0x1B, 0x0A, 0xAF, 0xF6, 0x4A, 0x03, 0x36, 0x60, 0x7A, 0x04, 0x41, 0xC3, 0xEF, 0x60, 0xDF, 0x55, 0xDF, 0x67, 0xA8, 0xEF, 0x8E, 0x6E, 0x31, 0x79, 0xBE, 0x69, 0x46, 0x8C, 0xB3, 0x61, 0xCB, 0x1A, 0x83, 0x66, 0xBC, 0xA0, 0xD2, 0x6F, 0x25, 0x36, 0xE2, 0x68, 0x52, 0x95, 0x77, 0x0C, 0xCC, 0x03, 0x47, 0x0B, 0xBB, 0xB9, 0x16, 0x02, 0x22, 0x2F, 0x26, 0x05, 0x55, 0xBE, 0x3B, 0xBA, 0xC5, 0x28, 0x0B, 0xBD, 0xB2, 0x92, 0x5A, 0xB4, 0x2B, 0x04, 0x6A, 0xB3, 0x5C, 0xA7, 0xFF, 0xD7, 0xC2, 0x31, 0xCF, 0xD0, 0xB5, 0x8B, 0x9E, 0xD9, 0x2C, 0x1D, 0xAE, 0xDE, 0x5B, 0xB0, 0xC2, 0x64, 0x9B, 0x26, 0xF2, 0x63, 0xEC, 0x9C, 0xA3, 0x6A, 0x75, 0x0A, 0x93, 0x6D, 0x02, 0xA9, 0x06, 0x09, 0x9C, 0x3F, 0x36, 0x0E, 0xEB, 0x85, 0x67, 0x07, 0x72, 0x13, 0x57, 0x00, 0x05, 0x82, 0x4A, 0xBF, 0x95, 0x14, 0x7A, 0xB8, 0xE2, 0xAE, 0x2B, 0xB1, 0x7B, 0x38, 0x1B, 0xB6, 0x0C, 0x9B, 0x8E, 0xD2, 0x92, 0x0D, 0xBE, 0xD5, 0xE5, 0xB7, 0xEF, 0xDC, 0x7C, 0x21, 0xDF, 0xDB, 0x0B, 0xD4, 0xD2, 0xD3, 0x86, 0x42, 0xE2, 0xD4, 0xF1, 0xF8, 0xB3, 0xDD, 0x68, 0x6E, 0x83, 0xDA, 0x1F, 0xCD, 0x16, 0xBE, 0x81, 0x5B, 0x26, 0xB9, 0xF6, 0xE1, 0x77, 0xB0, 0x6F, 0x77, 0x47, 0xB7, 0x18, 0xE6, 0x5A, 0x08, 0x88, 0x70, 0x6A, 0x0F, 0xFF, 0xCA, 0x3B, 0x06, 0x66, 0x5C, 0x0B, 0x01, 0x11, 0xFF, 0x9E, 0x65, 0x8F, 0x69, 0xAE, 0x62, 0xF8, 0xD3, 0xFF, 0x6B, 0x61, 0x45, 0xCF, 0x6C, 0x16, 0x78, 0xE2, 0x0A, 0xA0, 0xEE, 0xD2, 0x0D, 0xD7, 0x54, 0x83, 0x04, 0x4E, 0xC2, 0xB3, 0x03, 0x39, 0x61, 0x26, 0x67, 0xA7, 0xF7, 0x16, 0x60, 0xD0, 0x4D, 0x47, 0x69, 0x49, 0xDB, 0x77, 0x6E, 0x3E, 0x4A, 0x6A, 0xD1, 0xAE, 0xDC, 0x5A, 0xD6, 0xD9, 0x66, 0x0B, 0xDF, 0x40, 0xF0, 0x3B, 0xD8, 0x37, 0x53, 0xAE, 0xBC, 0xA9, 0xC5, 0x9E, 0xBB, 0xDE, 0x7F, 0xCF, 0xB2, 0x47, 0xE9, 0xFF, 0xB5, 0x30, 0x1C, 0xF2, 0xBD, 0xBD, 0x8A, 0xC2, 0xBA, 0xCA, 0x30, 0x93, 0xB3, 0x53, 0xA6, 0xA3, 0xB4, 0x24, 0x05, 0x36, 0xD0, 0xBA, 0x93, 0x06, 0xD7, 0xCD, 0x29, 0x57, 0xDE, 0x54, 0xBF, 0x67, 0xD9, 0x23, 0x2E, 0x7A, 0x66, 0xB3, 0xB8, 0x4A, 0x61, 0xC4, 0x02, 0x1B, 0x68, 0x5D, 0x94, 0x2B, 0x6F, 0x2A, 0x37, 0xBE, 0x0B, 0xB4, 0xA1, 0x8E, 0x0C, 0xC3, 0x1B, 0xDF, 0x05, 0x5A, 0x8D, 0xEF, 0x02, 0x2D];


    }
}