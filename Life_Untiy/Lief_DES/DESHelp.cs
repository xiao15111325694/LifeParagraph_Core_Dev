using Microsoft.CodeAnalysis.Options;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Life_Untiy
{
    public class DESHelp
    {
        public EcryptKeyConfig Config;
        public DESHelp(Option<EcryptKeyConfig> option)
        {
            Config = option.DefaultValue;
        }

        public DESHelp()
        {
        }
        #region ========加密========
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return DesEncrypt(text, Config.KeyCode);
            }
            else
            {
                return "";
            }

        }

        public  string DesEncrypt(string text, string key)
        {

            byte[] inputArray = Encoding.UTF8.GetBytes(text);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion



        #region ========解密========
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string Decrypt(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return DesDecrypt(Text, Config.KeyCode);
            }
            else
            {
                return "";
            }
        }

        public  string DesDecrypt(string text, string key)
        {
            byte[] inputArray = Convert.FromBase64String(text);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        #endregion
    }
}
