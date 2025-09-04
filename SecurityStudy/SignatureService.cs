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
            byte[] rawData = Encoding.UTF8.GetBytes(message);
            byte[] encrypedData = null;

            using (var aliceAlgorithm = new ECDiffieHellmanCng(aliceKey))
            using (CngKey bobPubKey =  CngKey.Import(bobPubKeyBlob,CngKeyBlobFormat.EccPublicBlob))
            {
                byte[] symmKey = aliceAlgorithm.DeriveKeyMaterial(bobPubKey);
                Console.WriteLine($"Alice creates this symmetric key with Bobs public key information:{Convert.ToBase64String(symmKey)}");

                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Key = symmKey;
                    aes.GenerateIV();
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // create CryptoStream and encrypt data to send
                        var cs = new CryptoStream(ms, encryptor,CryptoStreamMode.Write);

                        // write initialization vector not encrypted
                        await ms.WriteAsync(aes.IV,0, aes.IV.Length);
                        cs.Write(rawData,0,rawData.Length);
                        cs.Close();
                        encrypedData = ms.ToArray();
                    }
                    aes.Clear();
                }
            }

            Console.WriteLine($"Alice : message is encrpted:{Convert.ToBase64String(encrypedData)}");
            return encrypedData;
        }

        private static void BobReceiveData(byte[] encrptedData)
        {
            Console.WriteLine("Bob receive encrypted data");
            byte[] rawData = null;

            var aes = new AesCryptoServiceProvider();
            int nBytes= aes.BlockSize >> 3;
            byte[] iv = new byte[nBytes];
            for(int i=0;i< iv.Length;i++)
            {
                iv[i] = encrptedData[i];
            }
            using (var blbAlogrithm = new ECDiffieHellmanCng(bobKey))
            using (var alicePubKey = CngKey.Import(alicePubKeyBlob,CngKeyBlobFormat.EccPublicBlob))
            {
                byte[] symmKey = blbAlogrithm.DeriveKeyMaterial(alicePubKey);
                Console.WriteLine($"Bob creates this symmetric key with Bobs public key information:{Convert.ToBase64String(symmKey)}");

                aes.Key = symmKey;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream())
                {
                    // create CryptoStream and encrypt data to send
                    var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
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
            aliceKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256);
            bobKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256);

            alicePubKeyBlob = aliceKey.Export(CngKeyBlobFormat.EccPublicBlob);
            bobPubKeyBlob = bobKey.Export(CngKeyBlobFormat.EccPublicBlob);
        }
    }
}
