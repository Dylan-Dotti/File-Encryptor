using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FileEncryptor
{
    public class CryptographyService
    {
        public byte[] Encrypt(string input, string key)
        {
            Encoding.UTF8.GetBytes(input);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = EncodeKey(key);
                aesAlg.IV = GenerateIV(aesAlg.BlockSize);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(
                        msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        public byte[] Encrypt(byte[] data, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = EncodeKey(key);
                aesAlg.IV = GenerateIV(aesAlg.BlockSize);

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }

        public string DecryptToString(byte[] input, string key)
        {
            string decrypted = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = EncodeKey(key);
                aesAlg.IV = GenerateIV(aesAlg.BlockSize);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(input))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            decrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }

        public byte[] Decrypt(byte[] data, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = EncodeKey(key);
                aesAlg.IV = GenerateIV(aesAlg.BlockSize);

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(
                    msEncrypt, cryptoTransform, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(data, 0, data.Length);
                    csEncrypt.FlushFinalBlock();
                    return msEncrypt.ToArray();
                }
            }
        }

        private byte[] EncodeKey(string key)
        {
            using (SHA256 encoder = SHA256.Create())
            {
                return encoder.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        private byte[] GenerateIV(int blockSize)
        {
            return new byte[blockSize / 8];
        }
    }
}
