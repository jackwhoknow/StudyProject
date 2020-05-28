using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class ConcurrentStackDemo
    {
        private const int NUM_AES_KEYS = 800000;

        public static string[] _invalidHexValues = { "AF", "BD", "BF", "CF", "DA", "FA", "FE", "FF" };
        private static int taskHexStringRunning = 0;
        private static int Max_Invalid_Hex_Values = 3;

        private static ConcurrentStack<Byte[]> _byteArrayStack;
        private static ConcurrentStack<string> _keysStack;
        private static ConcurrentStack<string> _validateKeys;
        public static void Run()
        {
            var sw = Stopwatch.StartNew();
            _byteArrayStack = new ConcurrentStack<byte[]>();
            _keysStack = new ConcurrentStack<string>();
            _validateKeys= new ConcurrentStack<string>();

        }
        public static void ParallelPartitionGenerateAESKeys(int maxDegree)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegree;
            var sw = Stopwatch.StartNew();
            Parallel.ForEach(Partitioner.Create(1, NUM_AES_KEYS + 1), parallelOptions, range =>
            {
                var aesM = new AesManaged();
                Console.WriteLine("AES Range ({0},{1}. Time: {2})", range.Item1, range.Item2, DateTime.Now.TimeOfDay);
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    aesM.GenerateKey();
                    byte[] result = aesM.Key;
                    _byteArrayStack.Push(result);
                }
            });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static void ConvertAESKeyToHex(Task taskProducer)
        {
            var sw = Stopwatch.StartNew();
            while (taskProducer.Status == TaskStatus.Running || taskProducer.Status == TaskStatus.WaitingToRun ||
                (!_byteArrayStack.IsEmpty))
            {
                Byte[] result;
                if (_byteArrayStack.TryPop(out result))
                {
                    string hexString = ConvertToTextString(result);
                    _keysStack.Push(hexString);
                }
            }
            Debug.WriteLine("HEX: " + sw.Elapsed.ToString());
        }
        private static string ConvertToTextString(Byte[] byteArray)
        {
            var sb = new StringBuilder(byteArray.Length);
            for (int i = 0; i < byteArray.Length; i++)
            {
                sb.Append(byteArray[i].ToString("X2"));
            }
            return sb.ToString();
        }
        private static void ValidateKeys()
        {
            var sw = new Stopwatch();
            const int bufferSize = 100;
            string[] hexStrings = new string[bufferSize];
            string[] validHexStrings = new string[bufferSize];
            while (taskHexStringRunning > 0 || (!_keysStack.IsEmpty))
            {
                int numItems = _keysStack.TryPopRange(hexStrings, 0, bufferSize);
                int numValidKeys = 0;
                for(int i=0;i< numItems;i++)
                {
                    if(IsValidKey(hexStrings[i]))
                    {
                        validHexStrings[numValidKeys++] = hexStrings[i];
                    }
                }
                if (numValidKeys>0)
                {
                    _validateKeys.PushRange(validHexStrings, 0, numValidKeys);
                }
            }
            Console.WriteLine(sw.Elapsed.ToString());
        }
        private static bool IsValidKey(string key)
        {
            var sw = Stopwatch.StartNew();
            int count = 0;
            for (int i = 0; i < _invalidHexValues.Length; i++)
            {
                if (key.Contains(_invalidHexValues[i]))
                {
                    count++;
                    if (count == Max_Invalid_Hex_Values)
                    {
                        return true;
                    }
                    if (((_invalidHexValues.Length - i) + count) < Max_Invalid_Hex_Values)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
