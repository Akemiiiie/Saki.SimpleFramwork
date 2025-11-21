using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase.Utils
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public static class DESEncrypt
    {
        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "ltpower.net");
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string text, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.Default.GetBytes(text);
                des.Key = GetMD5Hash(sKey).Substring(0, 8).Select(c => (byte)c).ToArray();
                des.IV = GetMD5Hash(sKey).Substring(0, 8).Select(c => (byte)c).ToArray();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        byte[] encryptedBytes = ms.ToArray();
                        StringBuilder ret = new StringBuilder();
                        foreach (byte b in encryptedBytes)
                        {
                            ret.AppendFormat("{0:X2}", b);
                        }
                        return ret.ToString();
                    }
                }
            }
        }

        #endregion

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "ltpower.net");
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string text, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                int len;
                len = text.Length / 2;
                byte[] inputByteArray = new byte[len];
                for (int x = 0; x < len; x++)
                {
                    int i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = GetMD5Hash(sKey).Substring(0, 8).Select(c => (byte)c).ToArray();
                des.IV = GetMD5Hash(sKey).Substring(0, 8).Select(c => (byte)c).ToArray();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encoding.Default.GetString(ms.ToArray());
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// 模拟Hash
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
