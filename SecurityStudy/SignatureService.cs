using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace SecurityStudy
{
    internal class SignatureService
    {
        internal static CngKey aliceKeySignature; // 私钥
        internal static byte[] alicePubKeyBlob; //公钥

        public static void Run()
        {
            CreateKeys();

            byte[] aliceData = Encoding.UTF8.GetBytes("Alice");
            byte[] aliceSignature = CreateSignature(aliceData, aliceKeySignature); // 创建签名

            Console.WriteLine($"Alice created signature: {Convert.ToBase64String(aliceSignature)}");

            if(VerifySignature(aliceData,aliceSignature,alicePubKeyBlob))
            {
                Console.WriteLine("Alice signature verified successfully");
            }
        }

        static void CreateKeys()
        {
            aliceKeySignature = CngKey.Create(CngAlgorithm.ECDsaP256);
            alicePubKeyBlob = aliceKeySignature.Export(CngKeyBlobFormat.GenericPublicBlob);
        }

        static byte[] CreateSignature(byte[] data,CngKey key)
        {
            byte[] signature;
            using (var signingAlg = new ECDsaCng(key))
            {
                signature = signingAlg.SignData(data);
                signingAlg.Clear();
            }
            return signature;
        }

        static bool VerifySignature(byte[] data, byte[] signature, byte[] pubKey)
        {
            bool retValue = false;
            using (CngKey key = CngKey.Import(pubKey, CngKeyBlobFormat.GenericPublicBlob))
            using (var signingAlg = new ECDsaCng(key))
            {
                retValue = signingAlg.VerifyData(data,signature);
                signingAlg.Clear();
            }
            return retValue;
        }
    }

    internal class SignatureService2
    {
        static CngKey aliceKey;
        static CngKey bobKey;
        static byte[] alicePubKeyBlob;
        static byte[] bobPubKeyBlob;

        public static void Run()
        {
            DoWork();
        }

        private async static void DoWork()
        {
            try
            {
                CreateKeys();
                byte[] encryptedData = await AliceSendData("How are you!");
                BobReceiveData(encryptedData);
            }
            catch(Exception ex)
            {

            }
        }

        private async static Task<byte[]> AliceSendData(string message)
        {
            Console.WriteLine($"Alice sends message:{message}" );
            // 将字符串消息转换为UTF-8编码的字节数组
            byte[] rawData = Encoding.UTF8.GetBytes(message); 
            byte[] encrypedData = null;

            // 使用Alice的密钥初始化ECDH算法实例
            using (var aliceAlgorithm = new ECDiffieHellmanCng(aliceKey))
            // 导入Bob的公钥数据块
            using (CngKey bobPubKey =  CngKey.Import(bobPubKeyBlob,CngKeyBlobFormat.EccPublicBlob))
            {
                // 通过ECDH算法推导出对称密钥
                byte[] symmKey = aliceAlgorithm.DeriveKeyMaterial(bobPubKey);
                Console.WriteLine($"Alice creates this symmetric key with Bobs public key information:{Convert.ToBase64String(symmKey)}");

                // 使用AES对称加密算法
                using (var aes = new AesCryptoServiceProvider()) //保存密钥(Key)、向量(IV)、模式等参数。
                {
                    aes.Key = symmKey; // 设置加密密钥
                    aes.GenerateIV();  // 生成初始化向量，主要目的是确保即使加密相同的明文，每次产生的密文也会完全不同，从而大大提高安全性。
                    using (ICryptoTransform encryptor = aes.CreateEncryptor()) //根据配置，生产一个加密引擎。
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // create CryptoStream and encrypt data to send
                        // 创建加密流
                        var cs = new CryptoStream(ms, encryptor,CryptoStreamMode.Write); //将数据自动输送给引擎，并处理输出。

                        // 先将初始化向量写入内存流（不加密）
                        // write initialization vector not encrypted
                        await ms.WriteAsync(aes.IV,0, aes.IV.Length);
                        // 通过加密流写入加密数据
                        cs.Write(rawData,0,rawData.Length);
                        cs.Close();
                        encrypedData = ms.ToArray();
                    }
                    aes.Clear(); // 清除密钥信息
                }
            }

            Console.WriteLine($"Alice : message is encrpted:{Convert.ToBase64String(encrypedData)}");
            return encrypedData;
        }

        private static void BobReceiveData(byte[] encrptedData)
        {
            Console.WriteLine("Bob receive encrypted data");
            byte[] rawData = null;

            // 创建AES加密服务提供程序实例
            var aes = new AesCryptoServiceProvider();
            // 计算初始化向量(IV)的字节长度（位除以8）
            int nBytes = aes.BlockSize >> 3;
            // 创建存储IV的字节数组
            byte[] iv = new byte[nBytes];
            for(int i=0;i< iv.Length;i++)
            {
                iv[i] = encrptedData[i];
            }
            // 使用Bob的私钥创建ECDH算法实例
            using (var blbAlogrithm = new ECDiffieHellmanCng(bobKey))
            // 导入Alice的公钥
            using (var alicePubKey = CngKey.Import(alicePubKeyBlob,CngKeyBlobFormat.EccPublicBlob))
            {
                // 推导出与Alice相同的对称密钥
                byte[] symmKey = blbAlogrithm.DeriveKeyMaterial(alicePubKey);
                Console.WriteLine($"Bob creates this symmetric key with Bobs public key information:{Convert.ToBase64String(symmKey)}");

                // 配置AES算法的密钥和初始化向量
                aes.Key = symmKey;
                aes.IV = iv;

                // 创建解密器
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream())
                {
                    // create CryptoStream and encrypt data to send
                    // 创建解密流
                    var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
                    // 写入加密数据（跳过前nBytes字节的IV）
                    cs.Write(encrptedData,nBytes, encrptedData.Length- nBytes);
                    cs.Close();
                    rawData = ms.ToArray();

                    Console.WriteLine($"Bob decrypts message to:{Encoding.UTF8.GetString(rawData)}");
                }

                aes.Clear();
            }
        }

        private static void CreateKeys()
        {
            // 为Alice生成基于ECDH算法的P256曲线密钥对
            //ECDiffieHellmanP256: 椭圆曲线迪菲-赫尔曼算法，使用256位素数域曲线
            aliceKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256);
            // 为Bob生成基于ECDH算法的P256曲线密钥对 
            bobKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256);

            // 导出Alice的公钥数据块（ECC标准格式）
            alicePubKeyBlob = aliceKey.Export(CngKeyBlobFormat.EccPublicBlob);
            // 导出Bob的公钥数据块（ECC标准格式）
            bobPubKeyBlob = bobKey.Export(CngKeyBlobFormat.EccPublicBlob);
        }
    }
}
