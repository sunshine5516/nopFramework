using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Security
{
    /// <summary>
    /// 加密接口服务
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// 创建盐键
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        string CreateSaltKey(int size);
        /// <summary>
        /// 创建哈希密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltKey"></param>
        /// <param name="passwordFormate"></param>
        /// <returns></returns>
        string CreatePasswordHash(string password, string saltKey, string passwordFormate = "SHA1");
        /// <summary>
        /// 创建哈希数据
        /// </summary>
        /// <param name="data">用于计算哈希的数据</param>
        /// <param name="hashAlgorithm">散列算法</param>
        /// <returns></returns>
        string CreateHash(byte[] data, string hashAlgorithm = "SHA1");
        /// <summary>
        /// 加密文字
        /// </summary>
        /// <param name="plainText">要加密的文本</param>
        /// <param name="encryptionPrivateKey">加密私钥</param>
        /// <returns>加密的文本</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");
        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="encryptionPrivateKey"></param>
        /// <returns></returns>
        string DecryptText(string cipherText, string encryptionPrivateKey = "");
    }
}
