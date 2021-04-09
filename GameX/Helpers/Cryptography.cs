using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GameX.Helpers
{
    public class Cryptography
    {
        #region ### MD5 Hash ###
        /// <summary>
        /// Gera uma HASH MD5 pela string fornecida
        /// </summary>
        /// <param name="input">String para geração do hash. (Usar uma GUID de preferência)</param>
        /// <returns>Retorna uma HASH MD5.</returns>
        public static string GenerateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty);
            return encoded;
        }


        /// <summary>
        /// Gera uma HASH MD5 a partir de uma GUID.
        /// </summary>
        /// <returns>Retorna uma HASH MD5.</returns>
        public static string GenerateMD5Hash()
        {
            return GenerateMD5Hash(Guid.NewGuid().ToString());
        }
        #endregion

        #region Base64

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            string strData = "";
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                strData = Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {
                //nao faz nada
                // throw;
            }
            return strData;
        }

        public static bool IsBase64(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        #endregion

        #region AED
        public static string AESEncode(string str)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            //set the mode for operation of the algorithm
            objrij.Mode = CipherMode.CBC;
            //set the padding mode used in the algorithm.
            objrij.Padding = PaddingMode.PKCS7;
            //set the size, in bits, for the secret key.
            objrij.KeySize = 0x80;
            //set the block size in bits for the cryptographic operation.
            objrij.BlockSize = 0x80;
            //set the symmetric key that is used for encryption & decryption.
            byte[] passBytes = Encoding.UTF8.GetBytes("btQt84y#Ukqtv~fm)z#H)PW.+=F:d4id");
            //set the initialization vector (IV) for the symmetric algorithm
            byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            //Creates symmetric AES object with the current key and initialization vector IV.
            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(str);
            //Final transform the test string.
            return ArrayBytesToHexString(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
        }

        public static string AESDecode(string str)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;
            objrij.KeySize = 0x80;
            objrij.BlockSize = 0x80;
            byte[] encryptedTextByte = HexStringToArrayBytes(str);
            byte[] passBytes = Encoding.UTF8.GetBytes("btQt84y#Ukqtv~fm)z#H)PW.+=F:d4id");
            byte[] EncryptionkeyBytes = new byte[0x10];
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);

            return Encoding.UTF8.GetString(TextByte);  //it will return readable string
        }

        private static string ArrayBytesToHexString(byte[] conteudo)
        {
            string[] arrayHex = Array.ConvertAll(
                conteudo, b => b.ToString("X2"));
            return string.Concat(arrayHex);
        }
        private static byte[] HexStringToArrayBytes(string conteudo)
        {
            int qtdeBytesEncriptados =
                conteudo.Length / 2;
            byte[] arrayConteudoEncriptado =
                new byte[qtdeBytesEncriptados];
            for (int i = 0; i < qtdeBytesEncriptados; i++)
            {
                arrayConteudoEncriptado[i] = Convert.ToByte(
                    conteudo.Substring(i * 2, 2), 16);
            }

            return arrayConteudoEncriptado;
        }

        public static bool CheckIsValidAesEncode(string str)
        {
            if (!Regex.IsMatch(str, @"\A\b[0-9a-fA-F]+\b\Z"))
                return false;

            return str.Length % 32 == 0;
        }
        #endregion
    }
}
