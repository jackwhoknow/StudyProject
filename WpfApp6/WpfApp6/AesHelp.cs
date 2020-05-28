using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp6
{
    public class AesHelp
    {
        public static byte[] EncryptString(string content, byte[] key, byte[] iv)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, iv);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(content);
                }
                encrypted = ms.ToArray();
                cs.Close();
                ms.Close();
            }
            return encrypted;
        }
        public static string DecryptString(byte[] data, byte[] key, byte[] iv)
        {
            string str = null;
            using (Aes aesAlg = Aes.Create())
            {
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv);
                MemoryStream ms = new MemoryStream(data);
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using (StreamReader sr = new StreamReader(cs))
                {
                    str = sr.ReadToEnd();
                }
                cs.Close();
                ms.Close();
            }
            return str;
        }
        public static void GenKeyIV(string password,out byte[] key,out byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                key = new byte[aes.Key.Length];
                byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
                for(int i=0;i<pwdBytes.Length;i++)
                {
                    key[i] = pwdBytes[i];
                }
                iv = new byte[aes.IV.Length];
                for (int i = 0; i < pwdBytes.Length; i++)
                {
                    iv[i] = pwdBytes[i];
                }
            }
        }
        /// <summary>
        /// 随机生成Key和IV
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static void GenKeyIV(out byte[] key, out byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                key = aes.Key;
                iv = aes.IV;
            }
        }
    }
}
