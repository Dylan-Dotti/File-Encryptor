using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor
{
    public class FileCryptographyService
    {
        private readonly CryptographyService cryptoService;

        public FileCryptographyService()
        {
            cryptoService = new CryptographyService();
        }

        public void EncryptFile(string path, string key)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            File.WriteAllBytes(path, cryptoService.Encrypt(fileBytes, key));
        }

        public void DecryptFile(string path, string key)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            File.WriteAllBytes(path, cryptoService.Decrypt(fileBytes, key));
        }

        public void EncryptFolder(string path, string key)
        {

        }

        public void DecryptFolder(string path, string key)
        {

        }
    }
}
