using NopFramework.Core.Domains.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Security
{
    public partial class EncryptionService : IEncryptionService
    {
        #region 声明实例
        private readonly SecuritySettings _securitySetting;
        #endregion
        #region 构造函数
        public EncryptionService(SecuritySettings securitySetting)
        {
            this._securitySetting = securitySetting;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 创建密码哈希
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="saltkey">加盐码</param>
        /// <param name="passwordFormat">密码格式（默认散列算法）</param>
        /// <returns>Password hash</returns>
        public string CreateHash(byte[] data, string hashAlgorithm = "SHA1")
        {
            if (String.IsNullOrEmpty(hashAlgorithm))
                hashAlgorithm = "SHA1";
            var algorithm = HashAlgorithm.Create(hashAlgorithm);
            if(algorithm==null)
                throw new ArgumentException("Unrecognized hash name");
            var hashByteArray = algorithm.ComputeHash(data);
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }
        /// <summary>
        ///创建哈希数据
        /// </summary>
        /// <param name="data">用于计算散列的数据</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        /// <returns>Data hash</returns>
        public string CreatePasswordHash(string password, string saltKey, string passwordFormate = "SHA1")
        {
            return CreateHash(Encoding.UTF8.GetBytes(String.Concat(password, saltKey)), passwordFormate);
        }
        /// <summary>
        /// 创建盐键
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string CreateSaltKey(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="encryptionPrivateKey"></param>
        /// <returns></returns>
        public string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _securitySetting.EncryptionKey;
            var tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));
            byte[] buffer = Convert.FromBase64String(cipherText);
            return DecryptTextFromMemory(buffer, tDESalg.Key, tDESalg.IV);
        }
        /// <summary>
        /// 加密文字
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="encryptionPrivateKey"></param>
        /// <returns></returns>
        public string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _securitySetting.EncryptionKey;
            var tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));
            byte[] encryptedBinary = EncryptTextToMemory(plainText, tDESalg.Key, tDESalg.IV);
            return Convert.ToBase64String(encryptedBinary);
        }
        #endregion
        #region 辅助工具
        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv),
                    CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.Unicode.GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }
        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.Unicode))
                    {
                        return sr.ReadLine();
                    }
                }
            }
        }
        #endregion

    }
}
