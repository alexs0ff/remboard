using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Remontinka.Server.Crypto
{
    /// <summary>
    /// ����� ��� ��������������� ������� � �������.
    /// </summary>
    public static class SecureHelper
    {
        #region Public Methods

        /// <summary>
        /// ������� SecureString �� ������.
        /// </summary>
        /// <param name="data">������ ��� ���������.</param>
        /// <returns>��������� ������.</returns>
        public static SecureString CreateSecureString(string data)
        {
            SecureString result = new SecureString();
            foreach (char c in data)
            {
                result.AppendChar(c);
            }
            return result;
        }

        /// <summary>
        /// ������� ��� MD5 �� �������
        /// </summary>
        /// <param name="data">������ ��� ���������.</param>
        /// <returns>��� ������.</returns>
        public static string CreateMD5Hash(string data)
        {
            byte[] hash = null;
            using (MD5CryptoServiceProvider crypt = new MD5CryptoServiceProvider())
            {
                hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// ������� ��� �� ����������� �����.
        /// </summary>
        /// <param name="filePath">���� � �����.</param>
        /// <returns>���</returns>
        public static string CreateMD5HashFile(string filePath)
        {
            string result;
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    BinaryReader reader = new BinaryReader(fileStream);
                    int count = (int)fileStream.Length;
                    byte[] buf = new byte[count];
                    reader.Read(buf, 0, count);
                    byte[] hash = null;
                    using (MD5CryptoServiceProvider crypt = new MD5CryptoServiceProvider())
                    {
                        hash = crypt.ComputeHash(buf);
                    }
                    result = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// ������� ������������� ������ DES3 ����������.
        /// </summary>
        /// <param name="source">����� ��� ����������.</param>
        /// <param name="password">������.</param>
        /// <returns>������������� ������ � base34.</returns>
        public static string EncryptDES3(string source, string password)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] pwdhash, buff;
            pwdhash = provider.ComputeHash(ASCIIEncoding.UTF8.GetBytes(password));
            des.Key = pwdhash;
            des.Mode = CipherMode.ECB;
            buff = UTF8Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
        }

        /// <summary>
        /// �������������� ������ �� DES3 ���������.
        /// </summary>
        /// <param name="source">������.</param>
        /// <param name="password">������.</param>
        /// <returns>�������� ������</returns>
        public static string DecryptDES3(string source, string password)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] pwdhash, buff;
            pwdhash = provider.ComputeHash(ASCIIEncoding.UTF8.GetBytes(password));
            provider = null;
            buff = Convert.FromBase64String(source);
            des.Key = pwdhash;
            des.Mode = CipherMode.ECB;
            return UTF8Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
        }

        #endregion

        #region Public Propeties

        

        #endregion
    }
}
